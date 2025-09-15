using api.Data;
using api.Dtos.UserDTO;
using api.DTOs.UserDTO;
using api.Interfaces;
using api.Models;
using api.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace api.Repositories
{

    public class UserRepository : IUser
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IToken _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly IReview _reviewRepository;

        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IToken tokenService, IHttpContextAccessor httpContextAccessor,
            AppDbContext context, IReview reviewRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _reviewRepository = reviewRepository;



        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await Task.FromResult(_userManager.Users
                .Where(u => u.Email.ToLower() != "admin@gmail.com" && u.isDeleted == false)

                .ToList());
        }


        //user by id
        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }


        //register
        public async Task<string> RegisterAsync(RegisterUserDTO registerUser)
        {

            var existingUserByName = await _userManager.FindByNameAsync(registerUser.UserName);
            if (existingUserByName != null)
            {
                return "Username is already taken.";
            }


            var existingUserByEmail = await _userManager.FindByEmailAsync(registerUser.Email);
            if (existingUserByEmail != null)
            {
                return "Email is already taken.";
            }

            var user = new User
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email
            };


            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {

                var roleExists = await _roleManager.RoleExistsAsync("User");
                if (roleExists)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }

                return "User registered successfully.";
            }

            return string.Join(", ", result.Errors.Select(e => e.Description));
        }



        // log in
        public async Task<string> LoginAsync(LoginUserDTO loginUser)
        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email.ToLower());


            if (user == null)
                return "Invalid login attempt.";

            if (user.isDeleted)
                return "Your account has been deactivated.";

            var result = await _signInManager.PasswordSignInAsync(user, loginUser.Password, false, false);
            if (result.Succeeded)
            {
                var token = _tokenService.CreateTokenAsync(user);
                return await token;
            }

            return "Invalid login attempt.";
        }


        //change password
        public async Task<string> ChangePasswordAsync(ChangePasswordDTO changePassword)
        {

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
                return "User not found.";

            var result = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            if (result.Succeeded)
            {
                return "Password changed successfully.";
            }

            return string.Join(", ", result.Errors.Select(e => e.Description));
        }



        //delete
        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.isDeleted = true;
                await _userManager.UpdateAsync(user);

                var petIds = _context.Reviews
               .Where(r => r.UserId == userId)
               .Select(r => r.PetId)
               .Distinct()
               .ToList();

                foreach (var petId in petIds)
                {
                    await _reviewRepository.UpdateAverageRatingAsync(petId);
                }
            }


        }

        //log out
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> ActivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.isDeleted)
                return false;

            user.isDeleted = false;
            await _userManager.UpdateAsync(user);
            return true;
        }

        //deactivated users
        public async Task<List<User>> GetDeactivatedUsersAsync()
        {
            return await _context.Users
                .Where(u => u.isDeleted == true)
                .ToListAsync();
        }
    }
}