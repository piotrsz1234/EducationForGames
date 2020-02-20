using System;
using System.Threading.Tasks;
using EducationLib.Shared;
using EducationLib.DatabaseManagement;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Education.API.Controllers {
	
	[Route ("api/[controller]")]
	[ApiController]
	public class QuestionsController : ControllerBase {

		private readonly QuestionsManagement questionManagement;
		private readonly AnswerManagement answerManagement;

		public QuestionsController (IMongoCollection<Question> collection, IMongoCollection<QuestionAnswer> collection2) {
			questionManagement = new QuestionsManagement (collection);
			answerManagement = new AnswerManagement (collection2);
		}

		[Route ("AddQuestion/{question}")]
		[HttpPost]
		public async Task<ActionResult> AddQuestionAsync (Question question) {
			await questionManagement.AddQuestionAsync (question);
			return Ok ();
		}

		[Route ("UpdateQuestion/{newValue}/{id}")]
		[HttpPost]
		public async Task<ActionResult> UpdateQuestionAsync (Question newValue, Guid id) {
			await questionManagement.UpdateQuestionAsync (id, newValue);
			return Ok ();
		}

		[Route ("GetQuestion/{id}")]
		[HttpGet]
		public async Task<ActionResult<Question>> GetQuestionAsync (Guid id) {
			return await questionManagement.GetQuestionAsync (id);
		}

		[Route ("DeleteQuestion/{id}")]
		[HttpPost]
		public async Task<ActionResult> DeleteQuestionAsync (Guid id) {
			await questionManagement.DeleteQuestionAsync (id);
			return Ok ();
		}

		[Route("AddAnswer/{user}/{question}/{status}")]
		[HttpPost]
		public async Task<ActionResult> AddAnswerAsync(Guid user, Guid question, bool status) {
			await answerManagement.AnswerQuestion (question, user, status);
			return Ok ();
		}

	}
}