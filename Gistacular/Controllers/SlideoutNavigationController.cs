using System;
using MonoTouch.UIKit;
using Gistacular.Views;

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
            BackgroundColor = UIColor.FromRGB(227, 227, 227);
		}

		/// <summary>
		/// Creates the menu button.
		/// </summary>
		/// <returns>The menu button.</returns>
		protected override UIBarButtonItem CreateMenuButton()
		{
            return new UIBarButtonItem(NavigationButton.Create(Images.Buttons.ThreeLines, () => Show()));
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            SetMenuNavigationBackgroundImage(Images.Components.MenuNavbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIBarMetrics.Default);
			MenuView = new MenuController();
		}
	}
}

