using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using System.Linq;
using Gistacular.Elements;

namespace Gistacular.Controllers
{
    public class MenuController : DialogViewController
    {
		public MenuController()
            : base(UITableViewStyle.Plain, new RootElement("Gistacular"))
        {
            Autorotate = true;
//            if (Application.Account != null && !string.IsNullOrEmpty(Application.Account.Username))
//                Root.Caption = Application.Account.Username;
        }

		/// <summary>
		/// Invoked when it comes time to set the root so the child classes can create their own menus
		/// </summary>
		private void OnCreateMenu(RootElement root)
		{
            var addGistSection = new Section();
            root.Add(addGistSection);
            addGistSection.Add(new MenuElement("New Gist", () => { }, Images.Anonymous));

            var gistMenuSection = new Section() { HeaderView = new MenuSectionView("Gists") };
            root.Add(gistMenuSection);
            gistMenuSection.Add(new MenuElement("My Gists", () => { }, Images.Anonymous));
            gistMenuSection.Add(new MenuElement("Starred", () => { }, Images.Anonymous));


            var labelSection = new Section() { HeaderView = new MenuSectionView("Tags") };
            root.Add(labelSection);
            labelSection.Add(new MenuElement("Add New Tag", () => { }, Images.Anonymous));

            var moreSection = new Section() { HeaderView = new MenuSectionView("More") };
            root.Add(moreSection);
            moreSection.Add(new MenuElement("Settings", () => { }, Images.Anonymous));
		}
        
        protected virtual void NavPush(UIViewController controller)
        {
            NavigationController.PushViewController(controller, false);
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(Images.ThreeLines, UIBarButtonItemStyle.Bordered, (s, e) => {
                //var n = new UINavigationController(new SettingsController());
                //this.PresentModalViewController(n, true);
            });

			//Add some nice looking colors and effects
            TableView.SeparatorColor = UIColor.FromRGB(14, 14, 14);
            var view = new UIView(new RectangleF(0, 0, View.Bounds.Width, 10));
            view.BackgroundColor = UIColor.Clear;
            TableView.TableFooterView = view;

            //Prevent the scroll to top on this view
            this.TableView.ScrollsToTop = false;

            TableView.BackgroundColor = UIColor.FromRGB(34, 34, 34);
            TableView.BackgroundView = null;
        }
        
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
			var root = new RootElement(Application.Account.Username);
            Title = root.Caption;
			OnCreateMenu(root);
			Root = root;
        }

		private class MenuElement : StyledElement
		{
			public MenuElement(string title, NSAction tapped, UIImage image)
				: base(title, tapped)
			{
				BackgroundColor = UIColor.Clear;
				TextColor = UIColor.FromRGB(213, 213, 213);
				DetailColor = UIColor.White;
			}
			
			public override UITableViewCell GetCell(UITableView tv)
			{
				var cell = base.GetCell(tv);
                cell.TextLabel.ShadowColor = UIColor.Black;
                cell.TextLabel.ShadowOffset = new SizeF(0, -1); 
                cell.SelectedBackgroundView = new UIView { BackgroundColor = UIColor.FromRGB(25, 25, 25) };
				
				var f = cell.Subviews.Count(x => x.Tag == 1111);
				if (f == 0)
				{
					var v = new UIView(new RectangleF(0, 0, cell.Frame.Width, 1))
					{ BackgroundColor = UIColor.FromRGB(44, 44, 44), Tag = 1111};
					cell.AddSubview(v);
				}
				
				return cell;
			}
		}
    }
}

