using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Karigar.Data;
using Karigar.Models;
using System.Security.Claims;

namespace Karigar.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ApplicationDbContext context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Customer/Browse
        public async Task<IActionResult> Browse(string category = null, string location = null)
        {
            try
            {
                // Get distinct categories for filter dropdown
                ViewData["Categories"] = await _context.Services
                    .Select(s => s.Category)
                    .Distinct()
                    .ToListAsync();

                ViewData["Locations"] = await _context.Services
                    .Select(s => s.Location)
                    .Distinct()
                    .ToListAsync();

                // Get available services
                IQueryable<Service> services = _context.Services
                    .Where(s => s.IsAvailable);

                if (!string.IsNullOrEmpty(category))
                {
                    services = services.Where(s => s.Category == category);
                }

                if (!string.IsNullOrEmpty(location))
                {
                    services = services.Where(s => s.Location.Contains(location));
                }

                var serviceList = await services.ToListAsync();

                return View(serviceList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error browsing services");
                TempData["ErrorMessage"] = "Error loading services. Please try again.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Customer/Book/5
        public async Task<IActionResult> Book(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            var booking = new Booking
            {
                ServiceId = service.Id,
                ServiceTitle = service.Title,
                ServiceCategory = service.Category,
                ProviderId = service.ProviderId,
                ProviderName = service.ProviderName,
                Amount = service.Price
            };

            return View(booking);
        }

        // POST: Customer/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book([Bind("ServiceId,ServiceTitle,BookingDate,Address,PhoneNumber,SpecialInstructions")] Booking booking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await _context.Users.FindAsync(userId);
                    var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

                    // Get service details
                    var service = await _context.Services.FindAsync(booking.ServiceId);
                    if (service == null)
                    {
                        TempData["ErrorMessage"] = "Service not found.";
                        return RedirectToAction(nameof(Browse));
                    }

                    // Complete booking details
                    booking.CustomerId = userId;
                    booking.CustomerName = profile?.FullName ?? user?.UserName;
                    booking.CustomerEmail = user?.Email;
                    booking.CustomerPhone = profile?.PhoneNumber;
                    booking.ProviderId = service.ProviderId;
                    booking.ProviderName = service.ProviderName;
                    booking.Amount = service.Price;
                    booking.Status = BookingStatus.Requested;
                    booking.CreatedAt = DateTime.UtcNow;

                    _context.Add(booking);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Booking request submitted successfully! The service provider will confirm shortly.";
                    return RedirectToAction(nameof(MyBookings));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating booking");
                TempData["ErrorMessage"] = "Error creating booking. Please try again.";
            }

            // If we got this far, something failed, redisplay form
            return View(booking);
        }

        // GET: Customer/MyBookings
        public async Task<IActionResult> MyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookings = await _context.Bookings
                .Where(b => b.CustomerId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        // POST: Customer/CancelBooking/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Check if user owns this booking
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (booking.CustomerId != userId)
            {
                return Forbid();
            }

            // Only allow cancellation if booking is requested or confirmed
            if (booking.Status == BookingStatus.Requested || booking.Status == BookingStatus.Confirmed)
            {
                booking.Status = BookingStatus.Cancelled;
                booking.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Booking cancelled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Cannot cancel booking in its current status.";
            }

            return RedirectToAction(nameof(MyBookings));
        }

        // GET: Customer/AddReview/5
        public async Task<IActionResult> AddReview(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null || booking.Status != BookingStatus.Completed)
            {
                TempData["ErrorMessage"] = "Cannot review this booking.";
                return RedirectToAction(nameof(MyBookings));
            }

            var review = new Review
            {
                BookingId = booking.Id,
                ServiceId = booking.ServiceId,
                ServiceTitle = booking.ServiceTitle,
                ProviderId = booking.ProviderId,
                ProviderName = booking.ProviderName
            };

            return View(review);
        }

        // POST: Customer/AddReview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview([Bind("BookingId,ServiceId,Rating,Comment")] Review review)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await _context.Users.FindAsync(userId);
                    var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

                    // Complete review details
                    review.CustomerId = userId;
                    review.CustomerName = profile?.FullName ?? user?.UserName;
                    review.CreatedAt = DateTime.UtcNow;

                    _context.Add(review);

                    // Update service rating
                    var service = await _context.Services.FindAsync(review.ServiceId);
                    if (service != null)
                    {
                        // Calculate new average rating
                        var reviews = await _context.Reviews
                            .Where(r => r.ServiceId == review.ServiceId)
                            .ToListAsync();

                        service.Rating = reviews.Average(r => r.Rating);
                        service.ReviewCount = reviews.Count;
                        _context.Services.Update(service);
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Review submitted successfully!";
                    return RedirectToAction(nameof(MyBookings));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting review");
                TempData["ErrorMessage"] = "Error submitting review. Please try again.";
            }

            return View(review);
        }

        // GET: Customer/Profile
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
            {
                // Create new profile if doesn't exist
                var user = await _context.Users.FindAsync(userId);
                profile = new UserProfile
                {
                    UserId = userId,
                    FullName = user?.UserName,
                    Email = user?.Email
                };
            }

            return View(profile);
        }

        // POST: Customer/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfile profile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    profile.UserId = userId;
                    profile.UpdatedAt = DateTime.UtcNow;

                    var existingProfile = await _context.UserProfiles
                        .FirstOrDefaultAsync(p => p.UserId == userId);

                    if (existingProfile == null)
                    {
                        _context.Add(profile);
                    }
                    else
                    {
                        // Update existing profile
                        existingProfile.FullName = profile.FullName;
                        existingProfile.PhoneNumber = profile.PhoneNumber;
                        existingProfile.Address = profile.Address;
                        existingProfile.City = profile.City;
                        existingProfile.State = profile.State;
                        existingProfile.Pincode = profile.Pincode;
                        existingProfile.UpdatedAt = DateTime.UtcNow;

                        _context.Update(existingProfile);
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction(nameof(Profile));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile");
                TempData["ErrorMessage"] = "Error updating profile. Please try again.";
            }

            return View(profile);
        }
    }
}