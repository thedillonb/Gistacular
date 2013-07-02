using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using GitHubSharp.Models;

namespace Gistacular.Views
{
    public partial class GistDetailViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("GistDetailViewCell");
        public static readonly UINib Nib;

        static GistDetailViewCell()
        {
            Nib = UINib.FromName("GistDetailViewCell", NSBundle.MainBundle);
        }

        public GistDetailViewCell(IntPtr handle) : base (handle)
        {

        }

        public static GistDetailViewCell Create()
        {
            var cell = (GistDetailViewCell)Nib.Instantiate(null, null)[0];
            cell.MainImage.Layer.ShadowColor = UIColor.DarkGray.CGColor;
            cell.MainImage.Layer.ShadowOffset = new SizeF(0, 0);
            cell.MainImage.Layer.ShadowOpacity = 0.7f;
            return cell;
        }

        public static GistDetailViewCell CreateNoBottom()
        {
            var cell = Create();
            cell.Forks.RemoveFromSuperview();
            cell.ForkImage.RemoveFromSuperview();
            cell.Comments.RemoveFromSuperview();
            cell.CommentImage.RemoveFromSuperview();
            cell.Stars.RemoveFromSuperview();
            cell.StarsImage.RemoveFromSuperview();
            return cell;
        }

        public void SetInformation(UIImage image, string name, string date, string description)
        {
            MainImage.Image = image;
            Description.Text = description;
            Name.Text = name;
            Date.Text = date;
        }

        public void SetInformation(UIImage image, string name, string date, string description, int forks, int comments, int stars)
        {
            MainImage.Image = image;
            Description.Text = description;
            Name.Text = name;
            Date.Text = date;
            Forks.Text = forks + " forks";
            Comments.Text = comments + " comments";
            Stars.Text = stars + " stars";
        }
    }
}

