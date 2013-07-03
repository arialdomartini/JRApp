using System;

namespace JR.SL.Service.Messages
{
	public class FavoritesChangedMessage : Cirrious.MvvmCross.Plugins.Messenger.MvxMessage
	{
		public FavoritesChangedMessage (object sender)
			: base(sender)
		{
		}
	}
}

