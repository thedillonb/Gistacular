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
            UIToolbar.Appearance.SetBackgroundImage(Images.Toolbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIToolbarPosition.Bottom, UIBarMetrics.Default);
            
            var textAttrs = new UITextAttributes { TextColor = UIColor.White, TextShadowColor = UIColor.FromRGB(40, 40, 40), TextShadowOffset = new UIOffset(0, 1) };
            UINavigationBar.Appearance.SetTitleTextAttributes(textAttrs);

            UISearchBar.Appearance.BackgroundImage = Images.Searchbar.CreateResizableImage(new UIEdgeInsets(0, 1f, 0, 1f));
            UINavigationBar.Appearance.SetBackgroundImage(Images.TopNavbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIBarMetrics.Default);
            UINavigationBar.AppearanceWhenContainedIn(typeof(UIPopoverController)).SetBackgroundImage (null, UIBarMetrics.Default);
        }


        /// <summary>
        /// Processes the accounts.
        /// </summary>
        public void ProcessAccounts()
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
        private static Account GetDefaultAccount()
        {
            var defaultAccount = Application.Accounts.GetDefault();
            if (defaultAccount == null)
            {
                defaultAccount = Application.Accounts.FirstOrDefault();
                Application.Accounts.SetDefault(defaultAccount);
            }
            return defaultAccount;
        }

        public void ShowMainWindow()
        {
            var defaultAccount = GetDefaultAccount();
            Application.SetUser(defaultAccount);

            _nav = new SlideoutNavigationController();
            _window.RootViewController = _nav;
            _nav.SelectView(new MyGistsController());
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
