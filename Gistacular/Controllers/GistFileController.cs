using GitHubSharp.Models;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System;
using Gistacular.Views;

namespace Gistacular.Controllers
{
    public class GistFileController : FileSourceController
    {
        GistFileModel _model;
        string _content;

        public GistFileController(GistFileModel model)
        {
            Title = model.Filename;
            _model = model;

            //We can view markdown!
            if (model.Language.Equals("Markdown"))
            {
                NavigationItem.RightBarButtonItem = new UIBarButtonItem(NavigationButton.Create(Images.ViewButton, () => {
                    NavigationController.PushViewController(new GistViewableFileController(model, _content), true);
                }));
            }
        }

        protected override void Request()
        {
            _content = Application.Client.API.GetGistFile(_model.RawUrl);
            LoadSourceData(System.Security.SecurityElement.Escape(_content));
        }
    }

    public class GistViewableFileController : FileSourceController
    {
        GistFileModel _model;
        string _content;

        public GistViewableFileController(GistFileModel model, string content)
        {
            _content = content;
            _model = model;
            Title = model.Filename;
        }

        protected override void Request()
        {
            //If we already have it no need to request it again
            if (_content == null)
                _content = Application.Client.API.GetGistFile(_model.RawUrl);

            var data = Application.Client.API.GetMarkdown(_content);
            LoadRawData(data);
        }
    }

    public abstract class FileSourceController : WebViewController
    {
        bool _loaded = false;

        public FileSourceController()
            : base(false)
        {
            Web.DataDetectorTypes = UIDataDetectorType.None;
            Title = "Source";
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //Do the request
            if (_loaded == false)
            {
                this.DoWork(() => {
                    Request();
                    _loaded = true;
                });
            }
        }

        protected override void OnLoadError(object sender, UIWebErrorArgs e)
        {
            base.OnLoadError(sender, e);

            //Can't load this!
            ErrorView.Show(this.View, "Unable to display this type of file.");
        }

        protected abstract void Request();

        protected void LoadRawData(string data)
        {
            InvokeOnMainThread(delegate {
                Web.LoadHtmlString(data, null);
            });
        }

        protected void LoadSourceData(string data)
        {
            InvokeOnMainThread(delegate {
                var html = System.IO.File.ReadAllText("SourceBrowser/index.html");
                var filled = html.Replace("{DATA}", data);

                var url = NSBundle.MainBundle.BundlePath + "/SourceBrowser";
                url = url.Replace("/", "//").Replace(" ", "%20");

                Web.LoadHtmlString(filled, NSUrl.FromString("file:/" + url + "//"));
            });
        }
    }
}

