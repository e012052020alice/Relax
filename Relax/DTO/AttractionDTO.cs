using System.ComponentModel.DataAnnotations;

namespace Relax.DTO
{
    public class AttractionDTO
    {
        public int AttractionId { get; set; }
        [MaxLength(100)]
        public string? memberId { get; set; }
        public string? AttractionName { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
        public string? TimeCategory { get; set; }
        public int? EstimatedStayTime { get; set; }
        public int? XCoordinate { get; set; }
        public int? YCoordinate { get; set; }
        public bool? IsApproved { get; set; }
    }

}
