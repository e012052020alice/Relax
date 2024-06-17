using System.ComponentModel.DataAnnotations;

namespace RelaxService.Models
{
    public class Keyword
    {
        [Key]
        public int KeywordId { get; set; }
        [MaxLength(50)]
        public string? Keyname {  get; set; }
        public int? Value { get; set; }
        public DateTime? DataDate { get; set; }
    }
}
