using System;
using GitHubSharp.Models;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch;
using Gistacular.Elements;
using Gistacular.Views;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace Gistacular.Controllers
{
    public class GistInfoController : Controller<GistModel>
    {
        TabButtonView _tabButtons;
        Section _contentSection;
        List<GistCommentModel> _comments;

        public string Id { get; private set; }

        public GistInfoController(string id, bool owned = false)
            : base(true, true)
        {
            Id = id;
            Style = MonoTouch.UIKit.UITableViewStyle.Plain;
            Title = "Gist";

            _tabButtons = new TabButtonView(new RectangleF(0, 0, this.TableView.Bounds.Width, 42), "Files", "Comments", "Forks");
            _tabButtons.SegmentChanged = SegmentedChanged;

            if (owned)
            {
                NavigationItem.RightBarButtonItem = new UIBarButtonItem(NavigationButton.Create(Images.EditButton, () => {

                }));
            }
            else
            {
                NavigationItem.RightBarButtonItem = new UIBarButtonItem(NavigationButton.Create(Images.ForkButton, () => {
                    NavigationItem.RightBarButtonItem.Enabled = false;

                    this.DoWork(() => {
                        var forkedGist = Application.Client.API.ForkGist(id);
                        InvokeOnMainThread(delegate {
                            NavigationController.PushViewController(new GistInfoController(forkedGist.Data.Id, true), true);
                        });
                    }, null, () => {
                        NavigationItem.RightBarButtonItem.Enabled = true;
                    });
                }));
            }

            //The bottom bar
//            ToolbarItems = new []
//            {
//                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
//                new UIBarButtonItem(UIBarButtonSystemItem.Add),
//                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
//                new UIBarButtonItem(UIBarButtonSystemItem.Add),
//                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
//                new UIBarButtonItem(UIBarButtonSystemItem.Add),
//                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace)
//            };
        }

        /// <summary>
        /// Used when we retrieve data so we can add more comments and order them so we don't have to order during rendering
        /// </summary>
        /// <param name="comments">Comments.</param>
        private void AddToCommentList(List<GistCommentModel> comments)
        {
            //Never should happen, but invalidate it!
            if (comments == null)
            {
                _comments = null;
                return;
            }

            if (_comments == null)
                _comments = comments;
            else
                _comments.AddRange(comments);

            _comments = _comments.OrderByDescending(x => x.CreatedAt).ToList();
        }

        private void SegmentedChanged(int index)
        {
            if (index == 0)
            {
                OnRefresh();
            }
            else if (index == 1)
            {
                if (_comments == null)
                {
                    this.DoWork(() => {
                        AddToCommentList(Application.Client.API.GetGistComments(Id).Data);
                        InvokeOnMainThread(() => {
                            if (_tabButtons.Selected != 1) 
                                return;
                            OnRefresh();
                        });
                    });
                }
                else
                {
                    OnRefresh();
                }
            }
            else if (index == 2)
            {
                //Hack
                if (Model.Forks == null)
                {
                    Model = null;
                    Refresh(true);
                }
                else
                    OnRefresh();
            }
        }

        private void LoadComments(Section section)
        {
            section.Clear();
            if (_comments == null)
                return;

            foreach (var comment in _comments)
            {
                try
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
                catch (Exception e)
                {
                    Utilities.LogException("Unable to load comments!", e);
                }
            }
        }

        private void LoadForks(Section section)
        {
            section.Clear();
            if (Model == null || Model.Forks == null)
                return;

            foreach (var fork in Model.Forks)
            {
                try
                {
                    var sse = new SubcaptionElement(fork.User.Login, fork.CreatedAt.ToDaysAgo()) { 
                        Accessory = MonoTouch.UIKit.UITableViewCellAccessory.DisclosureIndicator, 
                        LineBreakMode = MonoTouch.UIKit.UILineBreakMode.TailTruncation,
                        Lines = 1,
                    };

                    sse.Image = Images.Anonymous;
                    sse.ImageUri = new Uri(fork.User.AvatarUrl);

                    var id = fork.Url.Substring(fork.Url.LastIndexOf('/') + 1);
                    sse.Tapped += () => NavigationController.PushViewController(new GistInfoController(id), true);
                    section.Add(sse);
                }
                catch (Exception e)
                {
                    Utilities.LogException("Unable to load forks!", e);
                }
            }
        }

        private void LoadFiles(Section section)
        {
            section.Clear();
            if (Model == null || Model.Files == null)
                return;

            foreach (var file in Model.Files.Keys.OrderBy(x => x))
            {
                try
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
                catch (Exception e)
                {
                    Utilities.LogException("Unable to load files!", e);
                }
            }
        }

        protected override void OnRefresh()
        {
            var sec = new Section();
            sec.Add(GistsController.CreateGistElement(Model));

            InvokeOnMainThread(delegate {
                var root = new RootElement(Title) { UnevenRows = true };
                var contentSection = new Section() { HeaderView = _tabButtons };
                root.Add(sec);
                root.Add(contentSection);

                if (_tabButtons.Selected == 0)
                    LoadFiles(contentSection);
                else if (_tabButtons.Selected == 1)
                    LoadComments(contentSection);
                else if (_tabButtons.Selected == 2)
                    LoadForks(contentSection);

                Root = root;
                _contentSection = contentSection;
            });
        }

        protected override GistModel OnUpdate(bool forced)
        {
            var selected = 0;
            InvokeOnMainThread(delegate { selected = _tabButtons.Selected; });

            //If we're currently looking at the comments then update it. If not, just invalidate it
            if (selected == 1)
                AddToCommentList(Application.Client.API.GetGistComments(Id).Data);
            else
                _comments = null;

            return Application.Client.API.GetGist(Id).Data;
        }

        public override void ViewWillAppear(bool animated)
        {
            if (ToolbarItems != null)
                NavigationController.SetToolbarHidden(IsSearching, animated);
            base.ViewWillAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (ToolbarItems != null)
                NavigationController.SetToolbarHidden(true, animated);
        }
    }
}

