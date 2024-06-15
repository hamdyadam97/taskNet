using Dan.Application.IServices;
using Dan.Application.Mapper;
using Dan.Application.Services;
using Dan.Application.Contract;
using Dan.Application.IServices;
using Dan.Context;
using Dan.Infrastructure;
using Dan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AliExpress.Application.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Dan.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add DbContext configuration
            builder.Services.AddDbContext<DanDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Add Identity configuration
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<DanDbContext>()
                .AddDefaultTokenProviders();

            // Configure roles and admin user
            await ConfigureRolesAndAdminUser(builder.Services, builder.Configuration);

            // Configure JWT authentication
            ConfigureJwtAuthentication(builder);

            // Set default culture
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            // Add AutoMapper profiles
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new MappingMoto());
                cfg.AddProfile(typeof(MappingUser));
            });

            // Add services
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IMotoRepository, MotoRepository>();
            builder.Services.AddScoped<IMotoService, MotoService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication(); // Ensure authentication middleware is used before authorization middleware

            app.MapControllers();

            app.Run();
        }

        // Method to configure roles and create admin user if not exists
        private static async Task ConfigureRolesAndAdminUser(IServiceCollection services, IConfiguration configuration)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create roles if they don't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Client"))
            {
                await roleManager.CreateAsync(new IdentityRole<int>("Client"));
            }

            // Check if admin user exists
            var adminUserName = configuration["AdminUser:Username"];
            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                // Create admin user
                var newAdminUser = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = configuration["AdminUser:Email"],
                    EmailConfirmed = true // Optionally confirm the email
                };

                var result = await userManager.CreateAsync(newAdminUser, configuration["AdminUser:Password"]);

                if (result.Succeeded)
                {
                    // Add admin role to the user
                    await userManager.AddToRoleAsync(newAdminUser, "Admin");
                }
                else
                {
                    throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors)}");
                }
            }
        }

        // Method to configure JWT authentication
        private static void ConfigureJwtAuthentication(WebApplicationBuilder builder)
        {
            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        }
    }
}
