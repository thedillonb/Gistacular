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
            var element = CreateGistElement(x);
            element.Tapped += () => NavigationController.PushViewController(new GistInfoController(x.Id) { Model = x }, true);
            return element;
        }

        public static NameTimeStringElement CreateGistElement(GistModel x)
        {
            var str = string.IsNullOrEmpty(x.Description) ? "No Description" : x.Description;
            var sse = new NameTimeStringElement() { 
                Time = x.UpdatedAt, 
                String = str, 
                Lines = 4, 
                Image = Images.Anonymous,
            };

            //We prefer the filename, so lets try and get it if it exists
            string filename = null;
            if (x.Files.Count > 0)
            {
                var iter = x.Files.Keys.GetEnumerator();
                iter.MoveNext();
                filename = iter.Current;
            }

            //Set the name (If we have no filename, fall back to the username)
            sse.Name = (filename == null) ? (x.User == null ? "Unknown" : x.User.Login) : filename;
            sse.ImageUri = (x.User == null) ? null : new Uri(x.User.AvatarUrl);
            return sse;
        }
    }
}

