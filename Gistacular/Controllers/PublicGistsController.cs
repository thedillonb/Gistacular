using System;
using GitHubSharp.Models;
using System.Collections.Generic;
using MonoTouch.Dialog;

namespace Gistacular.Controllers
{
    public class PublicGistsController : GistsController
    {
        public PublicGistsController(bool push = true)
            : base(push)
        {
            Title = "Public Gists";
            UnevenRows = true;
        }

        protected override Element CreateElement(GistModel x)
        {
            var element = CreateGistElement(x);
            element.Tapped += () => NavigationController.PushViewController(new GistInfoController(x.Id), true);
            return element;
        }

        protected override List<GistModel> GetData(bool force, int currentPage, out int nextPage)
        {
            var a = Application.Client.API.GetPublicGists(currentPage);
            nextPage = a.Next == null ? -1 : currentPage + 1;
            return a.Data;
        }
    }
}

