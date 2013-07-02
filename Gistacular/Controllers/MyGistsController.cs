using System;
using GitHubSharp.Models;
using System.Collections.Generic;
using MonoTouch.Dialog;

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
    }
}

