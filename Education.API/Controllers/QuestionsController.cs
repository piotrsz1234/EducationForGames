using System;
using System.Threading.Tasks;
using EducationLib.Shared;
using EducationLib.DatabaseManagement;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;

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
			await answerManagement.AnswerQuestionAsync (question, user, status);
			return Ok ();
		}

		[Route("GetAnswer/{user}/{question}")]
		[HttpPost]
		public async Task<ActionResult<bool>> GetAnswerAsync(Guid user, Guid question) {
			return await answerManagement.GetAnswerAsync (user, question);
		}

		[Route("GoodAnswersPercent/{id}")]
		[HttpGet]
		public async Task<ActionResult<float>> GoodAnswersPercentAsync (Guid id) {
			long answersCount = await answerManagement.HowManyAnswers (id);
			if (answersCount == 0) return 0f;
			long goodAnswersCount = await answerManagement.HowManyGoodAnswers (id);
			return goodAnswersCount / (float) answersCount;
		}

		[Route("IDOfUsersWhoAnswered/{id}")]
		[HttpGet]
		public async IAsyncEnumerable<Guid> IDOfUsersWhoAnsweredAsync(Guid id) {
			IEnumerable<Guid> ids = await answerManagement.IDOfUsersHowAnsweredQuestion (id);
			foreach (var item in ids) {
				yield return item;
			}
		}

		[Route("GetTeachersQuestion/{teacherID}")]
		[HttpGet]
		public async IAsyncEnumerable<Question> GetTeachersQuestionsAsync (Guid teacherID) {
			var enumerable = await questionManagement.GetTeachersQuestionsAsync (teacherID);
			foreach (var item in enumerable) {
				yield return item;
			}
		}

		[Route("GetStudentsAnswers/{id}")]
		[HttpGet]
		public async Task<ActionResult<Dictionary<Guid, bool>>> GetStudentsAnswersAsync(Guid id) {
			return await answerManagement.GetStudentAnswersAsync (id);
		}

	}
}