using Kargar.Data;
using Kargar.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Kargar.Controllers
{
    public class ProviderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hub;

        public ProviderController(ApplicationDbContext context,
            IHubContext<NotificationHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task<IActionResult> Accept(int id)
        {
            var booking = _context.Bookings.Find(id);
            booking.Status = "Confirmed";
            _context.SaveChanges();

            await _hub.Clients.All.SendAsync(
                "ReceiveNotification",
                "Your booking has been confirmed!"
            );

            return RedirectToAction("Requests");
        }
    }

}
