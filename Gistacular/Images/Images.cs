using System;
using MonoTouch.UIKit;

namespace Gistacular
{
	public static class Images
	{
        public static UIImage ThreeLines = UIImage.FromFile("Images/three_lines.png");
        public static UIImage Anonymous = UIImage.FromBundle("/Images/anonymous");
        public static UIImage GitHubLogo = UIImage.FromFile("Images/githublogo.png");

        public static UIImage MenuSectionBackground = UIImage.FromBundle("/Images/menu_section_bg");
        public static UIImage MenuNavbar = UIImage.FromBundle("/Images/menu_navbar");
        public static UIImage TopNavbar = UIImage.FromBundle("/Images/top_navbar");
        public static UIImage Searchbar = UIImage.FromBundle("/Images/search_bg");

        public static UIImage Arrow = UIImage.FromBundle("/Images/arrow");
        public static UIImage BackButton = UIImage.FromBundle("/Images/back-button");
        public static UIImage AddButton = UIImage.FromBundle("/Images/add-button");

        public static UIImage TabsBackground = UIImage.FromBundle("/Images/tabs_bg");
        public static UIImage TabsHighlighted = UIImage.FromBundle("/Images/tabs_highlighted");
        public static UIImage TabsVertical = UIImage.FromBundle("/Images/tabs_vertical");
	}
}

