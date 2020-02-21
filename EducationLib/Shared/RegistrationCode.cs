using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace EducationLib.Shared {
	
	[Serializable]
	[BsonNoId]
	public class RegistrationCode {
		
		[BsonElement("SchoolID")]
		public Guid SchoolID { get; set; }
		[BsonElement("Code")]
		public string Code { get; set; }
		[BsonElement("Role")]
		public UserRole Role { get; set; }

		public RegistrationCode () { }

		public RegistrationCode(Guid schoolID, string code, UserRole role) {
			SchoolID = schoolID;
			Code = code;
			Role = role;
		}

	}
}
