using System;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace JR.Core.ViewModels.AdvertLists
{
	public class AdvertListViewModel
		: BaseReloadingAdvertListViewModel<DateTime>
	{
		private int _page;
		public void Init()
		{
			_page = 1;
		}

		public override void GoNextPage(){
			_page++;
			LoadAdverts ();
		}

		protected override void LoadAdverts()
		{

			Service.DoSyncLoadAdvert (Parameters.What, Parameters.Where,_page);


			if (Service.Adverts == null)
				return;

			/*
            var grouped = Service.Adverts
                .Values
                .Where(slot => slot.Advert.When.Day == _dayOfMonth)
                .GroupBy(slot => slot.Advert.When)
                .OrderBy(slot => slot.Key)
                .Select(slot => new AdvertGroup(
                                    slot.Key,
                                    slot.OrderBy(Advert => Advert.Advert.Title),
                                    NavigateToAdvert));
			*/
			var grouped = Service.Adverts
				.Values
					.GroupBy(Advert => Advert.Advert.When)
					.OrderByDescending(slot => slot.Key)
					.Select(slot => new AdvertGroup(
						slot.Key,
						slot.OrderByDescending(Advert => Advert.Advert.When),
						NavigateToAdvert));

			GroupedList = grouped.ToList();
		}





		public class Parameters {

			public Parameters(string what, string where){

				What =what;
				Where = where;
			}

			public static string What;
			public static string Where;

		}

		private string _title;
		public string Title
		{
			get { return _title; }
			set { _title = value; RaisePropertyChanged(() => Title); }
		}
	}
}