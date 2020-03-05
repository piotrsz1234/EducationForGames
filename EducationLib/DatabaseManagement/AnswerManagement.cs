using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EducationLib.Shared;
using MongoDB.Driver;

namespace EducationLib.DatabaseManagement {
	public class AnswerManagement : Management<QuestionAnswer> {

		public AnswerManagement (IMongoCollection<QuestionAnswer> col) : base (col) { }

		public async Task AnswerQuestionAsync (Guid questionId, Guid userID, bool status) {
			var result = await collection.FindAsync (x => x.QuestionID == questionId && x.UserID == userID);
			if (!result.Any ()) {
				QuestionAnswer value = new QuestionAnswer {
					UserID = userID,
					QuestionID = questionId,
					Status = status
				};
				await collection.InsertOneAsync (value);
			} else
				await collection.UpdateOneAsync (x => x.QuestionID == questionId && x.UserID == userID,
					Builders<QuestionAnswer>.Update.Set ("Status", status));
		}

		public async Task<bool> GetAnswerAsync (Guid userID, Guid questionID) {
			var result = await collection.FindAsync (x => x.QuestionID == questionID && x.UserID == userID);
			return (await result.FirstAsync ()).Status;
		}

		public async Task<long> HowManyAnswers (Guid questionID) {
			if (await collection.CountDocumentsAsync (x => true) == 0) return 0;
			long output = await collection.CountDocumentsAsync (x => x.QuestionID == questionID);
			return output;
		}

		public async Task<long> HowManyGoodAnswers (Guid questionID) {
			long output = await collection.CountDocumentsAsync (x => x.Status);
			return output;
		}

		public async Task<IEnumerable<Guid>> IDOfUsersHowAnsweredQuestion (Guid questionID) {
			var result = await collection.FindAsync (x => x.QuestionID == questionID);
			return GetAnswers (result);
		}

		private IEnumerable<Guid> GetAnswers (IAsyncCursor<QuestionAnswer> result) {
			while (result.MoveNext ()) {
				foreach (var item in result.Current) {
					yield return item.UserID;
				}
			}
		}

		public async Task<Dictionary<Guid, bool>> GetStudentAnswersAsync (Guid userID) {
			Dictionary<Guid, bool> output = new Dictionary<Guid, bool> ();
			var result = await collection.FindAsync (x => x.UserID == userID);
			do {
				foreach (var item in result.Current) {
					output.Add (item.QuestionID, item.Status);
				}
			} while (await result.MoveNextAsync ());
			return output;
		}

	}
}
