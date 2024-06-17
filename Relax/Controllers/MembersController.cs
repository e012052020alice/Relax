using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Relax.Data;
using Relax.DTO;
using Relax.Models;
using System.Security.Principal;


namespace Relax.Controllers
{
    [EnableCors("AllowMy")]

    public class MembersController : Controller
    {
        [Authorize]
        public IActionResult Share()
        {
            return View();
        }

		public IActionResult Manage()
		{
			return View();

		}

    }
}
