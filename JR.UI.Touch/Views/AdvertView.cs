using System;
using System.Collections.Generic;
using System.Drawing;
using Cirrious.MvvmCross.Binding.Touch;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using JR.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace JR.UI.Touch
{
    public class AdvertView : MvxViewController
    {

		public new AdvertViewModel ViewModel {
			get { return (AdvertViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			/*
            TextView1.Editable = false;
            TextView1.Text = "JE";
            ImageView1.Image = UIImage.FromFile("JRResources/Images/appbar.people.png");
            ImageView2.Image = UIImage.FromFile("JRResources/Images/appbar.city.png");

            this.AddBindings(new Dictionary<object, string>()
                                 {
                                     {Label1, "Text Advert.Advert.Title"},
                                     {TextView1, "Text Advert.Advert.Description"},
                                     {SubLabel1, "Text Advert.Advert.SpeakerKey"},
                                     {SubLabel2, "Text Advert.Advert,Converter=AdvertSmallDetails,ConverterParameter='SmallDetailsFormat'"},
                                     {favoriteButton,"IsFavorite Advert.IsFavorite"}
                                 });
			*/
            //NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Tweet", UIBarButtonItemStyle.Bordered, (sender, e) => ViewModel.DoShare()), false);
			UIWebView webView = new UIWebView (this.View.Bounds);
			webView.ScalesPageToFit = true;
			this.View.Add (webView);

			NSUrlRequest url = new NSUrlRequest(new NSUrl(ViewModel.Url));

			webView.LoadRequest(url);
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            // Clear any references to subviews of the main view in order to
            // allow the Garbage Collector to collect them sooner.
            //
            // e.g. myOutlet.Dispose (); myOutlet = null;

            
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
        }
    }
}

