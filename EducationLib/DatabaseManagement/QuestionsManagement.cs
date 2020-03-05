using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EducationLib.Shared;
using MongoDB.Driver;

namespace EducationLib.DatabaseManagement {
	public class QuestionsManagement : Management<Question> {
		
		public QuestionsManagement (IMongoCollection<Question> col) : base (col) { }

		public async Task AddQuestionAsync (Question question) {
			question.ID = Guid.NewGuid ();
			await collection.InsertOneAsync (question);
		}

		public async Task<Question> GetQuestionAsync(Guid id) {
			var result = await collection.FindAsync (x => x.ID == id);
			return await result.FirstOrDefaultAsync ();
		}

		public async Task<Question> UpdateQuestionAsync(Guid id, Question newValue) {
			newValue.ID = id;
			var result = await collection.FindAsync (x => x.ID == id);
			if (!result.Any ()) return null;
			await collection.UpdateOneAsync (x => x.ID == id, GenerateUpdateDefinition (newValue));
			return newValue;
		}

		private UpdateDefinition<T> GenerateUpdateDefinition<T> (T t) {
			var temp = typeof (T).GetProperties ();
			UpdateDefinition<T> output = null;
			foreach (var item in temp) {
				if (output == null) output = Builders<T>.Update.Set (item.Name, item.GetValue (t));
				else output = output.Set (item.Name, item.GetValue (t));
			}
			return output;
		}

		public async Task DeleteQuestionAsync (Guid id) {
			var result = await collection.FindAsync (x => x.ID == id);
			if (!result.Any ()) return;
			await collection.DeleteOneAsync (x => x.ID == id);
		}

		public async Task<IEnumerable<Question>> GetTeachersQuestionsAsync(Guid id) {
			var result = await collection.FindAsync (x => x.TeacherID == id);
			if (!await result.AnyAsync ()) return Array.Empty<Question> ();
			List<Question> output =new List<Question> ();
			result = await collection.FindAsync (x => x.TeacherID == id);
			while (await result.MoveNextAsync ()) {
				output.AddRange (result.Current);
			}
			
			return output;
		}

		public async Task<long> HowManyQuestionsAddedAsync (Guid id) {
			var value = await collection.CountDocumentsAsync (x => x.TeacherID == id);
			return value;
		}

	}
}
