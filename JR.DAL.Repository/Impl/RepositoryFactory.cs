using System;
using System.Collections.Generic;
using JR.DL.Model.Entities;
using JR.DAL.Persistence;
using JR.DAL.Persistence.Sqlite;
using System.Linq;


namespace JR.DAL.Repository
{
	public class RepositoryFactory : IRepositoryFactory
	{
		//public static SQLiteSessionManager sessionManager =  new SQLiteSessionManager();

		public IFavoriteAdvertRepository GetFavoriteAdvertRepository()
		{
			SQLiteSessionManager.Instance.CreateTable<FavoriteAdvert2>();
			return new FavoriteAdvertRepository();
		}
		
		public IAdvertRepository GetAdvertRepository()
		{
			SQLiteSessionManager.Instance.CreateTable<Advert>();
			return new AdvertRepository();
		}

		public class FavoriteAdvertRepository : AbstractNHibernateDao<FavoriteAdvert2>,IFavoriteAdvertRepository
		{

		}
		public class AdvertRepository :  AbstractNHibernateDao<Advert> , IAdvertRepository
		{
			#region IAdvertRepository Members



			public Advert FindByKey(string key)
			{

				try{
					Advert advert = SQLiteSessionManager.Instance.Get<Advert> (key);
					return advert;

				}
				catch(Exception ex){
					return null;
				}
/*
				return SQLiteSessionManager.Instance.Table<Advert>()
						.Where(x => x.Key == key)
						.FirstOrDefault();
						*/
			}


			#endregion


		}
	}
}

