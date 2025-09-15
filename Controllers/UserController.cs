using api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using System.Security.AccessControl;
using api.DTOs.UserDTO;
using api.Dtos.UserDTO;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using api.Helpers;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userRepository;

        public UserController(IUser userService)
        {
            _userRepository = userService;
        }

        // register
        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUser)
        {
            var result = await _userRepository.RegisterAsync(registerUser);

            if (result.Contains("already"))
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }


        //log in
        [AllowAnonymous]
        [HttpPost("login-user")]

        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUser)
        {
            var result = await _userRepository.LoginAsync(loginUser);

            if (result == "Invalid login attempt.")
            {
                return Unauthorized(new { message = result });
            }

            if (result == "Your account has been deactivated.")
            {
                return Unauthorized(new { message = result });
            }

            return Ok(result);
        }


        //change password
        [HttpPost("change-password")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePassword)
        {
            var result = await _userRepository.ChangePasswordAsync(changePassword);
            if (result == "Password changed successfully.")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        //delete
        [HttpDelete("delete-user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            await _userRepository.DeleteUserAsync(userId);
            return NoContent();
        }


        //all users
        [HttpGet("all-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        //user by id
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }


        //log out
        [HttpPost("logout")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Logout()
        {
            await _userRepository.LogoutAsync();
            return Ok("User logged out successfully.");
        }

        //activate user
        [HttpPut("activate-user/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            var result = await _userRepository.ActivateUserAsync(id);

            if (!result)
                return NotFound("User not found or already active.");

            return Ok("User activated successfully.");
        }

        //deactivated users
        [HttpGet("deactivated-users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<User>>> GetDeactivatedUsers()
        {
            var users = await _userRepository.GetDeactivatedUsersAsync();
            return Ok(users);
        }


    }
}