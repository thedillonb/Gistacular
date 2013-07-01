using System;
using GitHubSharp.Models;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using Gistacular.Elements;

namespace Gistacular.Controllers
{
    public abstract class GistsController : ListController<GistModel>
    {
        public GistsController(bool push = true)
            : base(push)
        {
        }

        protected override Element CreateElement(GistModel x)
        {
            var str = string.IsNullOrEmpty(x.Description) ? "Gist " + x.Id : x.Description;
            var sse = new NameTimeStringElement() { 
                Time = x.UpdatedAt, 
                String = str, 
                Lines = 4, 
                Image = Images.Anonymous,
            };

            sse.Name = (x.User == null) ? "Anonymous" : x.User.Login;
            sse.ImageUri = (x.User == null) ? null : new Uri(x.User.AvatarUrl);
            //sse.Tapped += () => NavigationController.PushViewController(new GistInfoController(x.Id) { Model = x }, true);
            return sse;
        }
    }
}

