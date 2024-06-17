using System.ComponentModel.DataAnnotations;

namespace Relax.Models
{
    public class RandomTripHistory
    {
        [Key]
        public int RandomId { get; set; }
        public string? memberID { get; set; }
        [MaxLength(50)]
        public string? RandomName { get; set; }
        public string? GoogleMapResultLink { get; set; }
    }
}
