using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationLib.APIHelper;
using EducationLib.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Education.WebPage.Controllers {
	public class UserController : Controller {

		private readonly HttpHelper httpHelper;

		public UserController (HttpHelper helper) {
			httpHelper = helper;
		}

		[Route ("User/Index")]
		public IActionResult Index () {
			if (!this.IsUserLoggedIn ()) return RedirectToAction ("Index", "Home");
			var user = this.GetUser ();
			if (user.Role == UserRole.Student) return RedirectToAction ("Index", "Student");
			if (user.Role == UserRole.Teacher) return RedirectToAction ("Index", "Teacher");
			if (user.Role == UserRole.School) return RedirectToAction ("Index", "School");
			else return RedirectToAction ("Index", "Admin");
		}

		[Route ("User/LogIn")]
		[HttpPost]
		public IActionResult LogIn (string login, string password) {
			password = password.Encode ();
			var temp = new Dictionary<string, string> {
				{ "login", login },
				{ "password", password }
			};
			User user = httpHelper.Post<User> ($"Users/LogIn/{login}/{password}", temp);
			if (user == null) return RedirectToAction ("Index", "Home");
			HttpContext.Session.Set ("User", user);
			return RedirectToAction ("Index", "User");
		}

		[Route ("User/Register")]
		[HttpPost]
		public IActionResult Register (string login, string password, string registerCode) {
			User user = new User {
				Login = login,
				Password = password.Encode ()
			};
			Dictionary<string, string> temp = new Dictionary<string, string> {
				{ "user", JsonConvert.SerializeObject (user) },
				{ "code", registerCode }
			};
			var response = httpHelper.Post<Guid> ($"Users/Register/{temp["user"]}/{registerCode}", temp);
			Console.WriteLine (response.ToString ());
			if (response == Guid.Empty) {
				return RedirectToAction ("Register", "Home");
			}
			return LogIn (login, password);
		}
	
		public IActionResult LogOut() {
			HttpContext.Session.Clear ();
			return RedirectToActionPermanent ("Index", "Home");
		}

	}
}