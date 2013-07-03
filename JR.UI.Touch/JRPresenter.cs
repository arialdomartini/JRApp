using JR.UI.Touch.Interfaces;
using JR.UI.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using MonoTouch.UIKit;

namespace JR.UI.Touch
{
    public class JRPresenter
        : MvxModalSupportTouchViewPresenter
        , ITabBarPresenterHost
    {
        public JRPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override UINavigationController CreateNavigationController(UIViewController viewController)
        {
            var toReturn = base.CreateNavigationController(viewController);
            toReturn.NavigationBarHidden = true;
            return toReturn;
        }

        public ITabBarPresenter TabBarPresenter { get; set; }

        public override void Show(IMvxTouchView view)
        {
            if (TabBarPresenter != null && view != TabBarPresenter)
            {
                TabBarPresenter.ShowView(view);
                return;
            }

            base.Show(view);
        }
    }
}

