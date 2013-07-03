using System;
using System.Linq;
using Cirrious.MvvmCross.Plugins.Messenger;
using JR.SL.Service.Messages;

namespace JR.Core.ViewModels.AdvertLists
{
	public class FavoritesViewModel
		: BaseReloadingAdvertListViewModel<DateTime>
	{
		private readonly MvxSubscriptionToken _mvxSubscription;

		public FavoritesViewModel()
		{
			RebuildFavorites();

			_mvxSubscription = Subscribe<FavoritesChangedMessage>(ServiceOnFavoritesAdvertsChanged);
		}

		private void ServiceOnFavoritesAdvertsChanged(FavoritesChangedMessage message)
		{
			InvokeOnMainThread(RebuildFavorites);
		}

		private void RebuildFavorites()
		{
			var grouped = Service.GetCopyOfFavoriteAdverts()
				.Values
					.GroupBy(slot => slot.Advert.When)
					.OrderBy(slot => slot.Key)
					.Select(slot => new AdvertGroup(
						slot.Key,
						slot.OrderBy(Advert => Advert.Advert.Title),
						NavigateToAdvert));

			GroupedList = grouped.ToList();
		}
		public override void GoNextPage ()
		{
		}
		protected override void LoadAdverts ()
		{
		}
	}
}