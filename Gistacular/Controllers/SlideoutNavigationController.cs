using System;
using MonoTouch.UIKit;

namespace Gistacular.Controllers
{
	public class SlideoutNavigationController : MonoTouch.SlideoutNavigation.SlideoutNavigationController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CodeBucket.Controllers.SlideoutNavigationController"/> class.
		/// </summary>
		public SlideoutNavigationController()
		{
			//Setting the height to a large amount means that it will activate the slide pretty much whereever your finger is on the screen.
			SlideHeight = 9999f;
		}

		/// <summary>
		/// Creates the menu button.
		/// </summary>
		/// <returns>The menu button.</returns>
		protected override UIBarButtonItem CreateMenuButton()
		{
            var button = new UIButton(UIButtonType.Custom);
            button.Frame = new System.Drawing.RectangleF(0, 0, 40f, 40f);
            button.SetImage(Images.ThreeLines, UIControlState.Normal);
            button.TouchUpInside += (s, e) => Show();
            return new UIBarButtonItem(button);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            SetMenuNavigationBackgroundImage(Images.MenuNavbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIBarMetrics.Default);
			MenuView = new MenuController();
		}

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            //Select the default view
            SelectView(new MyGistsController());
        }
	}
}

