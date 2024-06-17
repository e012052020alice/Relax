using System.ComponentModel.DataAnnotations;

namespace Relax.DTO
{
    public class ADTripDTO
    {
        public int AdId { get; set; }
        public string? MemberId { get; set; }
        public string? Name { get; set; }
        public string? AdImg { get; set; }
        public string? Detail { get; set; }
        public int TripId { get; set; }
        public string? TripName { get; set; }
        public int? TotalDays { get; set; }
    }
}
