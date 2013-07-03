using System;
using JR.DAL.Persistence.Sqlite;
namespace JR.DL.Model.Entities
{
    public class FavoriteAdvert2
    {
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Key { get; set; }
        
    }
}
