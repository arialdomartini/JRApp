using System;
using JR.Core.Interfaces;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.ViewModels;

namespace JR.Core.ApplicationObjects
{
    public class ErrorApplicationObject
        : MvxMainThreadDispatchingObject
          , IErrorReporter
          , IErrorSource
    {
        public void ReportError(string error)
        {
            if (ErrorReported == null)
                return;

            InvokeOnMainThread(() =>
                                   {
                                       var handler = ErrorReported;
                                       if (handler != null)
                                           handler(this, new ErrorEventArgs(error));
                                   });
        }

        public event EventHandler<ErrorEventArgs> ErrorReported;
    }
}