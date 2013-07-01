using System;
using GitHubSharp.Models;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch;
using Gistacular.Elements;
using Gistacular.Views;
using System.Drawing;
using System.Collections.Generic;

namespace Gistacular.Controllers
{
    public class GistInfoController : Controller<GistModel>
    {
        TabButtonView _tabButtons;
        Section _contentSection;
        List<GistCommentModel> _comments;

        public string Id { get; private set; }

        public GistInfoController(string id)
            : base(true, true)
        {
            Id = id;
            Style = MonoTouch.UIKit.UITableViewStyle.Plain;
            Title = "Gist";

            _tabButtons = new TabButtonView(new RectangleF(0, 0, this.TableView.Bounds.Width, 42), "Files", "Comments");
            _tabButtons.SegmentChanged = SegmentedChanged;
        }

        private void SegmentedChanged(int index)
        {
            if (index == 0)
            {
                LoadFiles(_contentSection);
            }
            else if (index == 1)
            {
                if (_comments == null)
                {
                    this.DoWork(() => {
                        _comments = Application.Client.API.GetGistComments(Id).Data;
                        InvokeOnMainThread(() => {
                            if (_tabButtons.Selected != 1) 
                                return;
                            LoadComments(_contentSection);
                        });
                    });
                }
                else
                {
                    LoadComments(_contentSection);
                }
            }
        }

        private void LoadComments(Section section)
        {
            section.Clear();
            foreach (var comment in _comments)
            {
                var thisComment = comment;

                var sse = new NameTimeStringElement() { 
                    Time = thisComment.CreatedAt, 
                    String = thisComment.Body, 
                    Lines = 4, 
                    Image = Images.Anonymous,
                };

                sse.Name = thisComment.User == null ? "Unknown" : thisComment.User.Login;
                sse.ImageUri = (thisComment.User == null) ? null : new Uri(thisComment.User.AvatarUrl);

                section.Add(sse);
            }
        }

        private void LoadFiles(Section section)
        {
            section.Clear();
            foreach (var file in Model.Files.Keys)
            {
                var sse = new SubcaptionElement(file, Model.Files[file].Size + " bytes") { 
                    Accessory = MonoTouch.UIKit.UITableViewCellAccessory.DisclosureIndicator, 
                    LineBreakMode = MonoTouch.UIKit.UILineBreakMode.TailTruncation,
                    Lines = 1 
                };

                var fileSaved = file;
                sse.Tapped += () => NavigationController.PushViewController(new GistFileController(Model.Files[fileSaved]), true);
                section.Add(sse);
            }
        }

        protected override void OnRefresh()
        {
            var sec = new Section();
            sec.Add(GistsController.CreateGistElement(Model));

            InvokeOnMainThread(delegate {

                _contentSection = new Section() { HeaderView = _tabButtons };
                if (_tabButtons.Selected == 0)
                    LoadFiles(_contentSection);
                else if (_tabButtons.Selected == 1)
                    LoadComments(_contentSection);

                var root = new RootElement(Title) { UnevenRows = true };
                root.Add(sec);
                root.Add(_contentSection);
                Root = root;
            });
        }

        protected override GistModel OnUpdate(bool forced)
        {
            var selected = 0;
            InvokeOnMainThread(delegate { selected = _tabButtons.Selected; });

            //If we're currently looking at the comments then update it. If not, just invalidate it
            if (selected == 1)
                _comments = Application.Client.API.GetGistComments(Id).Data;
            else
                _comments = null;

            return Application.Client.API.GetGist(Id).Data;
        }
    }
}

