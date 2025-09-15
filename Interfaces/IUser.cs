using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.DTOs;
using api.DTOs.UserDTO;
using api.Dtos.UserDTO;
using Microsoft.AspNetCore.Identity;


namespace api.Interfaces
{
    public interface IUser
    {

        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string userId);
        Task<string> RegisterAsync(RegisterUserDTO registerUser);
        Task<string> LoginAsync(LoginUserDTO loginUser);
        Task<string> ChangePasswordAsync(ChangePasswordDTO changePassword);
        Task DeleteUserAsync(string userId);
        Task LogoutAsync();
        Task<bool> ActivateUserAsync(string userId);
        Task<List<User>> GetDeactivatedUsersAsync();


    }
}