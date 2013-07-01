using System;
using MonoTouch.UIKit;
using Gistacular.Views;

namespace Gistacular.Controllers
{
    public class WebViewController : UIViewController
    {
        public UIWebView Web { get; private set; }
        private readonly bool _navigationToolbar;

        protected virtual void GoBack()
        {
            Web.GoBack();
        }

        protected virtual void Refresh()
        {
            Web.Reload();
        }

        protected virtual void GoForward()
        {
            Web.GoForward();
        }
         
        public WebViewController()
            : this(true)
        {
        }

        public WebViewController(bool navigationToolbar)
        {
            Web = new UIWebView {ScalesPageToFit = true};
            Web.LoadFinished += OnLoadFinished;
            Web.LoadStarted += OnLoadStarted;
            Web.LoadError += OnLoadError;

            _navigationToolbar = navigationToolbar;

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(NavigationButton.Create(Images.BackButton, () => NavigationController.PopViewControllerAnimated(true)));
        }

        protected virtual void OnLoadError (object sender, UIWebErrorArgs e)
        {
            MonoTouch.Utilities.PopNetworkActive();
        }

        protected virtual void OnLoadStarted (object sender, EventArgs e)
        {
            MonoTouch.Utilities.PushNetworkActive();
        }

        protected virtual void OnLoadFinished(object sender, EventArgs e)
        {
            MonoTouch.Utilities.PopNetworkActive();
        }
        
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (_navigationToolbar)
                NavigationController.SetToolbarHidden(true, animated);
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Add(Web);
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            Web.Frame = View.Bounds;
        }
        
        public override void ViewWillAppear(bool animated)
        {
            if (_navigationToolbar)
                NavigationController.SetToolbarHidden(false, animated);
            base.ViewWillAppear(animated);
            var bounds = View.Bounds;
            if (_navigationToolbar)
                bounds.Height -= NavigationController.Toolbar.Frame.Height;
            Web.Frame = bounds;
        }
        
        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            Web.Frame = View.Bounds;
        }
    }
}

