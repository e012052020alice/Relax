namespace Relax.DTO
{
    public class TripDTO
    {
            // Trips 表的属性
            public int TripId { get; set; }
            public string memberId { get; set; }
            public string? TripName { get; set; }
            public int? TotalDays { get; set; }
            public DateTime CreatedAt { get; set; }

            // Schedules 表的属性
            public int ScheduleId { get; set; }
            public int? DayNumber { get; set; }
            public int? TotalStayTime { get; set; }

            // ScheduleDetails 表的属性
            public int ScheduleDetailsId { get; set; }
            public int? AttractionId { get; set; }
            public int? DailyTripNumber { get; set; }

            // Attractions 表的属性
            public string? AttractionName { get; set; }
            public string? Description { get; set; }
            public string? Tag { get; set; }
            public string? TimeCategory { get; set; }
            public int? EstimatedStayTime { get; set; }
    }
}
