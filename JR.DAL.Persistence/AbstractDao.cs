using System;
using System.Collections.Generic;
using JR.DAL.Persistence.Sqlite;
using System.Linq;

namespace JR.DAL.Persistence
{
	public abstract class AbstractNHibernateDao<T> : IDao<T> where T : class, new() //: class
	{
		private readonly Type _persitentType = typeof (T);

		#region IDao implementation
		public List<T> GetAll ()
		{
			return ConnectionFactory.Query<T> ("select * from " + _persitentType.ToString(), null);
		}
		public void Insert (T entity)
		{	
			try{
				//if(ConnectionFactory.Find<T>(entity)==null){
					ConnectionFactory.Insert(entity);
				
			}
			catch(Exception ex){
				string a = "";
			}

		}
		public void Update (T entity)
		{
			ConnectionFactory.Update(entity);
		}
		public void Delete (T entity)
		{
			ConnectionFactory.Delete(entity);
		}
		#endregion


		private static ISQLiteConnection ConnectionFactory
		{
			get { return SQLiteSessionManager.Instance; }
		}
	}
}

