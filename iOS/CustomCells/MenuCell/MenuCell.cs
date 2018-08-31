using System;

using Foundation;
using UIKit;

namespace LucidX.iOS.CustomCells
{
	public partial class MenuCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("MenuCell");
		public static readonly UINib Nib = UINib.FromName("MenuCell", NSBundle.MainBundle);

		protected MenuCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureCell(string title, UIImage img, int count, bool selected)
		{

			IBTitleLbl.Text = title;
			IBIconCell.Image = img;
			IBCountLbl.Text = count != 0 ? count.ToString() : "";
			IBBackVw.BackgroundColor = selected ? IosUtils.IosColorConstant.ThemeNavBlue : IosUtils.IosColorConstant.ThemeDarkBlue;
		}


	}
}
