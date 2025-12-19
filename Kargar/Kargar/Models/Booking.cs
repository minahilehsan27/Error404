using Kargar.Models;

public class Booking
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public string CustomerId { get; set; }
    public DateTime BookingDate { get; set; }
    public string Status { get; set; }       // 🔹 Must exist

    public Service Service { get; set; }
    public ApplicationUser Customer { get; set; }
}
