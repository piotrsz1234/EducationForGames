using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace EducationLib.Shared {
	
	[Serializable]
	[BsonNoId]
	public class QuestionAnswer {
		
		[BsonElement("QuestionID")]
		public Guid QuestionID { get; set; }
		[BsonElement("UserID")]
		public Guid UserID { get; set; }
		[BsonElement("Status")]
		public bool Status { get; set; }

		public QuestionAnswer () { }

	}
}
