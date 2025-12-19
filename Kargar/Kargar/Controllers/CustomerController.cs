using Kargar.Models;
using Kargar.Models.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Kargar.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly IBookingRepository _bookingRepo;

        public CustomerController(
            IServiceRepository serviceRepo,
            IBookingRepository bookingRepo)
        {
            _serviceRepo = serviceRepo;
            _bookingRepo = bookingRepo;
        }

        // GET: Customer/Dashboard
        public IActionResult Dashboard()
        {
            // Get all services to display in dashboard
            var services = _serviceRepo.GetAll();
            return View(services);
        }

        // POST: Customer/Book
        [HttpPost]
        public IActionResult Book(int serviceId)
        {
            // Get current logged-in user ID
            var userId = User.Identity.Name; // or use User.FindFirstValue(ClaimTypes.NameIdentifier)

            // Create booking object
            var booking = new Booking
            {
                ServiceId = serviceId,
                CustomerId = userId,
                BookingDate = DateTime.Now,
                Status = "Requested"
            };

            // Save booking in DB
            _bookingRepo.Create(booking);

            // Optional: Add TempData message to show confirmation
            TempData["SuccessMessage"] = "Service booked successfully!";

            return RedirectToAction("Dashboard");
        }

        // Optional: View My Bookings
        public IActionResult MyBookings()
        {
            var userId = User.Identity.Name;
            var bookings = _bookingRepo.GetAll()
                .Where(b => b.CustomerId == userId)
                .ToList();

            return View(bookings);
        }
    }
}
