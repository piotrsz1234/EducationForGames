using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationLib.APIHelper;
using EducationLib.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Education.WebPage.Controllers {
	public class QuestionController : Controller {

		private readonly HttpHelper httpHelper;

		public QuestionController (HttpHelper helper) {
			httpHelper = helper;
		}

		[Route ("Add/{question?}")]
		[HttpGet]
		public ActionResult Add (Question question = null) {
			return View (question);
		}

		[Route ("Add")]
		[HttpPost]
		public ActionResult AddPost (Question question) {
			if (string.IsNullOrWhiteSpace (question.QuestionText) || string.IsNullOrWhiteSpace (question.Answers))
				return RedirectToAction ("Add", "Question", question);
			question.TeacherID = HttpContext.Session.Get<User> ("User").ID;
			httpHelper.Post<object> ("Questions/AddQuestion", question);
			return RedirectToAction ("Index", "Teacher");
		}

		[Route ("Edit/{question:Guid}")]
		[HttpGet]
		public async Task<ActionResult> EditAsync (Guid guid) {
			Question question = await httpHelper.Get<Question> ($"Questions/GetQuestion/{guid}");
			if (question == null) return RedirectToAction ("Index", "Teacher");
			return View (question);
		}

		[Route ("Edit/{question}")]
		[HttpPost]
		public ActionResult Edit (Question question) {
			httpHelper.Post<string> ("Questions/UpdateQuestion", new Dictionary<string, string> () {
				{ "newValue", JsonConvert.SerializeObject(question) },
				{ "id", question.ID.ToString() }
			});
			return RedirectToAction ("Index", "Teacher");
		}

		[Route ("Delete/{id}")]
		[HttpGet]
		public ActionResult Delete (Guid id) {
			httpHelper.Post<string> ("Questions/DeleteQuestion", id);
			return Ok ();
		}

		[Route ("Stats/{id}")]
		[HttpGet]
		public ActionResult Stats (Guid id) {
			Dictionary<Guid, bool> data = new Dictionary<Guid, bool> ();
			var ids = httpHelper.Post<List<Guid>> ("Questions/IDOfUsersWhoAnswered", id);
			foreach (var item in ids) {
				bool value = httpHelper.Post<bool> ("Questions/GetAnswer", new Dictionary<string, string> () {
					{ "user", item.ToString() },
					{ "question", id.ToString() }
				});
				data.Add (item, value);
			}
			return View (data);
		}

	}
}