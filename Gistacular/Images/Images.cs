using System;
using MonoTouch.UIKit;

namespace Gistacular
{
	public static class Images
	{
		public static UIImage ThreeLines = UIImage.FromFile("Images/three_lines.png");
		public static UIImage Linen = UIImage.FromFile("Images/linen.png");
        public static UIImage Anonymous = UIImage.FromBundle("/Images/anonymous");
        public static UIImage GitHubLogo = UIImage.FromFile("Images/Logos/githublogo.png");

        public static UIImage MenuSectionBackground = UIImage.FromBundle("/Images/menu_section_bg");
        public static UIImage MenuNavbar = UIImage.FromBundle("/Images/menu_navbar");
        public static UIImage TopNavbar = UIImage.FromBundle("/Images/top_navbar");
	}
}

