
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EducationLib.DatabaseManagement;
using EducationLib.Shared;
using MongoDB.Driver;

namespace Education.API.Controllers {
	
	[Route ("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase {

		private readonly UserManagement userManagement;

		public UsersController (IMongoCollection<User> collection, IMongoCollection<RegistrationCode> codes) {
			userManagement = new UserManagement (collection, codes);
		}

		[Route("LogIn")]
		[HttpPost]
		public async Task<ActionResult<User>> LogInAsync (string login, string password) {
			var id = await userManagement.LogInAsync (login, password);
			if (id == Guid.Empty) return null;
			return await userManagement.GetUserDataAsync (id);
		}

		[Route("Register")]
		[HttpPost]
		public async Task<ActionResult<Guid>> RegisterAsync (User user, string code) {
			var id = await userManagement.RegisterAsync (user, code);
			return id;
		}

		[Route("GenerateCode")]
		[HttpPost]
		public async Task<ActionResult> GenerateCodeAsync (Guid id, int howMany) {
			await userManagement.GenerateRegisterCodes (id, howMany);
			return Ok ();
		}

	}

}