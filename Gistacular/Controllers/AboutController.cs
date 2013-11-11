using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using Gistacular.Elements;

namespace Gistacular.Controllers
{
    public class AboutController : BaseDialogViewController
    {
        static readonly string About = "Gistacular is a GitHub Gist client for the iOS platform. " +
                                       "It's the simplest and cleanest way to manage your Gist collection while on the go." + 
                                       "\n\nCreated By Dillon Buchanan";


        public AboutController()
            : base (true)
        {
            Title = "About";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var root = new RootElement(Title)
            {
                new Section()
                {
                    new Gistacular.Elements.MultilinedElement("Gistacular") { Value = About }
                },
                new Section()
                {
                    new StyledElement("Source Code", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://github.com/thedillonb/Gistacular")))
                },
                new Section(String.Empty, "Thank you for downloading. Enjoy!")
                {
                    new StyledElement("Follow Me On Twitter", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://twitter.com/thedillonb"))),
                    new StyledElement("Rate This App", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/us/app/gistacular/id670505001?ls=1&mt=8"))),
                    new StyledElement("App Version", NSBundle.MainBundle.InfoDictionary.ValueForKey(new NSString("CFBundleVersion")).ToString())
                }
            };

            root.UnevenRows = true;
            Root = root;
        }
    }
}

