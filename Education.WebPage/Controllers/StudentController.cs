using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationLib.APIHelper;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebPage.Controllers {
	public class StudentController : Controller {

		private readonly HttpHelper httpHelper;

		public StudentController(HttpHelper helper) {
			httpHelper = helper;
		}

		[Route("Student/Index")]
		public async Task<IActionResult> IndexAsync () {
			if (!this.IsStudentLoggedIn ()) return RedirectToAction ("Index", "User");
			var temp = await httpHelper.Get<Dictionary<Guid, bool>> ("Questions/GetStudentsAnswers/" + this.GetUser ().ID);
			if (temp == null) temp = new Dictionary<Guid, bool> ();
			return View (temp);
		}
	}
}