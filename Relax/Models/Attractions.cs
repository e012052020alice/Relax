using System.ComponentModel.DataAnnotations;

namespace Relax.Models
{
    public class Attractions
    {
        [Key]
        public int AttractionId { get; set; }
        public string? memberId { get; set; }
        [MaxLength(50)]
        public string? City { get; set; }
        [MaxLength(100)]
        public string? AttractionName { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        [MaxLength(500)]
        public string? Tag { get; set; }
        [MaxLength(50)]
        public string? TimeCategory { get; set; }
        public int? EstimatedStayTime { get; set; }
        public int? XCoordinate { get; set; }
        public int? YCoordinate { get; set; }
        public bool? IsApproved { get; set; }
    }
}
