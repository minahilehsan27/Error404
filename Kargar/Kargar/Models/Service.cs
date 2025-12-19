namespace Kargar.Models
{
    public class Service
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Skill { get; set; } = string.Empty;  // Electrician, Plumber
        public string Location { get; set; } = string.Empty;
        public double Rating { get; set; }
    }
}
