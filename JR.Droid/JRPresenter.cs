using JR.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using JR.Core.ViewModels;

namespace JR.Droid
{
    public class JRPresenter
		: MvxAndroidViewPresenter,IMvxAndroidViewPresenter
    {  

		public override void Show (MvxViewModelRequest request)
    	{	/*
			var requestTranslator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();
			var intent = requestTranslator.GetIntentFor(request);
			Activity.StartActivity(intent);


			if (request.ViewModelType is AdvertViewModel) {

			} else {
				
			}
			if (TabBarPresenter != null && request != TabBarPresenter)
			{
				TabBarPresenter.Show(request);
				return;
			}
			*/
    		base.Show (request);
    	}

    	public override void ChangePresentation (MvxPresentationHint hint)
    	{
    		base.ChangePresentation (hint);
    	}

    	public override void Close (IMvxViewModel viewModel)
    	{
    		base.Close (viewModel);
    	}

        public IMvxAndroidViewPresenter TabBarPresenter { get; set; }

		public class  AdvertsGroupActivity : TabGroupActivity {

			protected override void OnCreate (Android.OS.Bundle bundle )
			{
				base.OnCreate (bundle);
				//base.StartChildActivity ("advertlist", this.CreateIntentFor(null));
			}	


		}
      
    }
}

