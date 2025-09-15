using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class User : IdentityUser
    {
        public bool isDeleted { get; set; } = false;
    }
}