using System.ComponentModel.DataAnnotations;

namespace RelaxService.Models
{
    public class Advertisements
    {
        [Key]
        public int AdId { get; set; }
        public int? tripId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsCompleted { get; set; }
        //varbinary(MAX):是否需要調整
        public string? Image { get; set; }
        //廣告點擊次數
        public int? Clicks { get; set; }
        //廣告按愛心次數
        public int? AdLoved { get; set; }
        //廣告追蹤url
        public string? AdUrl { get; set; }
    }
}
