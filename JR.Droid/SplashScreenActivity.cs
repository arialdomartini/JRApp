using Android.App;
using Android.Graphics.Drawables;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;

namespace JR.Droid
{
    [Activity(Label = "Jobrapido", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
    public class SplashScreenActivity
        : MvxSplashScreenActivity
    {
        public SplashScreenActivity()
            : base(Resource.Layout.SplashScreen)
        {
        }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
		
            base.OnCreate(bundle);
            this.SetBackground();
        }
    }
}