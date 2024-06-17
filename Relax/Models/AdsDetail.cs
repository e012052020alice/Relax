using System.ComponentModel.DataAnnotations;

namespace Relax.Models
{
    public class AdsDetail
    {
        [Key]
        public int AdsDId { get; set; }
        public int adsId { get; set; }
        public string ClickMemberId { get; set; }
        public int AdsClicks { get; set; }
        public int LikeClicks { get; set; }
        public DateTime AdsClicksTime { get; set; }
    }
}
