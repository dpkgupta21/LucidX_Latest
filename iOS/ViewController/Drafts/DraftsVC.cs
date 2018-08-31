using System;

using UIKit;
using Xamarin.SWRevealViewController;

namespace Drafts
{
	public partial class DraftsVC : UIViewController
	{
		public DraftsVC() : base("DraftsVC", null)
		{
		}

		public SWRevealViewController revealVC;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigureView();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
#region Helping Methods
		/// <summary>
		/// Configures the view.
		/// </summary>
		void ConfigureView()
		{
			this.EdgesForExtendedLayout = UIRectEdge.None;
			this.NavigationItem.Title = "";
			var menuBtn = new UIBarButtonItem(UIImage.FromBundle("Menu"),
											  UIBarButtonItemStyle.Plain,
											  MenuClicked);
			this.NavigationItem.LeftBarButtonItem = menuBtn;
		}

#endregion

		#region IBAction Methods

		void MenuClicked(object sender, EventArgs e)
		{
			revealVC.RevealToggleAnimated(true);
		}
		#endregion
	}
}

