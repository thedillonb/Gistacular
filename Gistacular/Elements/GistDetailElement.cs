using System;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using Gistacular.Views;
using GitHubSharp.Models;
using MonoTouch.UIKit;
using MonoTouch.Dialog.Utilities;
using System.Drawing;

namespace Gistacular.Elements
{
    public class GistDetailElement : Element, IElementSizing, IImageUpdated
    {
        private static NSString CellName = new NSString("gistdetailelement");
        private GistModel _gist;
        public UIImage Image { get; set; }
        public Action Tapped;
        private bool _full;

        public GistDetailElement(GistModel gist, bool full = false)
            : base(string.Empty)
        {
            _gist = gist;
            _full = full;
        }

        protected override NSString CellKey
        {
            get
            {
                return CellName;
            }
        }

        public override void Selected(DialogViewController dvc, MonoTouch.UIKit.UITableView tableView, NSIndexPath path)
        {
            if (Tapped != null)
                Tapped ();
            tableView.DeselectRow (path, true);
        }

        public override MonoTouch.UIKit.UITableViewCell GetCell(MonoTouch.UIKit.UITableView tv)
        {
            var cell = tv.DequeueReusableCell(CellKey);
            if (cell == null){
                if (_full)
                    cell = GistDetailViewCell.Create();
                else
                    cell = GistDetailViewCell.CreateNoBottom();

                cell.BackgroundView = new CellBackgroundView();
            }

            cell.SelectionStyle = (Tapped != null) ? UITableViewCellSelectionStyle.Blue : UITableViewCellSelectionStyle.None;
            cell.Accessory = (Tapped != null) ? UITableViewCellAccessory.DisclosureIndicator : UITableViewCellAccessory.None;

            var gcell = cell as GistDetailViewCell;
            if (gcell == null)
                return cell;


            var str = string.IsNullOrEmpty(_gist.Description) ? "No Description" : _gist.Description;

            //We prefer the filename, so lets try and get it if it exists
            string filename = null;
            if (_gist.Files.Count > 0)
            {
                var iter = _gist.Files.Keys.GetEnumerator();
                iter.MoveNext();
                filename = iter.Current;
            }

            //Set the name (If we have no filename, fall back to the username)
            var name = (filename == null) ? (_gist.User == null ? "Unknown" : _gist.User.Login) : filename;
            var imageUri = (_gist.User == null) ? null : new Uri(_gist.User.AvatarUrl);
            var img = ImageLoader.DefaultRequestImage(imageUri, this);
            if (img != null)
                Image = img;

            if (_full)
                gcell.SetInformation(Image, name, _gist.CreatedAt.ToDaysAgo(), str, _gist.Forks.Count, _gist.Comments, 0);
            else
                gcell.SetInformation(Image, name, _gist.CreatedAt.ToDaysAgo(), str);

            return gcell;
        }

        public float GetHeight(MonoTouch.UIKit.UITableView tableView, NSIndexPath indexPath)
        {
            var str = string.IsNullOrEmpty(_gist.Description) ? "No Description" : _gist.Description;
            var height = str.MonoStringHeight(UIFont.SystemFontOfSize(12f), tableView.Bounds.Width - (46 + 28)) - 15f;
            return 90f + height;
        }

        public void UpdatedImage(Uri uri)
        {
            var img = ImageLoader.DefaultRequestImage(uri, this);
            if (img == null)
            {
                Console.WriteLine("DefaultRequestImage returned a null image");
                return;
            }

            Image = img;

            if (uri == null)
                return;
            var root = GetImmediateRootElement ();
            if (root == null || root.TableView == null)
                return;
            root.TableView.ReloadRows (new NSIndexPath [] { IndexPath }, UITableViewRowAnimation.None);
        }
    }
}

