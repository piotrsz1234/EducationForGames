using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EducationLib.APIHelper;
using EducationLib.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Education.WebPage.Controllers {
	[Route("[controller]/")]
	public class QuestionController : Controller {

		private readonly HttpHelper httpHelper;

		public QuestionController (HttpHelper helper) {
			httpHelper = helper;
		}

		[Route ("Add/{question?}")]
		[HttpGet]
		public ActionResult Add (Question question = null) {
			if (!this.IsTeacherLoggedIn ()) return RedirectToAction ("Index", "Home");
			if (question.TeacherID != HttpContext.Session.Get<User> ("User").ID)
				question = null;
			return View (question);
		}

		[Route ("Add")]
		[HttpPost]
		public async Task<ActionResult> AddPostAsync (Question question) {
			if (!this.IsTeacherLoggedIn ()) return RedirectToAction ("Index", "Home");
			question.TeacherID = HttpContext.Session.Get<User> ("User").ID;
			if (string.IsNullOrWhiteSpace (question.QuestionText) || question.SeparatedAnswers == null)
				return RedirectToAction ("Add", "Question", question);
			question.SeparatedAnswers = question.SeparatedAnswers.Where (x => !string.IsNullOrEmpty (x)).ToArray ();
			await httpHelper.PostAsync<string> ($"Questions/AddQuestion", question);
			return RedirectToAction ("Index", "Teacher");
		}

		[Route ("Edit/{id}")]
		[HttpGet]
		public async Task<ActionResult> EditAsync (Guid id) {
			if (!this.IsTeacherLoggedIn ()) return RedirectToAction ("Index", "Home");
			Question question = await httpHelper.Get<Question> ($"Questions/GetQuestion/{id}");
			Console.WriteLine (JsonConvert.SerializeObject (question));
			if (question == null) return RedirectToAction ("Index", "Teacher");
			return View (question);
		}

		[Route ("Edit")]
		[HttpPost]
		public async Task<ActionResult> EditAsync (Question question) {
			await httpHelper.PostAsync<object> ("Questions/UpdateQuestion", question);
			return RedirectToAction ("Index", "Teacher");
		}

		[Route ("Delete/{id}")]
		[HttpGet]
		public async Task<ActionResult> DeleteAsync (Guid id) {
			if (!this.IsTeacherLoggedIn ()) return RedirectToAction ("Index", "Home");
			await httpHelper.Get<string> ($"Questions/DeleteQuestion/{id}");
			return RedirectToAction("Index", "Teacher");
		}

		[Route ("Stats/{id}")]
		[HttpGet]
		public async Task<ActionResult> StatsAsync (Guid id) {
			if (!this.IsTeacherLoggedIn ()) return RedirectToAction ("Index", "Home");
			Dictionary<Guid, bool> data = new Dictionary<Guid, bool> ();
			var ids = await httpHelper.Get<List<Guid>> ($"Questions/IDOfUsersWhoAnswered/{id}");
			foreach (var item in ids) {
				bool value = await httpHelper.Get<bool> ($"Questions/GetAnswer/{item}/{id}");
				data.Add (item, value);
			}
			return View (data);
		}

	}
}