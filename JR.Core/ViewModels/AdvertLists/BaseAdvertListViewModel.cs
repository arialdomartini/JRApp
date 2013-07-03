using System;
using System.Collections.Generic;
using System.Linq;
using JR.DL.Model.Entities;
using JR.Core.ViewModels.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace JR.Core.ViewModels.AdvertLists
{
    public class BaseAdvertListViewModel<TKey>
        : BaseJRViewModel
    {
        public class AdvertGroup : List<WithCommand<FavoriteAdvert>>
        {
            public TKey Key { get; set; }

            public AdvertGroup(TKey key, IEnumerable<FavoriteAdvert> items, Action<Advert> tapAction)
                : base((IEnumerable<WithCommand<FavoriteAdvert>>) items.Select(x => new WithCommand<FavoriteAdvert>(x, new MvxCommand(() => tapAction(x.Advert)))))
            {
                Key = key;
            }
        }



        private List<AdvertGroup> _groupedList;
        public List<AdvertGroup> GroupedList
        {
            get { return _groupedList; }
            protected set { _groupedList = value; RaisePropertyChanged("GroupedList"); }
        }


        protected void NavigateToAdvert(Advert Advert)
        {
            ShowViewModel<AdvertViewModel>(new { url = Advert.Description });
        }
    }
}