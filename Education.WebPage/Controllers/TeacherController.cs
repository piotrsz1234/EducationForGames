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


		[Route("Index")]
		[HttpGet]
		public async Task<IActionResult> IndexAsync () {
			if (!this.IsUserLoggedIn ()) return RedirectToAction ("Index", "Home");
			List<Question> model =
				await httpHelper.Get<List<Question>> ($"Teacher/GetTeachersQuestions/{HttpContext.Session.Get<User> ("User").ID}");
			return View (model);
		}

		[Route("ViewStudents")]
		[HttpGet]
		public async Task<IActionResult> ViewSchoolStudentsAsync () {
			List<User> model = new List<User> ();
			var temp = await httpHelper.Get<IEnumerable<User>> ("Users/SchoolStudents/" + HttpContext.Session.Get<User> ("User").ID);
			if (temp != null) model.AddRange (temp);
			return View (model);
		}

		[Route("ViewStudentsAnswers/{studentID}")]
		[HttpGet]
		public async Task<IActionResult> ViewStudentsAnswersAsync (Guid studentID) {
			Dictionary<Guid, bool> model = await httpHelper.Get<Dictionary<Guid, bool>> ("Questions/GetStudentsAnswers");
			return View (model);
		}

	}
}