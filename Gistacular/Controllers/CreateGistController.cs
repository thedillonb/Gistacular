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
        private GistCreateModel _model;
        private TrueFalseElement _public;
        public Action<string> Created;

        public CreateGistController()
            : base(true)
        {
            Title = "Create Gist";
            Style = UITableViewStyle.Grouped;

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem (NavigationButton.Create(Images.Buttons.Cancel, Discard));
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(NavigationButton.Create(Images.Buttons.Save, Save));

            _model = new GistCreateModel() { Public = true };
            _model.Files = new Dictionary<string, GistCreateModel.File>();
        }

        private void Discard()
        {
            DismissViewController(true, null);
        }

        private void Save()
        {
            _model.Public = _public.Value;
            this.DoWork(() => {
                var newGist = Application.Client.API.CreateGist(_model);
                InvokeOnMainThread(() => {
                    DismissViewController(true, () => {
                        if (Created != null)
                            Created(newGist.Data.Id);
                    });
                });
            });
        }

        private void AddFile()
        {
            var createController = new ModifyGistFileController();
            createController.Save = (name, content) => {
                if (_model.Files.ContainsKey(name))
                    throw new InvalidOperationException("A filename by that type already exists");
                _model.Files.Add(name, new GistCreateModel.File { Content = content });
            };
            NavigationController.PushViewController(createController, true);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var root = new RootElement(Title) { UnevenRows = true };
            var section = new Section();
            root.Add(section);

            var desc = new MultilinedElement("Description") { Value = _model.Description };
            desc.Tapped += ChangeDescription;
            section.Add(desc);

            if (_public == null)
                _public = new TrueFalseElement("Public"); 
            _public.Value = _model.Public;
            section.Add(_public);

            var fileSection = new Section();
            root.Add(fileSection);

            foreach (var file in _model.Files.Keys)
            {
                var key = file;
                var size = System.Text.ASCIIEncoding.UTF8.GetByteCount(_model.Files[file].Content);
                var el = new StyledElement(file, size + " bytes", UITableViewCellStyle.Subtitle) { Accessory = UITableViewCellAccessory.DisclosureIndicator };
                el.Tapped += () => {
                    var createController = new ModifyGistFileController(key, _model.Files[key].Content);
                    createController.Save = (name, content) => {

                        //If different name & exists somewhere else
                        if (!name.Equals(key) && _model.Files.ContainsKey(name))
                            throw new InvalidOperationException("A filename by that type already exists");

                        //Remove old
                        _model.Files.Remove(key);
                        _model.Files.Add(name, new GistCreateModel.File { Content = content });
                    };

                    NavigationController.PushViewController(createController, true);
                };
                fileSection.Add(el);
            }

            fileSection.Add(new StyledElement("Add New File", AddFile));

            Root = root;
        }

        private void ChangeDescription()
        {
            var composer = new Composer { Title = "Description", Text = _model.Description };
            composer.NewComment(this, () => {
                var text = composer.Text;
                _model.Description = text;
                composer.CloseComposer();
            });
        }

        public override Source CreateSizingSource(bool unevenRows)
        {
            return new EditSource(this);
        }

        private void Delete(Element element)
        {
            _model.Files.Remove(element.Caption);
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

