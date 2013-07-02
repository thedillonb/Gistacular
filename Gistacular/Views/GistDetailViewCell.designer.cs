// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Gistacular.Views
{
	[Register ("GistDetailViewCell")]
	partial class GistDetailViewCell
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView CommentImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel Comments { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel Date { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel Description { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView ForkImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel Forks { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView MainImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel Name { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel Stars { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView StarsImage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (MainImage != null) {
				MainImage.Dispose ();
				MainImage = null;
			}

			if (Name != null) {
				Name.Dispose ();
				Name = null;
			}

			if (Date != null) {
				Date.Dispose ();
				Date = null;
			}

			if (Description != null) {
				Description.Dispose ();
				Description = null;
			}

			if (ForkImage != null) {
				ForkImage.Dispose ();
				ForkImage = null;
			}

			if (CommentImage != null) {
				CommentImage.Dispose ();
				CommentImage = null;
			}

			if (StarsImage != null) {
				StarsImage.Dispose ();
				StarsImage = null;
			}

			if (Forks != null) {
				Forks.Dispose ();
				Forks = null;
			}

			if (Comments != null) {
				Comments.Dispose ();
				Comments = null;
			}

			if (Stars != null) {
				Stars.Dispose ();
				Stars = null;
			}
		}
	}
}
