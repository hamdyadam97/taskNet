using Dan.Dtos.User;
using Dan.Dtos.ViewResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Application.IServices
{
   public interface IUserService
    {
        Task<ResponceUserDto> RegisterUserAsync(DtoUser registerDto);
        Task<bool> VerifyEmailAsync(string email, string verificationCode);
        Task<bool> UpdateUserDetailsAsync(string userId, UpdateUserDetailsDto updateUserDetailsDto);

    }
}
