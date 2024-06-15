using Dan.Application.IServices;
using Dan.Context;
using Dan.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Dan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService  _userService;
        private readonly DanDbContext _context;
        public UserController(IUserService userService, DanDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto model)
        {
            var isVerified = await _userService.VerifyEmailAsync(model.Email, model.VerificationCode);

            if (isVerified)
                return Ok(new { message = "Email verified successfully." });
            else
                return BadRequest(new { message = "Invalid email or verification code." });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DtoUser registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.RegisterUserAsync(registerDto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("{userId}/update-details")]
        public async Task<IActionResult> UpdateUserDetails(string userId, [FromBody] UpdateUserDetailsDto updateUserDetailsDto)
        {
            var success = await _userService.UpdateUserDetailsAsync(userId, updateUserDetailsDto);
            if (!success)
                return NotFound();

            return Ok("User details updated successfully.");
        }
        
        [HttpGet("client")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDashboardData()
        {
            var totalUsers = await _context.Users.CountAsync();
            var usersWithMotos = await _context.Users.Where(u => u.AccountMoto != null).CountAsync();

            var totalMotoData = await _context.Motos
                .Select(m => new
                {
                    TotalCount = m.Total,
                    TotalPrice = m.Total * m.Price
                })
                .FirstOrDefaultAsync();

            var data = new
            {
                TotalUsers = totalUsers,
                UsersWithMotos = usersWithMotos,
                TotalMotoCount = totalMotoData?.TotalCount ?? 0,
                TotalMotoPrice = totalMotoData?.TotalPrice ?? 0
            };

            return Ok(data);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponceUserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new ResponceUserDto
                {
                    Email = u.Email,
                    DisplayName = u.DisplayName,
                    Id = u.Id

                    // Map other properties as needed
                })
                .ToListAsync();

            return users;
        }
    
  
    }

}

