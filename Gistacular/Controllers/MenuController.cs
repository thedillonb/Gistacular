using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using System.Linq;
using Gistacular.Elements;
using Gistacular.Views;

namespace Gistacular.Controllers
{
    public class MenuController : DialogViewController
    {
        UILabel _title;

        public MenuController()
            : base(UITableViewStyle.Plain, new RootElement("Gistacular"))
        {
            Autorotate = true;
            _title = new UILabel(new RectangleF(0, 40, 320, 40));
            _title.TextAlignment = UITextAlignment.Left;
            _title.BackgroundColor = UIColor.Clear;
            _title.Font = UIFont.BoldSystemFontOfSize(20f);
            _title.TextColor = UIColor.FromRGB(246, 246, 246);
            _title.ShadowColor = UIColor.FromRGB(21, 21, 21);
            _title.ShadowOffset = new SizeF(0, 1);
            NavigationItem.TitleView = _title;
        }

		/// <summary>
		/// Invoked when it comes time to set the root so the child classes can create their own menus
		/// </summary>
		private void OnCreateMenu(RootElement root)
		{
            var addGistSection = new Section();
            root.Add(addGistSection);
            addGistSection.Add(new MenuElement("New Gist", () => {
                var gistController = new CreateGistController();
                var navController = new UINavigationController(gistController);
                PresentViewController(navController, true, null);
            }, Images.NewGist));

            var gistMenuSection = new Section() { HeaderView = new MenuSectionView("Gists") };
            root.Add(gistMenuSection);
            gistMenuSection.Add(new MenuElement("My Gists", () => NavigationController.PushViewController(new MyGistsController(), true), Images.MyGists));
            gistMenuSection.Add(new MenuElement("Starred", () => NavigationController.PushViewController(new StarredGistsController(), true), Images.Star2));
            gistMenuSection.Add(new MenuElement("Public", () => NavigationController.PushViewController(new PublicGistsController(), true), Images.Public));

//            var labelSection = new Section() { HeaderView = new MenuSectionView("Tags") };
//            root.Add(labelSection);
//            labelSection.Add(new MenuElement("Add New Tag", () => { }, null));

            var moreSection = new Section() { HeaderView = new MenuSectionView("More") };
            root.Add(moreSection);
            moreSection.Add(new MenuElement("Settings", () => { }, Images.Settings));
            moreSection.Add(new MenuElement("Feedback & Support", () => { 
                var config = UserVoice.UVConfig.Create("http://gistacular.uservoice.com", "lYY6AwnzrNKjHIkiiYbbqA", "9iLse96r8yki4ZKknfHKBlWcbZAH9g8yQWb9fuG4");
                UserVoice.UserVoice.PresentUserVoiceInterface(this, config);
            }, Images.Feedback));
            moreSection.Add(new MenuElement("Logout", () => { }, Images.Logout));
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(new ProfileView(new System.Uri(Application.Account.AvatarUrl)));

			//Add some nice looking colors and effects
            TableView.SeparatorColor = UIColor.FromRGB(14, 14, 14);
            TableView.TableFooterView = new UIView(new RectangleF(0, 0, View.Bounds.Width, 0));
            TableView.BackgroundColor = UIColor.FromRGB(34, 34, 34);

            //Prevent the scroll to top on this view
            this.TableView.ScrollsToTop = false;
        }
        
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _title.Text = Application.Account.Username;

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
                Image = image;
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

