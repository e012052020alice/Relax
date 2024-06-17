using System.ComponentModel.DataAnnotations;

namespace Relax.Models
{
    public class ScheduleDetails
    {
        [Key]
        public int ScheduleDetailsId { get; set; }
        public int scheduleId { get; set; }
        public int attractionId { get; set; }
        public int DailyTripNumber { get; set; }
    }
}
