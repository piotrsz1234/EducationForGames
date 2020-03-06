using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

		public static void Set(this ISession session, string key, object value) {
			session.SetString (key, JsonConvert.SerializeObject (value));
		}

		public static bool IsUserLoggedIn (this Controller controller) {
			if (controller.HttpContext.Session.Get<User> ("User") != null)
				return true;
			return false;
		}

		public static bool IsUserLoggedIn (this ISession session) {
			if (session.Get<User> ("User") != null)
				return true;
			return false;
		}

		public static bool IsTeacherLoggedIn(this Controller controller) {
			return IsUserWithGivenRoleLoggedIn (controller.HttpContext.Session, UserRole.Teacher);
		}

		public static bool IsSchoolLoggedIn (this Controller controller) {
			return IsUserWithGivenRoleLoggedIn (controller.HttpContext.Session, UserRole.School);
		}

		public static User GetUser(this Controller controller) {
			return controller.HttpContext.Session.Get<User> ("User");
		}

		public static IEnumerable<T> RemoveEmpties<T> (this IEnumerable<T> collection) {
			foreach (var item in collection) {
				if (item.Equals(default (T)) || item == null) continue;
				yield return item;
			}
		}

		public static string Encode (this string rawData) {
			if (rawData == null || string.IsNullOrWhiteSpace (rawData)) return "";
			using (SHA256 sha256Hash = SHA256.Create ()) {
				byte[] bytes = sha256Hash.ComputeHash (Encoding.UTF8.GetBytes (rawData));
				StringBuilder builder = new StringBuilder ();
				for (int i = 0; i < bytes.Length; i++) {
					builder.Append (bytes[i].ToString ("x2"));
				}
				return builder.ToString ();
			}
		}

		public static bool IsUserWithGivenRoleLoggedIn(this ISession session, UserRole userRole) {
			if (!IsUserLoggedIn (session)) return false;
			var role = session.Get<User> ("User").Role;
			if (role == userRole) return true;
			return false;
		}
	
	}
}
