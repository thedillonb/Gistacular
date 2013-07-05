using System;
using MonoTouch.UIKit;

namespace Gistacular
{
	public static class Images
	{
        public static class Buttons
        {
            public static UIImage View = UIImage.FromBundle("/Images/buttons/view");
            public static UIImage Edit = UIImage.FromBundle("/Images/buttons/edit");
            public static UIImage Fork = UIImage.FromBundle("/Images/buttons/fork");
            public static UIImage Back = UIImage.FromBundle("/Images/buttons/back");
            public static UIImage Add = UIImage.FromBundle("/Images/buttons/add");
            public static UIImage NewGist = UIImage.FromBundle("/Images/buttons/new_gist");
            public static UIImage Feedback = UIImage.FromBundle("/Images/buttons/feedback");
            public static UIImage Info = UIImage.FromBundle("/Images/buttons/info");
            public static UIImage Logout = UIImage.FromBundle("/Images/buttons/logout");
            public static UIImage Star = UIImage.FromBundle("/Images/buttons/star");
            public static UIImage StarHighlighted = UIImage.FromBundle("/Images/buttons/star_highlighted");
            public static UIImage Comment = UIImage.FromBundle("/Images/buttons/comment");
            public static UIImage Share = UIImage.FromBundle("/Images/buttons/share");
            public static UIImage User = UIImage.FromBundle("/Images/buttons/user");
            public static UIImage Star2 = UIImage.FromBundle("/Images/buttons/star2");
            public static UIImage MyGists = UIImage.FromBundle("/Images/buttons/my_gists");
            public static UIImage Public = UIImage.FromBundle("/Images/buttons/public");
            public static UIImage ThreeLines = UIImage.FromFile("Images/buttons/three_lines.png");
            public static UIImage Save = UIImage.FromBundle("/Images/buttons/save");
            public static UIImage Cancel = UIImage.FromBundle("/Images/buttons/cancel");
        }

        public static class Components
        {
            public static UIImage Arrow = UIImage.FromBundle("/Images/components/arrow");
            public static UIImage Searchbar = UIImage.FromBundle("/Images/components/search_bg");
            public static UIImage MenuNavbar = UIImage.FromBundle("/Images/components/menu_navbar");
            public static UIImage MenuSectionBackground = UIImage.FromBundle("/Images/components/menu_section_bg");
            public static UIImage TabsBackground = UIImage.FromBundle("/Images/components/tabs_bg");
            public static UIImage TabsHighlighted = UIImage.FromBundle("/Images/components/tabs_highlighted");
            public static UIImage TabsVertical = UIImage.FromBundle("/Images/components/tabs_vertical");
            public static UIImage Toolbar = UIImage.FromBundle("/Images/components/toolbar");
            public static UIImage TopNavbar = UIImage.FromBundle("/Images/components/top_navbar");
        }

        public static class Misc
        {
            public static UIImage Anonymous = UIImage.FromBundle("/Images/misc/anonymous");
            public static UIImage GitHubLogo = UIImage.FromFile("Images/misc/githublogo.png");
        }
	}
}

