using Android.App;
using Android.Content;
using Android.Widget;
using JR.Core.ViewModels;
using Cirrious.MvvmCross.Droid.Views;

namespace JR.Droid.Views
{
    [Activity(Label = "Jobrapido")]
    public class HomeView : BaseTabbedView<HomeViewModel>
    {
        protected override void OnViewModelSet()
        {

            SetContentView(Resource.Layout.Page_Home);

            TabHost.TabSpec spec;     // Resusable TabSpec for each tab
            Intent intent;            // Reusable Intent for each tab

            // Initialize a TabSpec for each tab and add it to the TabHost
            spec = TabHost.NewTabSpec("Adverts");
            spec.SetIndicator(this.GetText("Adverts"), Resources.GetDrawable(Resource.Drawable.Tab_Adverts));
			spec.SetContent(this.CreateIntentFor(ViewModel.Adverts));
            TabHost.AddTab(spec);
            
            spec = TabHost.NewTabSpec("favorites");
            spec.SetIndicator(this.GetText("Favorites"), Resources.GetDrawable(Resource.Drawable.Tab_Favorites));
            spec.SetContent(this.CreateIntentFor(ViewModel.Favorites));
            TabHost.AddTab(spec);

            
        }
    }
}