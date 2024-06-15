using Dan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Application.Contract
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByEmailAsync(string email);
     
        Task<bool> UpdateEmailVerificationAsync(string userId, bool isEmailVerified);
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password);
        Task SaveAsync();

        Task<ApplicationUser> GetUserByIdAsync(string userId);
        
        Task<bool> UpdateUserAsync(ApplicationUser user);
    }
}
