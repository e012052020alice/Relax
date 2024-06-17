using System.ComponentModel.DataAnnotations;

namespace Relax.Models
{
    public class Points
    {
        [Key]
        public int PointId { get; set; }
        public string? memberID { get; set; }
        public DateTime? ChangeTime { get; set; }
        [MaxLength(100)]
        public string? ChangeReason { get; set; }
        [MaxLength(50)]
        public string? ChangeValue { get; set; }
        public int? RemainingPoints { get; set; }
    }
}
