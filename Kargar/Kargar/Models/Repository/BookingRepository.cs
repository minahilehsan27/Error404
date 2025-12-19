using Microsoft.EntityFrameworkCore;
using Kargar.Data;
using Kargar.Models.Interface;

namespace Kargar.Models.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public List<Booking> GetAll()
        {
            return _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.Customer)
                .ToList();
        }
    }
}
