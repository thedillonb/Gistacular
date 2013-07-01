using System;
using GitHubSharp.Models;
using System.Collections.Generic;
using MonoTouch.Dialog;
using Gistacular.Views;
using MonoTouch.UIKit;

namespace Gistacular.Controllers
{
    public class CreateGistController : BaseDialogViewController
    {
        public CreateGistController()
            : base(true)
        {
            Title = "Create Gist";
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(NavigationButton.Create(Images.AddButton, () => AddFile()));
        }

        private void AddFile()
        {
        }
    }
}

