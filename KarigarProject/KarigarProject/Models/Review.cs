using System;
using System.ComponentModel.DataAnnotations;

namespace Karigar.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        // References
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceTitle { get; set; }

        // Customer Information
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }

        // Provider Information
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }

        // Review Details
        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
        public string Comment { get; set; }

        public bool IsApproved { get; set; } = true;

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}