using MonoTouch.UIKit;

namespace JR.UI.Touch.Views
{
    public class EmptyViewController 
        : UIViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Gray;
        }
    }
}