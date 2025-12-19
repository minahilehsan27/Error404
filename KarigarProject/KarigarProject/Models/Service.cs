using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karigar.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 100000, ErrorMessage = "Price must be between 0 and 100000")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        // Provider Information
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderPhone { get; set; }

        // Service Details
        public string Availability { get; set; } = "Mon-Fri, 9AM-6PM";
        public bool IsAvailable { get; set; } = true;

        // Ratings
        [Range(1, 5)]
        public double Rating { get; set; } = 3.0;
        public int ReviewCount { get; set; } = 0;

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}