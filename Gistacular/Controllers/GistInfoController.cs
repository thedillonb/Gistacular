using System;
using GitHubSharp.Models;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch;
using Gistacular.Elements;

namespace Gistacular.Controllers
{
    public class GistInfoController : Controller<GistModel>
    {
        public string Id { get; private set; }

        public GistInfoController(string id)
            : base(true, false)
        {
            Id = id;
            Style = MonoTouch.UIKit.UITableViewStyle.Plain;
            Title = "Gist";
            Root.UnevenRows = true;
        }

        protected override void OnRefresh()
        {
            var sec = new Section();
            sec.Add(GistsController.CreateGistElement(Model));

            var sec2 = new Section();

            foreach (var file in Model.Files.Keys)
            {
                var sse = new SubcaptionElement(file, Model.Files[file].Size + " bytes") { 
                    Accessory = MonoTouch.UIKit.UITableViewCellAccessory.DisclosureIndicator, 
                    LineBreakMode = MonoTouch.UIKit.UILineBreakMode.TailTruncation,
                    Lines = 1 
                };

                var fileSaved = file;
                sse.Tapped += () => NavigationController.PushViewController(new GistFileController(Model.Files[fileSaved]), true);
                sec2.Add(sse);
            }

            InvokeOnMainThread(delegate {
                var root = new RootElement(Title) { UnevenRows = true };
                root.Add(sec);
                root.Add(sec2);
                Root = root;
                ReloadData();
            });
        }

        protected override GistModel OnUpdate(bool forced)
        {
            return Application.Client.API.GetGist(Id).Data;
        }
    }
}

