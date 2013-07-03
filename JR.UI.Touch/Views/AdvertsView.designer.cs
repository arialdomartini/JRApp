// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace JR.UI.Touch
{
	[Register ("AdvertsView")]
	partial class AdvertsView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel lblWhat { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtWhat { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblWhere { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtWhere { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnSearch { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblWhat != null) {
				lblWhat.Dispose ();
				lblWhat = null;
			}

			if (txtWhat != null) {
				txtWhat.Dispose ();
				txtWhat = null;
			}

			if (lblWhere != null) {
				lblWhere.Dispose ();
				lblWhere = null;
			}

			if (txtWhere != null) {
				txtWhere.Dispose ();
				txtWhere = null;
			}

			if (btnSearch != null) {
				btnSearch.Dispose ();
				btnSearch = null;
			}
		}
	}
}
