using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using Gistacular.Views;

namespace Gistacular.Controllers
{
    public class CreateGistFileController : BaseDialogViewController
    {
        CustomEntryElement _filename;
        TextEntryElement _content;
        public Action<string, string> Save;


        public CreateGistFileController()
            : base(true)
        {
            Title = "New File";
            Style = UITableViewStyle.Plain;
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(NavigationButton.Create(Images.Buttons.Save, () => {
                try
                {
                    if (Save != null)
                        Save(_filename.Value, string.Empty);
                    NavigationController.PopViewControllerAnimated(true);
                }
                catch (Exception e)
                {
                    MonoTouch.Utilities.ShowAlert("Error", e.Message);
                    return;
                }
            }));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var root = new RootElement(Title) { UnevenRows = true };
            var sec = new Section();
            root.Add(sec);

            sec.Add((_filename = new CustomEntryElement(String.Empty)));
            sec.Add((_content = new TextEntryElement(String.Empty)));
            Root = root;
        }

        private class TextEntryElement : Element, IElementSizing
        {

            UITextView _text;


            public TextEntryElement(string content)
                : base(String.Empty)
            {
                _text = new UITextView() { ScrollEnabled = false };
                _text.AutoresizingMask = UIViewAutoresizing.All;
                _text.ContentInset = new UIEdgeInsets(4, 4, 4, 4);
                _text.AutocorrectionType = UITextAutocorrectionType.No;
                _text.AutocapitalizationType = UITextAutocapitalizationType.None;
                _text.Font = UIFont.SystemFontOfSize(12f);
                _text.Changed += HandleChanged;
            }

            void HandleChanged (object sender, EventArgs e)
            {
                GetImmediateRootElement().Reload(this, UITableViewRowAnimation.None);
                _text.BecomeFirstResponder();
            }

            public override UITableViewCell GetCell(UITableView tv)
            {
                var cell = base.GetCell(tv);
                _text.Frame = cell.Bounds;
                cell.AddSubview(_text);
                return cell;
            }

            public float GetHeight(UITableView tableView, NSIndexPath indexPath)
            {
                var height = _text.Text.MonoStringHeight(UIFont.SystemFontOfSize(12f), tableView.Bounds.Width);
                Console.WriteLine("Height: " + height);
                return 8f + height;
            }
        }


        private class CustomEntryElement : EntryElement
        {
            public CustomEntryElement(string value)
                : base("Name", String.Empty, value)
            {
                AutocorrectionType = UITextAutocorrectionType.No;
                AutocapitalizationType = UITextAutocapitalizationType.None;
                TitleFont = UIFont.BoldSystemFontOfSize(14f);
                TitleColor = UIColor.FromRGB(41, 41, 41);
            }

            public override UITableViewCell GetCell(UITableView tv)
            {
                var cell = base.GetCell(tv);
                cell.ContentView.BackgroundColor = UIColor.White;
                foreach (var view in cell.ContentView.Subviews)
                    view.BackgroundColor = cell.ContentView.BackgroundColor;
                return cell;
            }
        }
    }
}

