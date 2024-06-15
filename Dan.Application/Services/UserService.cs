using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Dan.Application.IServices;
using Dan.Application.Contract;
using Dan.Application.IServices;
using Dan.Dtos.User;
using Dan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;

namespace Dan.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<bool> UpdateUserDetailsAsync(string userId, UpdateUserDetailsDto updateUserDetailsDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            // Update user details based on DTO
            user.DisplayName = updateUserDetailsDto.DisplayName;
            user.IdNational = updateUserDetailsDto.IdNational;
            user.PhoneNumber = updateUserDetailsDto.PhoneNumber;
            user.AccountMoto = updateUserDetailsDto.AccountMoto;
            user.File = updateUserDetailsDto.File;
            user.RulesAgreed = updateUserDetailsDto.RulesAgreed;
            user.Agreement = updateUserDetailsDto.Agreement;
            user.PhoneCountryCode = updateUserDetailsDto.PhoneCountryCode;

            // Save changes to database
            await _userRepository.UpdateUserAsync(user);

            return true;
        }
        public async Task<bool> VerifyEmailAsync(string email, string verificationCode)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null || user.EmailVerificationCode != verificationCode)
                return false;
            string userIdString = user.Id.ToString();
            // Update user's email verification status
            return await _userRepository.UpdateEmailVerificationAsync(userIdString, true);
        }
        public async Task<ResponceUserDto> RegisterUserAsync(DtoUser registerDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("Email already in use.");
            }

            var user = _mapper.Map<ApplicationUser>(registerDto);
            user.DateJoined = DateTime.UtcNow;
            user.EmailVerificationCode = new Random().Next(100000, 999999).ToString();

            var createdUser = await _userRepository.CreateUserAsync(user, registerDto.Password);

            if (createdUser == null)
            {
                throw new Exception("User registration failed.");
            }

            // Generate JWT token
            var token = GenerateJwtToken(createdUser);

            // Send email confirmation code
            await SendEmailVerificationAsync(user.Email, user.EmailVerificationCode);

            return new ResponceUserDto
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                DisplayName = createdUser.DisplayName,
                Token = token
            };

        }
            private string GenerateJwtToken(ApplicationUser user)
            {
                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Issuer"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            private async Task SendEmailVerificationAsync(string email, string code)
            {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("hamdyadam543@gmail.com"); // Replace with your sender email
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = "Verify Your noon Account";
         
            mailMessage.Body = string.Format("Your verification code is: {0}", code);
            mailMessage.IsBodyHtml = false; // Set to true for HTML formatting (sanitize user input)

            var smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com"; // Replace with your SMTP server address
            smtpClient.Port = 587; // Replace with your SMTP port (may vary)
            smtpClient.EnableSsl = true; // Use SSL for secure communication
            smtpClient.Credentials = new NetworkCredential("hamdyadam543@gmail.com", "dpwp arsg vqjy nmns"); // Replace with your SMTP credentials

            smtpClient.Send(mailMessage);
            

        }
        }

    }


