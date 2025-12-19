using KarigarProject.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karigar.Models
{
    public enum BookingStatus
    {
        Requested,
        Confirmed,
        InProgress,
        Completed,
        Cancelled,
        Rejected
    }

    public class Booking
    {
        [Key]
        public int Id { get; set; }

        // Service Information
        public int ServiceId { get; set; }
        public string ServiceTitle { get; set; }
        public string ServiceCategory { get; set; }

        // Customer Information
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }

        // Provider Information
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }

        // Booking Details
        [Required(ErrorMessage = "Booking date is required")]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [StringLength(500, ErrorMessage = "Instructions cannot exceed 500 characters")]
        public string SpecialInstructions { get; set; }

        // Status
        public BookingStatus Status { get; set; } = BookingStatus.Requested;

        // Payment
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = "Pending";

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
    }
}