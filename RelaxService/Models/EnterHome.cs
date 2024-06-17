using System.ComponentModel.DataAnnotations;

namespace RelaxService.Models
{
    public class EnterHome
    {
        [Key]
        public int EnterId { get; set; }
        [MaxLength(100)]
        public string? memberId {  get; set; }
        [MaxLength(100)]
        public string PublicIp { get; set; }
        [MaxLength(50)]
        public string? EnterDevice { get; set; }
        [MaxLength(200)]
        public string? EnterPage { get; set; }

        public DateTime EnterTime { get; set; }
    }
}
