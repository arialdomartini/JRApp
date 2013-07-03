using JR.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Touch.Views;

namespace JR.UI.Touch.Views
{
    public class SplashScreenView
        : MvxTableViewController
    {
		public new SplashScreenViewModel ViewModel {
			get { return (SplashScreenViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.SplashScreenComplete = true;
        }
    }
}