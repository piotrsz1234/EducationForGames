using System;
using MongoDB.Bson.Serialization.Attributes;

namespace EducationLib.Shared {
	
	[Serializable]
	public class Question {
		
		[BsonId]
		public Guid ID { get; set; }
		[BsonElement("QuestionText")]
		public string QuestionText { get; set; }
		[BsonElement("CorrectAnswer")]
		public int CorrectAnswer { get; set; }
		[BsonElement("TeacherID")]
		public Guid TeacherID { get; set; }

		[BsonElement]
		public string[] SeparatedAnswers { get; set; }

	}
}
