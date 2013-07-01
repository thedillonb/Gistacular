using System;
using GitHubSharp.Models;
using System.Collections.Generic;

namespace Gistacular.Controllers
{
    public class MyGistsController : GistsController
    {
        public string User { get; private set; }

        public MyGistsController(string user, bool push = true)
            : base(push)
        {
            User = user;
            Title = "Gists";
            UnevenRows = true;
        }

        protected override List<GistModel> GetData(bool force, int currentPage, out int nextPage)
        {
            var a = Application.Client.API.GetGists(User, currentPage);
            nextPage = a.Next == null ? -1 : currentPage + 1;
            return a.Data;
        }
    }
}

