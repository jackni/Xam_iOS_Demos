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
	[Register ("AMSController")]
	partial class AMSController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem MoreMenuBtn { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (MoreMenuBtn != null) {
				MoreMenuBtn.Dispose ();
				MoreMenuBtn = null;
			}
		}
	}
}
