using Foundation;
using System;
using UIKit;

namespace LucidX.iOS
{
	public partial class MenuHeader : UIView
	{

		public EventHandler<bool> Clicked;

		bool IsSelected;

		public MenuHeader(IntPtr handle) : base(handle)
		{
		}

		public static readonly UINib Nib = UINib.FromName("MenuHeader", NSBundle.MainBundle);

		public static MenuHeader Create() {
			return Nib.Instantiate(null, null)[0] as MenuHeader;
		}


		public void Configure(string Title, int section, bool expnaded = false, bool Hide = false) {
			IBTitleLbl.Text = Title;
			this.Tag = section;
			IsSelected = expnaded;
			IBDropImg.Highlighted = expnaded;
			IBDropImg.Hidden = Hide;
		}

		partial void IBHeaderClicked(Foundation.NSObject sender)
		{
			IsSelected = !IsSelected;
			IBDropImg.Highlighted = IsSelected;
			if (Clicked != null)
			{
				Clicked(this, IsSelected);
			}

		}
	}
}