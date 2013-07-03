using System;
using System.Collections.Generic;
using JR.DL.Model.Entities;

namespace JR.SL.Service.Interfaces
{
    public interface IJRService
    {
		void BeginAsyncLoad();
		void DoSyncLoad();
		void DoSyncLoadAdvert(string what, string where, int page);
		void LoadAdvert ();
		
        bool IsLoading { get; }

        IDictionary<string, FavoriteAdvert> Adverts { get; }
		List<Advert> Items { get; set;}
		bool isFinished{ get;}

        IDictionary<string, FavoriteAdvert> GetCopyOfFavoriteAdverts();
    }
}