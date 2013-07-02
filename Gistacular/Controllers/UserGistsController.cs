using System;
using GitHubSharp.Models;
using System.Collections.Generic;

namespace Gistacular.Controllers
{
    public class UserGistsController : GistsController
    {
        string _user;

        public UserGistsController(string user)
            : base(true)
        {
            _user = user;
            Title = user + "'s Gists";
            UnevenRows = true;
        }

        protected override List<GistModel> GetData(bool force, int currentPage, out int nextPage)
        {
            var a = Application.Client.API.GetGists(_user, currentPage);
            nextPage = a.Next == null ? -1 : currentPage + 1;
            return a.Data;
        }
    }
}

