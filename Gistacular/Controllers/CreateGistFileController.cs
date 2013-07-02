using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;

namespace Gistacular.Controllers
{
   
    /// <summary>
    ///   Composer is a singleton that is shared through the lifetime of the app,
    ///   the public methods in this class reset the values of the composer on 
    ///   each invocation.
    /// </summary>
    public class CreateGistFileController : UIViewController
    {
        readonly ComposerView _composerView;
        UIViewController _previousController;
        public Action ReturnAction;

        private class ComposerView : UIWebView 
        {
            const UIBarButtonItemStyle Style = UIBarButtonItemStyle.Bordered;

            public ComposerView (RectangleF bounds, CreateGistFileController composer) : base (bounds)
            {
            }

            internal void Reset (string text)
            {
            }

            public override void LayoutSubviews ()
            {
                Resize (Bounds);
            }

            void Resize (RectangleF bounds)
            {
            }

            public override UIView InputAccessoryView
            {
                get
                {
                    return null;
                }
            }

            public string Text { 
                get {
                    return String.Empty;
                }
                set {

                }
            }
        }

        public CreateGistFileController () : base (null, null)
        {
            Title = "New Comment";

            // Composer
            _composerView = new ComposerView (ComputeComposerSize (RectangleF.Empty), this);
            _composerView.LoadHtmlString(System.IO.File.ReadAllText("CodeEditor/index.html", System.Text.Encoding.UTF8), null);

            // Add the views
            NSNotificationCenter.DefaultCenter.AddObserver (new NSString("UIKeyboardWillShowNotification"), KeyboardWillShow);

            View.AddSubview (_composerView);
        }

        public string Text
        {
            get { return _composerView.Text; }
            set { _composerView.Text = value; }
        }

        public void CloseComposer ()
        {
            _previousController.DismissModalViewControllerAnimated (true);
        }

        void PostCallback (object sender, EventArgs a)
        {
            if (ReturnAction != null)
                ReturnAction();
        }

        void KeyboardWillShow (NSNotification notification)
        {
            var nsValue = notification.UserInfo.ObjectForKey (UIKeyboard.BoundsUserInfoKey) as NSValue;
            if (nsValue == null) return;
            var kbdBounds = nsValue.RectangleFValue;
//            _composerView.Frame = ComputeComposerSize (kbdBounds);
        }

        RectangleF ComputeComposerSize (RectangleF kbdBounds)
        {
            var view = View.Bounds;
            return new RectangleF (0, 0, view.Width, view.Height-kbdBounds.Height);
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

        public override void ViewWillAppear (bool animated)
        {
            base.ViewWillAppear (animated);
            _composerView.BecomeFirstResponder();

            this.View.BackgroundColor = UIColor.White;
            _composerView.BackgroundColor = UIColor.White;


            foreach (var view in _composerView.Subviews)
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

        public void NewComment (UIViewController parent, Action action)
        {
            ReturnAction = action;
            _previousController = parent;
            parent.PresentModalViewController (this, true);
        }
    }
}

