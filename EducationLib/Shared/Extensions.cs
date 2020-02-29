using System;
using System.Collections.Generic;
using System.Text;

namespace EducationLib.Shared {
	internal static class Extensions {
	
		public static IEnumerable<T> Join<T> (this IEnumerable<T> col1, IEnumerable<T> col2) {
			foreach (var item in col1) {
				yield return item;
			}
			foreach (var item in col2) {
				yield return item;
			}
		}

	}
}
