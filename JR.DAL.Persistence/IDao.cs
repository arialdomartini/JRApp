using System;
using System.Collections.Generic;


namespace JR.DAL.Persistence
{
	public interface IDao<T>
	{
		List<T> GetAll();
		void Insert(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}

