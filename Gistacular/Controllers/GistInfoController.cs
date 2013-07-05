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
        List<GistCommentModel> _comments;
        UIBarButtonItem _shareButton;
        UIButton _starButton;
        bool _starred = false;

        public string Id { get; private set; }

        public GistInfoController(string id)
            : base(true, true)
        {
            Id = id;
            Style = MonoTouch.UIKit.UITableViewStyle.Plain;
            Title = "Gist";

            _tabButtons = new TabButtonView(new RectangleF(0, 0, this.TableView.Bounds.Width, 42), "Files", "Comments", "Forks");
            _tabButtons.SegmentChanged = SegmentedChanged;
           
            //The bottom bar
            ToolbarItems = new []
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem((_starButton = ToolbarButton.Create(Images.Buttons.Star, StarButtonPress))),
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem(ToolbarButton.Create(Images.Buttons.User, UserButtonPress)),
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem(ToolbarButton.Create(Images.Buttons.Comment, CommentButtonPress)),
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                (_shareButton = new UIBarButtonItem(ToolbarButton.Create(Images.Buttons.Share, ShareButtonPress))),
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace)
            };
        }

        private void UpdateOwned()
        {
            var owned = false;
            if (Model != null && Model.User != null)
            {
                if (Model.User.Login.ToLower().Equals(Application.Account.Username.ToLower()))
                    owned = true;
            }
            
            if (owned)
            {
                NavigationItem.RightBarButtonItem = new UIBarButtonItem(NavigationButton.Create(Images.Buttons.Edit, () => {
                }));
            }
            else
            {
                NavigationItem.RightBarButtonItem = new UIBarButtonItem(NavigationButton.Create(Images.Buttons.Fork, () => {
                    NavigationItem.RightBarButtonItem.Enabled = false;

                    this.DoWork(() => {
                        var forkedGist = Application.Client.API.ForkGist(Id);
                        InvokeOnMainThread(delegate {
                            NavigationController.PushViewController(new GistInfoController(forkedGist.Data.Id), true);
                        });
                    }, null, () => {
                        NavigationItem.RightBarButtonItem.Enabled = true;
                    });
                }));
            }
        }

        private void UserButtonPress()
        {
            if (Model == null || Model.User == null)
                return;
            var user = Model.User.Login;
            NavigationController.PushViewController(new UserGistsController(user), true);
        }

        private void StarButtonPress()
        {
            if (Model == null)
                return;

            // Don't show the HUD because these are sooo quick that it ends up being a goofy flash of a screen
            if (_starred)
            {
                this.DoWorkNoHud(() => {
                    Application.Client.API.UnstarGist(Model.Id);
                    _starred = false;
                    UpdateStar();
                });
            }
            else
            {
                this.DoWorkNoHud(() => {
                    Application.Client.API.StarGist(Model.Id);
                    _starred = true;
                    UpdateStar();
                });
            }
        }

        private void UpdateStar()
        {
            InvokeOnMainThread(() => {
                _starButton.SetImage(_starred ? Images.Buttons.StarHighlighted : Images.Buttons.Star, UIControlState.Normal);
                _starButton.SetNeedsDisplay();
            });
        }

        private void CommentButtonPress()
        {
            var composer = new Composer();
            composer.NewComment(this, () => {
                composer.DoWork(() => {
                    var text = String.Empty;
                    InvokeOnMainThread(delegate { text = composer.Text;});
                    var comment = Application.Client.API.CreateGistComment(Model.Id, text);
                    if (_comments != null)
                    {
                        var temp = new List<GistCommentModel>();
                        temp.Add(comment.Data);
                        AddToCommentList(temp);
                    }
                    InvokeOnMainThread(delegate {
                        if (_tabButtons.Selected == 1)
                            OnRefresh();
                        composer.CloseComposer();
                    });
                }, null, () => {
                    composer.EnableSendButton = true;
                });
            });
        }

        private void ShareButtonPress()
        {
            var sheet = Utilities.GetSheet(String.Empty);
            var githubButton = sheet.AddButton("View on GitHub");
            sheet.CancelButtonIndex = sheet.AddButton("Close");
            sheet.Clicked += (object sender, UIButtonEventArgs e) => {
                if (e.ButtonIndex == githubButton)
                {
                    if (Model != null && Model.HtmlUrl != null)
                        UIApplication.SharedApplication.OpenUrl(new MonoTouch.Foundation.NSUrl(Model.HtmlUrl));
                }
            };

            sheet.ShowFrom(_shareButton, true);

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
                    Refresh();
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
                        Image = Images.Misc.Anonymous,
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

                    sse.Image = Images.Misc.Anonymous;
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
            });

            UpdateStar();
            UpdateOwned();
        }

        protected override GistModel OnUpdate(bool forced)
        {
            var selected = 0;
            InvokeOnMainThread(delegate { selected = _tabButtons.Selected; });

            //If we're currently looking at the comments then update it. If not, just invalidate it
            if (selected == 1)
            {
                if (_comments != null)
                    _comments.Clear();
                AddToCommentList(Application.Client.API.GetGistComments(Id).Data);
            }
            else
                _comments = null;

            //I've noticed that sometimes this randomly fails (especially when you just created a fork)
            //Don't bet the farm on this working...
            try
            {
                _starred = Application.Client.API.IsGistStarred(Id);
            }
            catch (Exception e)
            {
                Utilities.LogException("Unable to determine if gist was starred!", e);
            }

            return Application.Client.API.GetGist(Id).Data;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.DoWorkNoHud(() => {
                _starred = Application.Client.API.IsGistStarred(Id);
                UpdateStar();
            });
        }

        public override void ViewDidAppear(bool animated)
        {
            if (ToolbarItems != null)
                NavigationController.SetToolbarHidden(IsSearching, animated);
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (ToolbarItems != null)
                NavigationController.SetToolbarHidden(true, animated);
        }
    }
}

