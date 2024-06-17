using System.ComponentModel.DataAnnotations;

namespace RelaxService.DTOs
{
    public class AttractionsDTO
    {
        public int AttractionId { get; set; }
        [MaxLength(100)]
        public string? memberId { get; set; }
        public string? City { get; set; }
        public string? AttractionName { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
        public bool? IsApproved { get; set; }
    }
}
