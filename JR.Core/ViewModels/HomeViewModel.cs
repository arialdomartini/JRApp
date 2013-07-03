using JR.Core.ViewModels.HomeViewModels;
using JR.Core.ViewModels.AdvertLists;

namespace JR.Core.ViewModels
{
    public class HomeViewModel
        : BaseJRViewModel
    {
        public HomeViewModel()
        {
			Adverts = new AdvertsViewModel();            
			Favorites = new FavoritesViewModel();
        }

        public FavoritesViewModel Favorites { get; private set; }
        public AdvertsViewModel Adverts { get; private set; }
    }
}