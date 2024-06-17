using System.ComponentModel.DataAnnotations;

namespace Relax.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }
        public string memberId { get; set; }
        [MaxLength(50)]
        public string? Title { get; set; }
        public DateOnly? StartTime { get; set; }
        public DateOnly? EndTime { get; set; }
        //varbinary(MAX):是否需要調整
        public string? Image { get; set; }
        [MaxLength(200)]
        public string? Detail { get; set; }
        //廣告追蹤url
        public string? ActivityUrl { get; set; }
        [MaxLength(50)]
        public string? Position { get; set; }

    }
}
