using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EducationLib.APIHelper;
using EducationLib.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebPage.Controllers {

	public class SchoolController : Controller {

		private readonly HttpHelper httpHelper;

		public SchoolController (HttpHelper helper) {
			httpHelper = helper;
		}

		[Route ("School/Index")]
		public async Task<IActionResult> IndexAsync () {
			if (!this.IsSchoolLoggedIn ()) return RedirectToAction ("Index", "User");
			var model = await httpHelper.Get<List<User>> ("Users/SchoolUsers/" + this.GetUser ().ID);
			return View (model);
		}

		[Route ("School/Codes")]
		public async Task<IActionResult> CodesAsync () {
			if (!this.IsSchoolLoggedIn ()) return RedirectToAction ("Index", "User");
			var model = await httpHelper.Get<List<RegistrationCode>> ("Users/SchoolCodes/" + this.GetUser ().ID);
			return View (model);
		}

		[HttpGet]
		public IActionResult AddCodes () {
			if (!this.IsSchoolLoggedIn ()) return RedirectToAction ("Index", "User");
			return View ();
		}

		[HttpPost]
		public async Task<IActionResult> AddCodesAsync (int howMany, int forWho) {
			if (!this.IsSchoolLoggedIn ()) return RedirectToAction ("Index", "User");
			Dictionary<string, object> values = new Dictionary<string, object> {
				{"id", this.GetUser().ID.ToString() },
				{"howMany", howMany },
				{"role", forWho }
			};
			await httpHelper.PostAsync<string> ($"Users/GenerateCode/{this.GetUser().ID}/{howMany}/{forWho}", null);
			return RedirectToAction ("Index", "School");
		}

	}

}