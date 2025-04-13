using System.ComponentModel.DataAnnotations;

namespace FlightSimulator.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public int Altitude { get; set; }
        public int Speed { get; set; }
        public string Status { get; set; } // Airborne, Landing, Emergency
    }
}
