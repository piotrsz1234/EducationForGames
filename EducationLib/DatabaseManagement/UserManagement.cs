using MongoDB.Driver;
using EducationLib.Shared;
using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Collections.Generic;

namespace EducationLib.DatabaseManagement {
	public class UserManagement : Management<User> {

		private readonly IMongoCollection<RegistrationCode> codesCollection;

		public UserManagement (IMongoCollection<User> col, IMongoCollection<RegistrationCode> col2) : base (col) {
			codesCollection = col2;
		}

		public async Task<User> GetUserDataAsync (Guid userID) {
			var temp = await collection.FindAsync (x => x.ID == userID);
			return await temp.FirstOrDefaultAsync ();
		}

		public async Task<Guid> LogInAsync (string login, string password) {
			var temp = await collection.FindAsync (x => x.Login == login && x.Password.Equals (password));
			var temp2 = await temp.FirstOrDefaultAsync ();
			if (temp2 == null) return Guid.Empty;
			else return temp2.ID;
		}

		public async Task<Guid> RegisterAsync (User newUser, string code) {
			var search = await collection.FindAsync (x => x.Login == newUser.Login);
			if (search == null || search.Any ()) return Guid.Empty;
			var temp = await (await codesCollection.FindAsync (x => x.Code == code))?.FirstOrDefaultAsync ();
			if (temp == null) return Guid.Empty;
			newUser.SchoolID = temp.SchoolID;
			newUser.Role = temp.Role;
			await codesCollection.FindOneAndDeleteAsync (x => x.Code.Equals (code));
			newUser.ID = Guid.NewGuid ();
			await collection.InsertOneAsync (newUser);
			return newUser.ID;
		}

		public async Task GenerateRegisterCodes (Guid forWho, int howMany, UserRole role) {
			RegistrationCode[] codes = new RegistrationCode[howMany];
			for (int i = 0; i < howMany; i++) codes[i] = new RegistrationCode (forWho, Guid.NewGuid ().ToString (), role);
			await codesCollection.InsertManyAsync (codes);
		}

		public async Task<IEnumerable<User>> GetSchoolStudentsAsync (Guid schoolID) {
			var result = await collection.FindAsync (x => x.SchoolID == schoolID && x.Role == UserRole.Student);
			IEnumerable<User> output = Array.Empty<User> ();
			do {
				output = output.Join (result.Current);
			} while (await result.MoveNextAsync ());
			return output;
		}

		public void AddUser (User user) {
			collection.InsertOne (user);
		}

	}
}
