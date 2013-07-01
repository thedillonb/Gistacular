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

            var gistMenuSection = new Section();
            root.Add(gistMenuSection);
            addGistSection.Add(new MenuElement("My Gists", () => { }, Images.Anonymous));
            addGistSection.Add(new MenuElement("Starred", () => { }, Images.Anonymous));


            var labelSection = new Section();
            root.Add(labelSection);
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
            TableView.BackgroundColor = UIColor.Clear;
            UIImage background = Images.Linen;
            View.BackgroundColor = UIColor.FromPatternImage(background);            
            TableView.SeparatorColor = UIColor.FromRGBA(128, 128, 128, 128);
            var view = new UIView(new RectangleF(0, 0, View.Bounds.Width, 10));
            view.BackgroundColor = UIColor.Clear;
            TableView.TableFooterView = view;

            //Prevent the scroll to top on this view
            this.TableView.ScrollsToTop = false;
        }
        
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
			var root = new RootElement(string.Empty); //TODO: This
            Title = root.Caption;
			OnCreateMenu(root);
			Root = root;
        }

		private class MenuElement : StyledElement
		{
			public MenuElement(string title, NSAction tapped, UIImage image)
				: base(title, tapped, image)
			{
				BackgroundColor = UIColor.Clear;
				TextColor = UIColor.White;
				DetailColor = UIColor.White;
			}
			
			public override UITableViewCell GetCell(UITableView tv)
			{
				var cell = base.GetCell(tv);
				cell.SelectedBackgroundView = new UIView { BackgroundColor = UIColor.FromRGBA(41, 41, 41, 200) };
				
				var f = cell.Subviews.Count(x => x.Tag == 1111);
				if (f == 0)
				{
					var v2 = new UIView(new RectangleF(0, cell.Frame.Height - 3, cell.Frame.Width, 1))
					{BackgroundColor = UIColor.FromRGBA(41, 41, 41, 64), Tag = 1111};
					cell.AddSubview(v2);
					
					var v = new UIView(new RectangleF(0, cell.Frame.Height - 2, cell.Frame.Width, 1))
					{ BackgroundColor = UIColor.FromRGBA(41, 41, 41, 200), Tag = 1111};
					cell.AddSubview(v);
				}
				
				return cell;
			}
		}
    }
}

