using System.ComponentModel.DataAnnotations;

namespace Relax.DTO
{
    public class ADTripDetailDTO
    {
        //public int AdId { get; set; }
        public string? AdImg { get; set; }
        public string? Detail { get; set; }

        public int TripId { get; set; }
        public string? TripName { get; set; }
        public int? TotalDays { get; set; }
        public int? DayNumber { get; set; }
        public int? TotalTime { get; set; }
        public int? AttractionId { get; set; }
        public int? DailyNumber { get; set; }
        public string? AttractionName { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
        public string? TimeCategory { get; set; }
        public int? StayTime { get; set; }



    }
}
