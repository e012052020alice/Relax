using System.ComponentModel.DataAnnotations;

namespace Relax.DTO
{
    public class ShareDTO
    {
        public int TripId { get; set; }
        public string? TripName { get; set; }
        public string? MemberId { get; set; }
        public int? TotalDays { get; set; }
        public string? Name { get; set; }
        public int? AdId { get; set; }
        public string? AdImg { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Detail { get; set; }
    }
}
