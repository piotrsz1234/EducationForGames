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

		public UserController(HttpHelper helper) {
			httpHelper = helper;
		}


		[Route("LogIn")]
		[HttpPost]
		public async Task<IActionResult> LogInAsync (string login, string password) {
			password = password.GetHashCode ().ToString ();
			var temp = new Dictionary<string, string> {
				{ "login", login },
				{ "password", password }
			};
			User user = await httpHelper.Post<User> ("Users/LogIn", temp);
			if (user == null) return RedirectToAction ("Index", "Home");
			HttpContext.Session.Set ("User", user);
			return RedirectToAction ("Index", "User");
		}
		
		[Route("Register")]
		[HttpPost]
		public async Task<IActionResult> RegisterAsync (string login, string password, string registerCode) {
			User user = new User {
				Login = login,
				Password = password.GetHashCode().ToString()
			};
			var response = await httpHelper.Post<Guid> ("Users/Register", new Dictionary<string, string> {
				{ "newUser", JsonConvert.SerializeObject(user) },
				{ "code", registerCode },
			});
			if (response == Guid.Empty) {
				return RedirectToAction ("Register", "Home");
			}
			return await LogInAsync (login, password);
		}
	


	}
}