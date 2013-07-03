using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.File;
using System.Text;
using JR.DAL.Repository;
using System.Linq;

using JR.DL.Model.Entities;

namespace JR.SL.Service
{
    public class FavoritesSaver
    {
		private IDictionary<string, FavoriteAdvert> _toSave;
		private static readonly IRepositoryFactory _daoFactory=new RepositoryFactory() ;
		private static readonly IFavoriteAdvertRepository _repositoryFavorite = _daoFactory.GetFavoriteAdvertRepository();
		private static readonly IAdvertRepository _repositoryAdvert = _daoFactory.GetAdvertRepository();


		public void RequestAsyncSave(IDictionary<string, FavoriteAdvert> toSave)
        {
            lock (this)
            {
                var wasNull = _toSave == null;
                _toSave = toSave;
                if (wasNull)
                    MvxAsyncDispatcher.BeginAsync(DoSave);
            }
        }

        private void DoSave()
        {
            try
            {
				IDictionary<string, FavoriteAdvert> toSave;
                lock (this)
                {
                    toSave = _toSave;
                    _toSave = null;
                }

                if (toSave == null)
                    return; // nothing to do

                var jsonConvert = Mvx.Resolve<IMvxJsonConverter>();
                var json = jsonConvert.SerializeObject(toSave.Keys.ToList());
                var fileService = Mvx.Resolve<IMvxFileStore>();
                fileService.WriteFile(Constants.FavoritesFileName, json);
				foreach(FavoriteAdvert st in toSave.Values.ToList()){
					FavoriteAdvert2 fa = new FavoriteAdvert2();
					fa.Key = st.Advert.Key;
					//if(_repositoryAdvert.FindByKey(st.Advert.Key)== null){
					//
					//_repositoryAdvert.Insert(st.Advert);
					//_repositoryFavorite.Insert(fa);
				}

            }
            catch (Exception exception)
            {
                //Console.Out("Failed to save favorites: {0}", exception.ToLongString());
            }
        }
    }
}