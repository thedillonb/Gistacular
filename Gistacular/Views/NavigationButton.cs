using System;
using MonoTouch.UIKit;

namespace Gistacular.Views
{
    public static class NavigationButton
    {
        public static UIButton Create(UIImage image, Action action = null)
        {
            var button = new UIButton(UIButtonType.Custom);
            button.Frame = new System.Drawing.RectangleF(0, 0, 40f, 40f);
            button.SetImage(image, UIControlState.Normal);
            button.Layer.ShadowColor = UIColor.Black.CGColor;
            button.Layer.ShadowOpacity = 0.4f;
            button.Layer.ShadowOffset = new System.Drawing.SizeF(0, 1.0f);
            if (action != null)
                button.TouchUpInside += (s, e) => action();
            return button;
        }
    }
}

