using Kargar.Data;
using Kargar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kargar.Controllers.Api
{
    [Route("api/services")]
    [ApiController]
    public class ServicesApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetServices()
        {
            var services = _context.Service.ToList();
            return Ok(services);
        }

        [HttpPost("book")]
        public IActionResult BookService([FromBody] Booking booking)
        {
            booking.Status = "Requested";
            booking.BookingDate = DateTime.Now;

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return Ok("Booking Created");
        }
    }
}
