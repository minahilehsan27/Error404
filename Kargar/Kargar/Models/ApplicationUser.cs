using Microsoft.AspNetCore.Identity;

namespace Kargar.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;  // Customer / Provider / Admin
        public string Location { get; set; } = string.Empty;
    }
}
