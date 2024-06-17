using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;  // 新增這行

namespace Relax.Data
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(256)]
        public string Name { get; set; }

        public int? Points { get; set; }
        [MaxLength(50)]
        public string? Gender { get; set; }
        public DateOnly? BirthDay { get; set; }
    }
}
