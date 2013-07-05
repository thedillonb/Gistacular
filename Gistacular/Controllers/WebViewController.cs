using System;
using MonoTouch.UIKit;
using Gistacular.Views;

namespace Gistacular.Controllers
{
    public class WebViewController : UIViewController
    {
        public UIWebView Web { get; private set; }

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
        {
            Web = new UIWebView {ScalesPageToFit = true};
            Web.LoadFinished += OnLoadFinished;
            Web.LoadStarted += OnLoadStarted;
            Web.LoadError += OnLoadError;

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
            base.ViewWillAppear(animated);
            var bounds = View.Bounds;
            Web.Frame = bounds;

            this.View.BackgroundColor = UIColor.White;
            this.Web.BackgroundColor = UIColor.White;

            foreach (var view in Web.Subviews)
            {
                if (view is UIScrollView)
                {
                    foreach (var shadowView in view.Subviews)
                    {
                        if (shadowView is UIImageView)
                            shadowView.Hidden = true;
                    }
                }
            }
        }
        
        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            Web.Frame = View.Bounds;
        }
    }
}

