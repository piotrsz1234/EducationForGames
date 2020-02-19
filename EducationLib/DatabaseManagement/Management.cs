using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EducationLib.DatabaseManagement {
	public abstract class Management<T> {
		protected readonly IMongoCollection<T> collection;

		public Management (IMongoCollection<T> col) {
			collection = col;
		}

	}
}
