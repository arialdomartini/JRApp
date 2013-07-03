using System;
using System.Collections.Generic;
using JR.DL.Model.Entities;
using JR.DAL.Persistence;

namespace JR.DAL.Repository
{
	public interface IRepositoryFactory	
	{
		IFavoriteAdvertRepository GetFavoriteAdvertRepository();
		IAdvertRepository GetAdvertRepository();

	}
	
	public interface IFavoriteAdvertRepository : IDao<FavoriteAdvert2>
	{


	}
	public interface IAdvertRepository : IDao<Advert>
	{
		Advert FindByKey(string key);

	}
}

