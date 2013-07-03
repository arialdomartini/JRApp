using System;
using JR.Core.Interfaces;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins.Messenger;
using JR.SL.Service.Interfaces;
using JR.SL.Service.Messages;

namespace JR.Core.ViewModels
{
    public class BaseJRViewModel
        : BaseViewModel
        
    {
		private MvxSubscriptionToken _mvxSubscription;

        public BaseJRViewModel()
        {
			_mvxSubscription = Subscribe<LoadingChangedMessage>(message => RepositoryOnLoadingChanged());
        }

        public IJRService Service
        {
            get { return Mvx.Resolve<IJRService>(); }
        }

        public bool IsLoading
        {
            get { return Service.IsLoading; }
        }

        protected virtual void RepositoryOnLoadingChanged()
        {
            RaisePropertyChanged(() => IsLoading);
        }

    }
}