using System;
using MonoTouch.Dialog.Utilities;
using MonoTouch.UIKit;
using System.Drawing;

namespace Gistacular.Views
{
    public class ProfileView : UIView, IImageUpdated
    {
        UIImageView _imageView;

        public ProfileView(Uri uri)
            : base()
        {
            var size = new SizeF(32, 32);
            if (UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeLeft ||
                UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeRight)
            {
                size = new SizeF(24, 24);
            }

            this.Frame = new RectangleF(new PointF(4, 4), size);

            _imageView = new UIImageView(new RectangleF(new PointF(0, 0), size));
            _imageView.Image = ImageLoader.DefaultRequestImage(uri, this);
            _imageView.Layer.MasksToBounds = true;
            _imageView.Layer.CornerRadius = 4.0f;

            this.Layer.ShadowColor = UIColor.Black.CGColor;
            this.Layer.ShadowOpacity = 0.4f;
            this.Layer.ShadowOffset = new SizeF(0, 1);
            this.Layer.ShadowRadius = 4.0f;

            this.AddSubview(_imageView);
        }

        public void UpdatedImage(Uri uri)
        {
            var img = ImageLoader.DefaultRequestImage(uri, this);
            if (img == null)
                return;

            _imageView.Image = img;
            _imageView.SetNeedsDisplay();
        }

    }
}

