using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.Touch;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using JR.Core.ViewModels.HomeViewModels;
using Cirrious.MvvmCross.Views;
using System.Collections.Generic;
using Cirrious.MvvmCross.Touch.Views;

using Cirrious.MvvmCross.Binding.BindingContext;

namespace JR.UI.Touch
{
    public partial class AdvertsView
        : MvxViewController
    {
        public AdvertsView()
            : base("AdvertsView", null)
        {
        }

		public new AdvertsViewModel ViewModel {
			get { return (AdvertsViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			TextFieldDelegate tfd = new TextFieldDelegate (this.View, 100);

			this.View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("JRResources/background.png"));
            //NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Tweet", UIBarButtonItemStyle.Bordered, (sender, e) => ViewModel.DoShareGeneral()), false);

            // Perform any additional setup after loading the view, typically from a nib.
            //Button1.SetImage(UIImage.FromFile("JRResources/Images/appbar.calendar.png"), UIControlState.Normal);
            //Button2.SetImage(UIImage.FromFile("JRResources/Images/appbar.calendar.png"), UIControlState.Normal);
            //Button3.SetImage(UIImage.FromFile("JRResources/Images/appbar.calendar.png"), UIControlState.Normal);
            //Button4.SetImage(UIImage.FromFile("JRResources/Images/appbar.people.png"), UIControlState.Normal);
            //Button5.SetImage(UIImage.FromFile("JRResources/Images/appbar.database.png"), UIControlState.Normal);

            this.AddBindings(new Dictionary<object, string>()
			    {
					{ lblWhat, "Text TextSource,Converter=Language, ConverterParameter='What'" },				
					{ lblWhere, "Text TextSource,Converter=Language, ConverterParameter='Where'" },				
					{ btnSearch, "Title TextSource,Converter=Language, ConverterParameter='Search'" }		
				});

            this.AddBindings(new Dictionary<object, string>()
			    {
					
				{ btnSearch, "TouchUpInside GoParameterizedCommand" },
				});

	
			this.txtWhat.ShouldReturn += (textField) => { 
				textField.ResignFirstResponder();
				return true; 
			};
			NavigationController.NavigationBarHidden = true;
			txtWhere.Delegate = tfd;

			this.CreateBinding(txtWhat).To((AdvertsViewModel vm) => vm.SearchTerm).Apply();
			this.CreateBinding(txtWhere).To((AdvertsViewModel vm) => vm.SearchWhereTerm).Apply();
        }


        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            // Clear any references to subviews of the main view in order to
            // allow the Garbage Collector to collect them sooner.
            //
            // e.g. myOutlet.Dispose (); myOutlet = null;

            ReleaseDesignerOutlets();
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
        }
    }

	public class TextFieldDelegate : UITextFieldDelegate {
		private UIView reference;
		private int distance;

		public TextFieldDelegate(UIView view, int dist) { 
			reference = view;
			distance = dist;
		}

		public override void EditingStarted(UITextField field) {
			animateTextField(true);
		}

		public override void EditingEnded(UITextField field) { 
			animateTextField(false);
		}
		public override bool ShouldReturn (UITextField textField)
		{
			textField.ResignFirstResponder();
			return true; 
		}

		private void animateTextField(bool up) {
			const float duration = 0.4f;

			int movement = up ? -distance : distance;

			UIView.BeginAnimations("Anim");
			UIView.SetAnimationBeginsFromCurrentState(true);
			UIView.SetAnimationDuration(duration);

			RectangleF frameM = reference.Frame;
			frameM.Y += movement;
			reference.Frame = frameM;

			UIView.CommitAnimations();
		}
	}
}

