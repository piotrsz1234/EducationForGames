using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationLib.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebPage.Controllers {
	public class HomeController : Controller {

		public IActionResult Index () {
			if (this.IsUserLoggedIn ()) return RedirectToAction ("Index", "User");
			return View ();
		}

		public IActionResult Register () {
			if (this.IsUserLoggedIn ()) return RedirectToAction ("Index", "User");
			return View ();
		}

	}
}