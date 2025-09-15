using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Helpers
{
    public class AdminSeeder
    {
        private readonly UserManager<User> _userManager;

        public AdminSeeder(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAdminAsync()
        {
            var email = "admin@gmail.com";
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {

                    Email = email,
                    UserName = "Admin"
                };

                var result = await _userManager.CreateAsync(user, "Admin123!");
                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}