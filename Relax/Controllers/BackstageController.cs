using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Relax.Data;
using Relax.Models;
using static System.Net.Mime.MediaTypeNames;


namespace Relax.Controllers
{
    [EnableCors("AllowMy")]
    [Authorize(Roles = "Staff")]
    public class BackstageController : Controller
	{

        public IActionResult ShareBack()
        {
            return View();
        }
        public IActionResult MemdersBack()
        {
            return View();
        }
        public IActionResult ActiveBack()
        {
            return View();
        }
        public IActionResult TripBack()
        {
            return View();
        }

        public IActionResult Analysis()
		{
			return View();
		}
        public IActionResult UserBehavior()
        {
            return View();
        }
        public IActionResult Sensitivity()
		{
			return View();
		}
	}
}
