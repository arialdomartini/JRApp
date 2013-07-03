using Android.Content;
using Android.Widget;
using JR.Core;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Localization;
using JR.Droid.Bindings;


namespace JR.Droid
{
    public class Setup
        : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {

        }

        protected override IMvxApplication CreateApp()
        {
			return new NoSplashScreenJRApp();
        }

		protected override Cirrious.MvvmCross.Droid.Views.IMvxAndroidViewPresenter CreateViewPresenter ()
		{
			return base.CreateViewPresenter ();
		}
        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterFactory(new MvxCustomBindingFactory<Button>("IsFavorite", (button) => new FavoritesButtonBinding(button)));
        }

		protected override System.Collections.Generic.List<System.Reflection.Assembly> ValueConverterAssemblies 
		{
			get 
			{
				var toReturn = base.ValueConverterAssemblies;
				toReturn.Add(typeof(MvxLanguageConverter).Assembly);
				return toReturn;
			}
		}
    }
}

