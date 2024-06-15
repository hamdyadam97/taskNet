using Dan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Dan.Context
{
    public class DanDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DanDbContext(DbContextOptions<DanDbContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }
    }
}
