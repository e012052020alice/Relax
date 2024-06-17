using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace Relax.DTO
{
	public class ManageNameDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
