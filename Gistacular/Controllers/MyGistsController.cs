using System;
using GitHubSharp.Models;
using System.Collections.Generic;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using Gistacular.Elements;

namespace Gistacular.Controllers
{
    public class MyGistsController : GistsController
    {
        public MyGistsController(bool push = true)
            : base(push)
        {
            Title = "My Gists";
            UnevenRows = true;
        }

        protected override Element CreateElement(GistModel x)
        {
            var element = CreateGistElement(x);
            element.Tapped += () => NavigationController.PushViewController(new GistInfoController(x.Id, true) { Model = x }, true);
            return element;
        }

        protected override List<GistModel> GetData(bool force, int currentPage, out int nextPage)
        {
            var a = Application.Client.API.GetGists(Application.Account.Username, currentPage);
            nextPage = a.Next == null ? -1 : currentPage + 1;
            return a.Data;
        }

        private void Delete(Element element, Section section)
        {
            var e = element as NameTimeStringElement;
            if (e == null || e.Tag == null)
                return;

            var gist = e.Tag as GistModel;
            if (gist == null)
                return;

            this.DoWork(() => {
                Application.Client.API.DeleteGist(gist.Id);

                InvokeOnMainThread(() => {
                    section.Remove(element);
                });
            });
        }

        public override Source CreateSizingSource(bool unevenRows)
        {
            return new EditSource(this);
        }

        private class EditSource : SizingSource
        {
            private readonly MyGistsController _parent;
            public EditSource(MyGistsController dvc) 
                : base (dvc)
            {
                _parent = dvc;
            }

            public override bool CanEditRow(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                return true;
            }

            public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                return UITableViewCellEditingStyle.Delete;
            }

            public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                switch (editingStyle)
                {
                    case UITableViewCellEditingStyle.Delete:
                        var section = _parent.Root[indexPath.Section];
                        var element = section[indexPath.Row];
                        _parent.Delete(element, section);
                        break;
                }
            }
        }
    }
}

