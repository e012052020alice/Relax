using System.ComponentModel.DataAnnotations;

namespace RelaxService.DTOs.Analysis
{
    public class KeywordDTO
    {
        [MaxLength(50)]
        public string? KeynameA { get; set; }
        [MaxLength(50)]
        public string? KeynameB { get; set; }
        [MaxLength(50)]
        public string? KeynameC { get; set; }
        [MaxLength(50)]
        public string? KeynameD { get; set; }
        [MaxLength(50)]
        public string? KeynameE { get; set; }
        [MaxLength(50)]
        public string? KeynameF { get; set; }
    }
}
