using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EducationLib.APIHelper;
using EducationLib.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebPage.Controllers {
	public class TeacherController : Controller {

		private readonly HttpHelper httpHelper;

		public TeacherController(HttpHelper helper) {
			httpHelper = helper;
		}


		[Route("Teacher/Index")]
		[HttpGet]
		public async Task<IActionResult> IndexAsync () {
			if (!this.IsTeacherLoggedIn ()) return RedirectToAction ("Index", "Home");
			List<Question> model = 
				new List<Question> (await httpHelper.Get<IEnumerable<Question>> ($"Questions/GetTeachersQuestions/{HttpContext.Session.Get<User> ("User").ID}"));
			return View (model);
		}

		[Route("Teacher/ViewStudents")]
		[HttpGet]
		public async Task<IActionResult> ViewSchoolStudentsAsync () {
			if (!this.IsTeacherLoggedIn ()) return RedirectToAction ("Index", "Home");
			List<User> model = new List<User> ();
			var temp = await httpHelper.Get<IEnumerable<User>> ("Users/SchoolStudents/" + HttpContext.Session.Get<User> ("User").ID);
			if (temp != null) model.AddRange (temp);
			return View (model);
		}

		[Route("Teacher/ViewStudentsAnswers/{studentID}")]
		[HttpGet]
		public async Task<IActionResult> ViewStudentsAnswersAsync (Guid studentID) {
			if (!this.IsTeacherLoggedIn ()) return RedirectToAction ("Index", "Home");
			Dictionary<Guid, bool> model = await httpHelper.Get<Dictionary<Guid, bool>> ($"Questions/GetStudentsAnswers/{studentID}");
			return View (model);
		}

	}
}