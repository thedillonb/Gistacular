using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Gistacular.Controllers;
using Gistacular.Data;

namespace Gistacular
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow _window;
		SlideoutNavigationController _nav;

		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
            //Set the theme
            SetTheme();

			_window = new UIWindow (UIScreen.MainScreen.Bounds);

            //Process the accounts
            ProcessAccounts();
            
            //Make what ever window visible.
            _window.MakeKeyAndVisible();

			_window.MakeKeyAndVisible ();
			return true;
		}

        /// <summary>
        /// Sets the theme of the application.
        /// </summary>
        private void SetTheme()
        {
            //Set the status bar
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.BlackOpaque, false);
//            
//            //Set the theming
//            UINavigationBar.Appearance.SetBackgroundImage(Images.Titlebar.CreateResizableImage(new UIEdgeInsets(0, 0, 1, 0)), UIBarMetrics.Default);
//            
//            UIBarButtonItem.Appearance.SetBackgroundImage(Images.BarButton.CreateResizableImage(new UIEdgeInsets(8, 7, 8, 7)), UIControlState.Normal, UIBarMetrics.Default);
//            UISegmentedControl.Appearance.SetBackgroundImage(Images.BarButton.CreateResizableImage(new UIEdgeInsets(8, 7, 8, 7)), UIControlState.Normal, UIBarMetrics.Default);
//            
//            UIBarButtonItem.Appearance.SetBackgroundImage(Images.BarButtonLandscape.CreateResizableImage(new UIEdgeInsets(8, 7, 8, 7)), UIControlState.Normal, UIBarMetrics.LandscapePhone);
//            UISegmentedControl.Appearance.SetBackgroundImage(Images.BarButtonLandscape.CreateResizableImage(new UIEdgeInsets(8, 7, 8, 7)), UIControlState.Normal, UIBarMetrics.LandscapePhone);
//            
//            //BackButton
//            UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(Images.BackButton.CreateResizableImage(new UIEdgeInsets(0, 14, 0, 5)), UIControlState.Normal, UIBarMetrics.Default);
//            
//            UISegmentedControl.Appearance.SetDividerImage(Images.Divider, UIControlState.Normal, UIControlState.Normal, UIBarMetrics.Default);
//            
//            UIToolbar.Appearance.SetBackgroundImage(Images.Bottombar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIToolbarPosition.Bottom, UIBarMetrics.Default);
//            //UIBarButtonItem.Appearance.TintColor = UIColor.White;
//            
            var textAttrs = new UITextAttributes { TextColor = UIColor.White, TextShadowColor = UIColor.FromRGB(40, 40, 40), TextShadowOffset = new UIOffset(0, 1) };
            UINavigationBar.Appearance.SetTitleTextAttributes(textAttrs);

//            
//            SearchFilterBar.ButtonBackground = Images.BarButton.CreateResizableImage(new UIEdgeInsets(0, 6, 0, 6));
//            SearchFilterBar.FilterImage = Images.Filter;
//            
//            DropbarView.Image = UIImage.FromFile("Images/Controls/Dropbar.png");
//            WatermarkView.Image = Images.Background;
//            HeaderView.Gradient = Images.CellGradient;
//            StyledElement.BgColor = UIColor.FromPatternImage(Images.TableCell);
//            ErrorView.AlertImage = UIImage.FromFile("Images/warning.png");
//            UserElement.Default = Images.Anonymous;
//            NewsFeedElement.DefaultImage = Images.Anonymous;
//            NewsFeedElement.LinkColor = UIColor.FromRGB(0, 64, 128);
//            NewsFeedElement.LinkFont = UIFont.BoldSystemFontOfSize(12f);
//            TableViewSectionView.BackgroundImage = Images.Searchbar;
//
//            
//            //Resize the back button only on the iPhone
//            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
//            {
//                UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(Images.BackButtonLandscape.CreateResizableImage(new UIEdgeInsets(0, 14, 0, 6)), UIControlState.Normal, UIBarMetrics.LandscapePhone);
//            }


            //Set the theming
//            UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(Images.Controls.BackButton.CreateResizableImage(new UIEdgeInsets(0, 16, 0, 10)), UIControlState.Normal, UIBarMetrics.Default);
//            UIBarButtonItem.AppearanceWhenContainedIn(typeof(UIPopoverController)).SetBackButtonBackgroundImage(null, UIControlState.Normal, UIBarMetrics.Default);
//
//            UIBarButtonItem.Appearance.SetBackgroundImage(Images.Controls.Button, UIControlState.Normal, UIBarMetrics.Default);
//            UIBarButtonItem.AppearanceWhenContainedIn(typeof(UIPopoverController)).SetBackgroundImage(null, UIControlState.Normal, UIBarMetrics.Default);
//

            UISearchBar.Appearance.BackgroundImage = Images.Searchbar.CreateResizableImage(new UIEdgeInsets(0, 1f, 0, 1f));

            UINavigationBar.Appearance.SetBackgroundImage(Images.TopNavbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIBarMetrics.Default);
            UINavigationBar.AppearanceWhenContainedIn(typeof(UIPopoverController)).SetBackgroundImage (null, UIBarMetrics.Default);
        }


        /// <summary>
        /// Processes the accounts.
        /// </summary>
        private void ProcessAccounts()
        {
            //There's no accounts...
            if (GetDefaultAccount() == null)
            {
                var login = new GitHubLoginController { LoginComplete = delegate { ShowMainWindow(); }};

                //Make it so!
                _window.RootViewController = new UINavigationController(login);
            }
            else
            {
                ShowMainWindow();
            }
        }

        /// <summary>
        /// Gets the default account. If there is not one assigned it will pick the first in the account list.
        /// If there isn't one, it'll just return null.
        /// </summary>
        /// <returns>The default account.</returns>
        private Account GetDefaultAccount()
        {
            var defaultAccount = Application.Accounts.GetDefault();
            if (defaultAccount == null)
            {
                defaultAccount = Application.Accounts.FirstOrDefault();
                Application.Accounts.SetDefault(defaultAccount);
            }
            return defaultAccount;
        }

        private void ShowMainWindow()
        {
            var defaultAccount = GetDefaultAccount();
            Application.SetUser(defaultAccount);

            _nav = new SlideoutNavigationController();
            //_nav.SetMenuNavigationBackgroundImage(Images.TitlebarDark, UIBarMetrics.Default);
            _window.RootViewController = _nav;
        }

        public override void ReceiveMemoryWarning(UIApplication application)
        {
            //Remove everything from the cache
            Application.Cache.DeleteAll();

            //Pop back to the root view...
            if (_nav.TopView != null && _nav.TopView.NavigationController != null)
                _nav.TopView.NavigationController.PopToRootViewController(false);
        }
	}
}
