using System.ComponentModel.DataAnnotations;

namespace Relax.Models
{
    public class Trips
    {
        [Key]
        public int TripId { get; set; }
        public string memberId { get; set; }
        [MaxLength(50)]
        public string? TripName { get; set; }
        public int? TotalDays { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
