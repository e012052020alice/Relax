using System.ComponentModel.DataAnnotations;

namespace Relax.DTO
{
    public class ADDTO
    {

        public int AdId { get; set; }

        public string? MemberId { get; set; }

        public string? UserName { get; set; }

        public int? TripId { get; set; }

        public string? TripName { get; set; }
        public string? Detail { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string? Image { get; set; }

        public int? Clicks { get; set; }

        public int? AdLoved { get; set; }

    }
}
