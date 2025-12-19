namespace Kargar.Models.Interface
{
    public interface IBookingRepository
    {
        void Create(Booking booking);
        List<Booking> GetAll();
    }
}
