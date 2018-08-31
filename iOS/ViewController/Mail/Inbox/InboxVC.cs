using System;
using UIKit;
using Plugin.Connectivity;
using Xamarin.SWRevealViewController;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Foundation;
using CustomCells;
using IosUtils;
using LucidX.ResponseModels;
using System.Collections.Generic;
using LucidX.Webservices;

namespace Inbox
{
	public partial class InboxVC : UIViewController, IUITableViewDelegate, IUITableViewDataSource, IUISearchBarDelegate
	{
		int mailTypeId;
		public int MailTypeId
		{
			get
			{
				return mailTypeId;
			}
			set
			{
				mailTypeId = value;
				GetData();
			}

		}

		public SWRevealViewController revealVC;
		List<EmailResponse> mails = new List<EmailResponse>();

		public InboxVC() : base("InboxVC", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigureView();
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
			var createBtn = new UIBarButtonItem(UIImage.FromBundle("Edit"),
											  UIBarButtonItemStyle.Plain,
												EditClicked);
			var refreshBtn = new UIBarButtonItem(UIImage.FromBundle("Refresh"),
											  UIBarButtonItemStyle.Plain,
												 RefreshClicked);
			this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[] { createBtn, refreshBtn };
			this.IBCancelWidth.Constant = 0;
			IBContntTbl.RegisterNibForCellReuse(MailDetailsCell.Nib, MailDetailsCell.Key);
			IBContntTbl.TableFooterView = new UIView();
			this.NavigationItem.Title = "Inbox";
			this.NavigationItem.BackBarButtonItem = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, null, null);
		}

		async void GetData()
		{
			if (IosUtils.IosUtility.IsReachable())
			{
				IosUtils.IosUtility.showProgressHud("");
				var res = await WebServiceMethods.InboxEmails(IosUtils.Settings.UserId, mailTypeId);
				if (res != null)
				{
					mails = res;
				}
				else
				{

				}
				IBContntTbl.ReloadData();
				IosUtils.IosUtility.hideProgressHud();
			}
		}

		async void markRead(string mailId)
		{
			await WebServiceMethods.MarkReadEmail(mailId);
		}

		#endregion


		#region SearchBar Delegate Methods

		[Export("searchBarShouldBeginEditing:")]
		public bool ShouldBeginEditing(UISearchBar searchBar)
		{
			UIView.Animate(0.2f, () =>
			{
				this.IBCancelWidth.Constant = 60;
				this.View.LayoutIfNeeded();
			});
			return true;
		}

		#endregion
		#region IBAction Methods

		void MenuClicked(object sender, EventArgs e)
		{
			revealVC.RevealToggleAnimated(true);
		}

		void RefreshClicked(object sender, EventArgs e)
		{
			GetData();
		}

		void EditClicked(object sender, EventArgs e)
		{

		}

		partial void IBCancelClicked(Foundation.NSObject sender)
		{
			IBSearchBar.EndEditing(true);
			UIView.Animate(0.2f, () =>
			{
				this.IBCancelWidth.Constant = 0;
				this.View.LayoutIfNeeded();
			});
		}

		#endregion

		#region Tableview delegate and database methods

		[Export("numberOfSectionsInTableView:")]
		public nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public nint RowsInSection(UITableView tableView, nint section)
		{
			return mails.Count;
		}

		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(MailDetailsCell.Key) as MailDetailsCell;
			cell.ConfigureView(mails[indexPath.Row]);
			return cell;
		}
		[Export("tableView:heightForRowAtIndexPath:")]
		public nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return HeightConstants.CellHeight70;
		}

		[Export("tableView:didSelectRowAtIndexPath:")]
		public void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if (IosUtils.IosUtility.IsReachable())
			{
				mails[indexPath.Row].Unread = false;
				markRead(mails[indexPath.Row].MailId);
				IBContntTbl.ReloadData();
				var detailsVC = new MailDetails.MailDetailsVC();
				detailsVC.mail = mails[indexPath.Row];
				this.NavigationController.PushViewController(detailsVC, true);
			}
		}


		#endregion

	}
}

