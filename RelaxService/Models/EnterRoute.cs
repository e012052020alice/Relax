using System.ComponentModel.DataAnnotations;

namespace RelaxService.Models
{
    public class EnterRoute
    {
        [Key]
        public int RouteId { get; set; }
        [MaxLength(50)]
        public string? RouteName { get; set; }
        [MaxLength(50)]
        public string? Device { get; set; }
        public int? Value { get; set; }
        public DateTime DataDate { get; set; }
    }
}
