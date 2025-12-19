namespace Kargar.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ServiceProviderId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

}
