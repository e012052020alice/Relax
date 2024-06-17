using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace Relax.DTO
{
    public class ManagePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public bool IsValidPassword()
        {
            string pattern = @"^(?=.*[a-zA-Z])(?=.*\d).{6,}$";
            return Regex.IsMatch(NewPassword, pattern);
        }
    }

}
