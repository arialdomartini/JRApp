using Android.App;
using JR.Core.ViewModels.HomeViewModels;

namespace JR.Droid.Views.Home
{
    [Activity]
    public class AdvertsView : BaseView<AdvertsViewModel>
    {
        protected override void OnViewModelSet()
        {
            this.SetContentView(Resource.Layout.ChildPage_Adverts);

        }
    }
}