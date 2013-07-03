using System;

namespace JR.SL.Service.Messages
{	
	public class LoadingChangedMessage : Cirrious.MvvmCross.Plugins.Messenger.MvxMessage
	{
		public LoadingChangedMessage (object sender)
			: base(sender)
		{
		}
	}
}
