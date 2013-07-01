using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Drawing;

namespace Gistacular.Controllers
{
    public class BaseDialogViewController : DialogViewController
    {
        protected bool IsSearching;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (NavigationController != null && IsSearching)
                NavigationController.SetNavigationBarHidden(true, true);
            if (IsSearching)
            {
                TableView.ScrollRectToVisible(new RectangleF(0, 0, 1, 1), false);
            }
        }
        
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (NavigationController != null && NavigationController.NavigationBarHidden)
                NavigationController.SetNavigationBarHidden(false, true);
            
            if (IsSearching)
            {
                View.EndEditing(true);
                var searchBar = TableView.TableHeaderView as UISearchBar;
                if (searchBar != null)
                {
                    //Enable the cancel button again....
                    foreach (var s in searchBar.Subviews)
                    {
                        var x = s as UIButton;
                        if (x != null)
                            x.Enabled = true;
                    }
                }
            }
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseDialogViewController"/> class.
		/// </summary>
		/// <param name="push">If set to <c>true</c> push.</param>
        public BaseDialogViewController(bool push)
			: this(push, "Back")
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseDialogViewController"/> class.
		/// </summary>
		/// <param name="push">If set to <c>true</c> push.</param>
		/// <param name="backButtonText">Back button text.</param>
		public BaseDialogViewController(bool push, string backButtonText)
			: base(new RootElement(""), push)
		{
			NavigationItem.BackBarButtonItem = new UIBarButtonItem(backButtonText, UIBarButtonItemStyle.Plain, null);
			SearchPlaceholder = "Search";
			Autorotate = true;
		}
        
        /// <summary>
        /// Makes the refresh table header view.
        /// </summary>
        /// <returns>
        /// The refresh table header view.
        /// </returns>
        /// <param name='rect'>
        /// Rect.
        /// </param>
        public override RefreshTableHeaderView MakeRefreshTableHeaderView(RectangleF rect)
        {
            //Replace it with our own
            return new RefreshView(rect);
        }
        
        public override void ViewDidLoad()
        {
            if (Title != null && Root != null)
                Root.Caption = Title;

            TableView.BackgroundColor = UIColor.FromRGB(227, 227, 227);
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
            TableView.SeparatorColor = UIColor.FromRGB(199, 199, 199);
            TableView.BackgroundView = null;

            if (Style != UITableViewStyle.Grouped)
            {
                var view = new UIView(new RectangleF(0, 0, View.Bounds.Width, 1));
                var shadowBackgroundView = new UIView(new RectangleF(0, 0, View.Bounds.Width, 1));
                shadowBackgroundView.Layer.ShadowOpacity = 0.4f; 
                shadowBackgroundView.Layer.ShadowColor = UIColor.Black.CGColor;
                shadowBackgroundView.Layer.ShadowOffset = new SizeF(0, -1.0f);
                var path = UIBezierPath.FromRoundedRect(shadowBackgroundView.Bounds, 0.0f).CGPath;
                shadowBackgroundView.Layer.ShadowPath = path;
                shadowBackgroundView.Layer.ShouldRasterize = true;
                view.AddSubview(shadowBackgroundView);
                TableView.TableFooterView = view;
            }

            base.ViewDidLoad();
        }

        sealed class RefreshView : RefreshTableHeaderView
        {
            public RefreshView(RectangleF rect)
                : base(rect)
            {
                BackgroundColor = UIColor.Clear;
                StatusLabel.BackgroundColor = UIColor.Clear;
                LastUpdateLabel.BackgroundColor = UIColor.Clear;
            }
        }
    }
}

