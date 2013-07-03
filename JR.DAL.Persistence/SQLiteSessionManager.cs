using System;
using SQLite;
using JR.DAL.Persistence.Sqlite;

namespace JR.DAL.Persistence
{
	public static class SQLiteSessionManager
	{
		private static ISQLiteConnectionFactory _factory = new SQLiteConnectionFactory();
		
		private static readonly ISQLiteConnection _connection =_factory.Create("db.sql");


		public static ISQLiteConnection Instance{get {return _connection;}}


	}
}
