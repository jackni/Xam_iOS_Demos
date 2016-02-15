// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MyApp.iOS
{
	[Register ("GooglePlaceController")]
	partial class GooglePlaceController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISearchBar AddressSearchBar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISearchDisplayController searchDisplayController { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (AddressSearchBar != null) {
				AddressSearchBar.Dispose ();
				AddressSearchBar = null;
			}
			if (searchDisplayController != null) {
				searchDisplayController.Dispose ();
				searchDisplayController = null;
			}
		}
	}
}
