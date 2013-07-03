using System;
using System.IO;
using SQLite;
using JR.DAL.Persistence;
using JR.DAL.Persistence.Sqlite;

namespace JR.DAL.Persistence
{
	public class SQLiteConnectionFactory : ISQLiteConnectionFactory
	{

		public ISQLiteConnection Create(string address)
		{
			var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			return new SQLiteConnection(Path.Combine(path, address));
		}
	}
}