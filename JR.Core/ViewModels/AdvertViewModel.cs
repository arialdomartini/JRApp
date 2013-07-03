using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Share;
using Cirrious.MvvmCross.ViewModels;

namespace JR.Core.ViewModels
{
    public class AdvertViewModel: BaseViewModel
    {

			private string _url;

			public string Url {
				get {
					return _url;
				}
				set {
					_url = value;
				}
			}

			
			public void Init(string url)
			{
				_url = url;
			}

			public override void Start()
			{
				//ShowAdvert();
				base.Start();
			}
		/*

			protected override void RepositoryOnLoadingChanged()
			{
				ShowAdvert();
				base.RepositoryOnLoadingChanged();
			}

		/*
			private void ShowAdvert()
			{

				//ShowWebPage(_key);
				
				FavoriteAdvert Advert;
				if (Service.Adverts == null 
				    || !Service.Adverts.TryGetValue(_key, out Advert))
				{
					// TODO - some kind of error notification would be nice
					return;
				}

				Advert = Advert;
				
				
			}
			*/

			//public FavoriteAdvert Advert { get; private set; }

			/*
			public ICommand ShareCommand
			{
				get { return new MvxCommand(DoShare); }
			}

			public void DoShare()
			{
				if (Advert == null)
					return;

				var service = Mvx.Resolve<IMvxShareTask>();
				var toShare = string.Format("#Jobrapido: {0} - {1}", Advert.Advert.SpeakerKey, Advert.Advert.Title);
				if (toShare.Length > 140)
					toShare = toShare.Substring(0, 135).Trim() + "...";
				service.ShareShort(toShare);
			}
			*/

	}
}