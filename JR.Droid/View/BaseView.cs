using Android.Views;
using JR.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;

namespace JR.Droid.Views
{
    public abstract class BaseView<TViewModel> 
        : MvxActivity
        , IBaseView<TViewModel>
        where TViewModel : BaseViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(bundle);

            this.SetBackground();
        }
    }
}