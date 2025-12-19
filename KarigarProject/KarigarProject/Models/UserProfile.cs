using System;
using System.ComponentModel.DataAnnotations;

namespace Karigar.Models
{
    public class UserProfile
    {
        [Key]
        public string UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }

        public string ProfilePicture { get; set; } = "/images/default-avatar.png";
        public string Bio { get; set; }

        // For Service Providers
        public string Skills { get; set; }
        public string Experience { get; set; }
        public string Certifications { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}