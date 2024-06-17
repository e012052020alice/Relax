using System.ComponentModel.DataAnnotations;

namespace RelaxService.DTOs.Analysis
{
    public class EnterRouteDTO
    {
        [MaxLength(50)]
        public string? RouteName { get; set; }
        [MaxLength(50)]
        public string? Device { get; set; }
        public int? Value { get; set; }
        public DateTime DataDate { get; set; }
    }
}
