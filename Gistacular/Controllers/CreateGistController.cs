using System;
using GitHubSharp.Models;
using System.Collections.Generic;
using MonoTouch.Dialog;
using Gistacular.Views;
using MonoTouch.UIKit;
using Gistacular.Elements;
using System.Drawing;

namespace Gistacular.Controllers
{
    public class CreateGistController : BaseDialogViewController
    {
        private GistModel _editModel;
        private bool _isEdit;


        public CreateGistController(GistModel editModel = null)
            : base(true)
        {
            _editModel = editModel;
            Title = _isEdit ? "Edit Gist" : "Create Gist";
            Style = UITableViewStyle.Grouped;

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem (NavigationButton.Create(Images.Buttons.Cancel, Discard));
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(NavigationButton.Create(Images.Buttons.Save, Save));
        }

        private new void Delete()
        {
            this.DoWork(() => {
                Application.Client.API.DeleteGist("");
                InvokeOnMainThread(() => {
                    DismissViewController(true, null);
                });
            });
        }

        private void Discard()
        {
            DismissViewController(true, null);
        }

        private void Save()
        {
            this.DoWork(() => {


            });
        }

        private void AddFile()
        {
            var createController = new CreateGistFileController();
            NavigationController.PushViewController(createController, true);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var root = new RootElement(Title) { UnevenRows = true };
            var section = new Section();
            root.Add(section);


            var desc = new MultilinedElement("Description");
            desc.Tapped += () =>
            {
                var composer = new Composer { Title = "Description", Text = desc.Value, ActionButtonText = "Save" };
                composer.NewComment(this, () => {
                    var text = composer.Text;
                    desc.Value = text;
                    composer.CloseComposer();
                    Root.Reload(desc, UITableViewRowAnimation.None);
                });
            };

            section.Add(desc);
            section.Add(new TrueFalseElement("Public") { Value = true });

            var fileSection = new Section();
            root.Add(fileSection);
            fileSection.Add(new StyledElement("Add New File", AddFile));

            Root = root;
        }

        
        public override Source CreateSizingSource(bool unevenRows)
        {
            return new EditSource(this);
        }

        private void Delete(Element element)
        {
          
        }

        private class EditSource : SizingSource
        {
            private readonly CreateGistController _parent;
            public EditSource(CreateGistController dvc) 
                : base (dvc)
            {
                _parent = dvc;
            }

            public override bool CanEditRow(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                return (indexPath.Section == 1 && indexPath.Row != (_parent.Root[0].Count - 1));
            }

            public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                if (indexPath.Section == 1 && indexPath.Row != (_parent.Root[0].Count - 1))
                    return UITableViewCellEditingStyle.Delete;
                return UITableViewCellEditingStyle.None;
            }

            public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                switch (editingStyle)
                {
                    case UITableViewCellEditingStyle.Delete:
                        var section = _parent.Root[indexPath.Section];
                        var element = section[indexPath.Row];
                        _parent.Delete(element);
                        section.Remove(element);
                        break;
                }
            }
        }
    }
}

