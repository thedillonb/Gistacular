using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace Gistacular
{
    public class MenuSectionView : UIView
    {
        public MenuSectionView(string caption)
            : base(new RectangleF(0, 0, 320, 27))
        {
            UIImageView background = new UIImageView(Images.Components.MenuSectionBackground);
            background.Frame = this.Frame; 

            UILabel label = new UILabel(); 
            label.BackgroundColor = UIColor.Clear; 
            label.Opaque = false; 
            label.TextColor = UIColor.FromRGB(171, 171, 171); 
            label.Font =  UIFont.BoldSystemFontOfSize(12.5f);
            label.Frame = new RectangleF(8,1,200,23); 
            label.Text = caption; 
            label.ShadowColor = UIColor.FromRGB(0, 0, 0); 
            label.ShadowOffset = new SizeF(0, -1); 

            this.AddSubview(background); 
            this.AddSubview(label); 
        }
    }
}

