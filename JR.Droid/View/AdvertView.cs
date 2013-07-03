using Android.App;
using Android.Content;
using Android.Widget;
using JR.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Android.Webkit;


namespace JR.Droid.Views
{
    [Activity(Label = "Jobrapido")]
	public class AdvertView: BaseView<AdvertViewModel>   
    {
		WebView web_view;

		public new AdvertViewModel ViewModel {
			get { return (AdvertViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected override void OnCreate(Android.OS.Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Page_Advert);

			web_view = FindViewById<WebView> (Resource.Id.webview);
			web_view.Settings.JavaScriptEnabled = (true);
			web_view.Settings.PluginsEnabled = (true);	
			web_view.SetWebViewClient (new ViewClient (this));


			web_view.SetInitialScale(1);
			web_view.Settings.LoadWithOverviewMode = (true);
			web_view.Settings.UseWideViewPort = true;
			web_view.ScrollBarStyle = Android.Views.ScrollbarStyles.OutsideOverlay;
			web_view.ScrollbarFadingEnabled = (false);
			web_view.Settings.SetRenderPriority(WebSettings.RenderPriority.High);
			web_view.Settings.CacheMode = CacheModes.NoCache;


			web_view.Settings.SetSupportZoom(true);
			web_view.Settings.BuiltInZoomControls = (true);
			web_view.ScrollbarFadingEnabled = (true);
			web_view.Settings.LoadsImagesAutomatically = (true);



			web_view.LoadUrl (ViewModel.Url);


		}

		class ViewClient : WebViewClient
		{
			AdvertView _activity;

			public ViewClient(AdvertView activity)
			{
				_activity = activity;   
			}

			public override void OnPageStarted (WebView view, string url, Android.Graphics.Bitmap favicon)
			{
				//Console.WriteLine ("On Page Started");
			}

			public override void OnReceivedHttpAuthRequest (WebView view, HttpAuthHandler handler, string host, string realm)
			{
				//Console.WriteLine ("On received Http auth request");
				handler.Proceed("username", "password");
			}

			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				//Console.WriteLine ("Should Override Url Loading...");
				return base.ShouldOverrideUrlLoading (view, url);
			}
		}
    }
}