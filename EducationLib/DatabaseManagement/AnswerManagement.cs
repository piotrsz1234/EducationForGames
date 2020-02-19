using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EducationLib.Shared;
using MongoDB.Driver;

namespace EducationLib.DatabaseManagement {
	public class AnswerManagement : Management<QuestionAnswer> {

		public AnswerManagement (IMongoCollection<QuestionAnswer> col) : base (col) { }

		public async Task AnswerQuestion(Guid questionId, Guid userID, bool status) {
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

	}
}
