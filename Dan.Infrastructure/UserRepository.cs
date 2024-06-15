using Dan.Application.Contract;
using Dan.Context;
using Dan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DanDbContext _context;
        public UserRepository(UserManager<ApplicationUser> userManager, DanDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return user;
            }
            return null;
        }
        public async Task<bool> UpdateEmailVerificationAsync(string userId, bool isEmailVerified)
        {
            // Convert userId to int (assuming user.Id is an int)
            if (!int.TryParse(userId, out int userIdInt))
                return false; // Handle conversion failure

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userIdInt);
            if (user == null)
                return false;

            user.IsEmailVerified = isEmailVerified;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task SaveAsync()
        {
            // No-op since UserManager handles saving internally.
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }
    }
}
