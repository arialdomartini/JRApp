using JR.Core.Interfaces;
using Cirrious.CrossCore;
using MonoTouch.UIKit;

namespace JR.UI.Touch
{
    public class ErrorDisplayer
    {
        public ErrorDisplayer()
        {
            var source = Mvx.Resolve<IErrorSource>();
            source.ErrorReported += (sender, args) => ShowError(args.Message);
        }

        private void ShowError(string message)
        {
            var errorView = new UIAlertView("Jobrapido X", message, null, "OK", null);
            errorView.Show();
        }
    }
}