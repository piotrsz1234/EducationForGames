using System;
using MongoDB.Bson.Serialization.Attributes;

namespace EducationLib.Shared {

	//[Serializable]
	public class User {

		[BsonId]
		public Guid ID { get; set; }
		[BsonElement ("Login")]
		public string Login { get; set; }
		[BsonElement ("Password")]
		public string Password { get; set; }
		[BsonElement ("SchoolID")]
		public Guid SchoolID { get; set; }
		[BsonElement("Role")]
		public UserRole Role { get; set; }

		public User () { }

	}
}
