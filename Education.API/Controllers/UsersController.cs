using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EducationLib.DatabaseManagement;
using EducationLib.Shared;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Education.API.Controllers {

	[Route ("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase {

		private readonly UserManagement userManagement;

		public UsersController (IMongoCollection<User> collection, IMongoCollection<RegistrationCode> codes) {
			userManagement = new UserManagement (collection, codes);
		}

		[Route ("LogIn/{login}/{password}")]
		[HttpPost]
		public async Task<ActionResult<User>> LogInAsync (string login, string password) {
			var id = await userManagement.LogInAsync (login, password);
			if (id == Guid.Empty) return null;
			return await userManagement.GetUserDataAsync (id);
		}

		[Route ("Register/{user}/{code}")]
		[HttpPost]
		public async Task<ActionResult<Guid>> RegisterAsync (string user, string code) {
			var id = await userManagement.RegisterAsync (JsonConvert.DeserializeObject<User> (user), code);
			return id;
		}

		[Route ("GenerateCode/{id}/{howMany}/{role}")]
		[HttpPost]
		public async Task<ActionResult<string>> GenerateCodeAsync (Guid id, int howMany, UserRole role) {
			await userManagement.GenerateRegisterCodes (id, howMany, role);
			return "";
		}

		[Route ("SchoolStudents/{schoolID}")]
		[HttpGet]
		public async IAsyncEnumerable<User> SchoolStudentsAsync (Guid schoolID) {
			var ids = await userManagement.GetSchoolStudentsAsync (schoolID);
			foreach (var item in ids) {
				yield return item;
			}
		}

		[Route ("SchoolUsers/{schoolID}")]
		[HttpGet]
		public async IAsyncEnumerable<User> SchoolUsersAsync (Guid schoolID) {
			var temp = await userManagement.GetSchoolUsersAsync (schoolID);
			foreach (var item in temp) {
				yield return item;
			}
		}

		[Route("SchoolCodes/{schoolID}")]
		[HttpGet]
		public async IAsyncEnumerable<RegistrationCode> SchoolCodesAsync(Guid schoolID) {
			var codes = await userManagement.GetCodesAsync (schoolID);
			foreach (var item in codes) {
				yield return item;
			}
		}

	}

}