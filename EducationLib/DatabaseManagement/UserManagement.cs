using MongoDB.Driver;
using EducationLib.Shared;
using System;
using System.Threading.Tasks;
using MongoDB.Bson;

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
			var temp = await collection.FindAsync (x => x.Login == login && x.Password == password);
			return (await temp.FirstOrDefaultAsync ()).ID;
		}

		public async Task<Guid> RegisterAsync (User newUser, RegistrationCode code) {
			var temp = (await codesCollection.FindAsync (x => x.Equals (code)));
			if (!temp.Any ()) return Guid.Empty;
			newUser.SchoolID = temp.First ().SchoolID;
			_ = (await codesCollection.FindOneAndDeleteAsync (x => x.Equals (code)));
			newUser.ID = Guid.NewGuid ();
			await collection.InsertOneAsync (newUser);
			return newUser.ID;
		}

		public async Task GenerateRegisterCodes (Guid forWho, int howMany) {
			RegistrationCode[] codes = new RegistrationCode[howMany];
			for (int i = 0; i < howMany; i++) codes[i] = new RegistrationCode (forWho, Guid.NewGuid ().ToString ());
			await codesCollection.InsertManyAsync (codes);
		}

	}
}
