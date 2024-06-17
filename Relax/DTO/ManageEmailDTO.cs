using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace Relax.DTO
{
	public class ManageEmailDTO
    {
        public string Id { get; set; }
        public string NewEmail { get; set; }

        public bool IsValidEmail()
        {
            string emailRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            return Regex.IsMatch(NewEmail, emailRegex);
        }

    }
}
