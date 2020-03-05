using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationLib.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebPage.Controllers {
	public class SchoolController : Controller {
		public IActionResult Index () {
			return View (HttpContext.Session.Get<User>("User").ID);
		}
	}
}