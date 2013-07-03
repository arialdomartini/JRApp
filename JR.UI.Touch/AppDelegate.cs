using JR.UI.Touch.Interfaces;
using JR.UI.Touch.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
using Cirrious.MvvmCross.Touch.Platform;

namespace JR.UI.Touch
{
	[Register("AppDelegate")]
    public partial class AppDelegate
        : MvxApplicationDelegate
        
        
    {
        public static readonly NSString NotificationWillChangeStatusBarOrientation = new NSString("UIApplicationWillChangeStatusBarOrientationNotification");
        public static readonly NSString NotificationDidChangeStatusBarOrientation = new NSString("UIApplicationDidChangeStatusBarOrientationNotification");
        public static readonly NSString NotificationOrientationDidChange = new NSString("UIDeviceOrientationDidChangeNotification");
        public static readonly NSString NotificationFavoriteUpdated = new NSString("NotificationFavoriteUpdated");

        // class-level declarations
        UIWindow _window;

        public static bool IsPhone
        {
            get
            {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
            }
        }

        public static bool IsPad
        {
            get
            {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
            }
        }

        public static bool HasRetina
        {
            get
            {
                if (MonoTouch.UIKit.UIScreen.MainScreen.RespondsToSelector(new Selector("scale")))
                    return (MonoTouch.UIKit.UIScreen.MainScreen.Scale == 2.0);
                else
                    return false;
            }
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // create a new window instance based on the screen size
            _window = new UIWindow(UIScreen.MainScreen.Bounds);

            var presenter = new JRPresenter(this, _window);

			var setup = new Setup(this, presenter);
            setup.Initialize();

            Mvx.RegisterSingleton<ITabBarPresenterHost>(presenter);

            // start the app
            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            _window.MakeKeyAndVisible();

            return true;
        }
    }
}