using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace Relax.DTO
{
	public class ManageDTO
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateOnly? BirthDay { get; set; }

    }
}
