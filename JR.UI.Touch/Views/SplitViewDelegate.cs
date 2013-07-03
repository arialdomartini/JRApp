using MonoTouch.UIKit;

namespace JR.UI.Touch.Views
{
    public class SplitViewDelegate : UISplitViewControllerDelegate
    {
        public override bool ShouldHideViewController (UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
        {
            return false;
        }
    }
}