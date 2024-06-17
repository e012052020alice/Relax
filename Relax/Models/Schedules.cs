using System.ComponentModel.DataAnnotations;

namespace Relax.Models
{
    public class Schedules
    {
        [Key]
        public int ScheduleId { get; set; }
        public int tripID { get; set; }
        public int? DayNumber { get; set; }
        public int? TotalStayTime { get; set; }
    }
}
