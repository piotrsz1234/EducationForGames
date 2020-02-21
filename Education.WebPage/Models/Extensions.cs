using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationLib.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Education.WebPage {
	public static class Extensions {
		
		public static T Get<T>(this ISession session, string key) {
			if (!session.Keys.Contains (key)) return default;
			return JsonConvert.DeserializeObject<T> (session.GetString (key));
		}

		public static void Set<T> (this ISession session, string key, T value) {
			session.SetString (key, JsonConvert.SerializeObject (value));
		}

		public static bool IsUserLoggedIn (this Controller controller) {
			if (controller.HttpContext.Session.Get<User> ("User") != null)
				return true;
			return false;
		}

	}
}
