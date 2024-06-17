using System.ComponentModel.DataAnnotations;

namespace Relax.Models
{
    public class Tags
    {
        [Key]
        public int TagId { get; set; }
        [MaxLength(50)]
        public string? TagName { get; set; }
    }
}
