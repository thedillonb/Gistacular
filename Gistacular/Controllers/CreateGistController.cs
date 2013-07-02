using System;
using GitHubSharp.Models;
using System.Collections.Generic;
using MonoTouch.Dialog;
using Gistacular.Views;
using MonoTouch.UIKit;

namespace Gistacular.Controllers
{
    public class CreateGistController : BaseDialogViewController
    {
        public CreateGistController()
            : base(true)
        {
            Title = "Create Gist";
            Style = UITableViewStyle.Plain;
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(NavigationButton.Create(Images.AddButton, () => AddFile()));
        }

        private void AddFile()
        {
            var createController = new CreateGistFileController();
            NavigationController.PushViewController(createController, true);
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var section = new Section();
            section.Add(new Entry());
            Root = new RootElement(Title) { section };
        }
    }

    public class Entry : MonoTouch.Dialog.EntryElement
    {
        public Entry()
            : base("Description", "(Optional)", String.Empty)
        {
            TitleFont = UIFont.BoldSystemFontOfSize(10f);
            EntryFont = UIFont.SystemFontOfSize(10f);
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = base.GetCell(tv);
            cell.BackgroundView = new CellBackgroundView();
            cell.ContentView.BackgroundColor = UIColor.Clear;

            foreach (var views in cell.ContentView.Subviews)
                views.BackgroundColor = UIColor.Clear;

            return cell;
        }
    }
}

