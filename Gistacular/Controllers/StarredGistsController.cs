using System;
using GitHubSharp.Models;
using System.Collections.Generic;
using MonoTouch.Dialog;

namespace Gistacular.Controllers
{
    public class StarredGistsController : GistsController
    {
        public StarredGistsController(bool push = true)
            : base(push)
        {
            Title = "Starred";
            UnevenRows = true;
        }

        protected override List<GistModel> GetData(bool force, int currentPage, out int nextPage)
        {
            var a = Application.Client.API.GetStarredGists(currentPage);
            nextPage = a.Next == null ? -1 : currentPage + 1;
            return a.Data;
        }
    }
}

