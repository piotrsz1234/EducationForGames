using System;
using MongoDB.Bson.Serialization.Attributes;

namespace EducationLib.Shared {
	
	[Serializable]
	public class Question {
		
		[BsonId]
		public Guid ID { get; set; }
		[BsonElement("QuestionText")]
		public string QuestionText { get; set; }
		[BsonElement("Answers")]
		public string Answers { get; set; }
		[BsonElement("CorrectAnswer")]
		public int CorrectAnswer { get; set; }
		[BsonElement("TeacherID")]
		public Guid TeacherID { get; set; }

		[BsonIgnore]
		public string[] SeparatedAnswers {
			get {
				return Answers.Split('\n');
			}
			set {
				Answers = string.Join (Answers, '\n');
			}
		}

	}
}
