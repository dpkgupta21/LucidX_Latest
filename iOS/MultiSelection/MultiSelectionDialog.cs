using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using LucidX.ResponseModels;

namespace LucidX.iOS
{
	public partial class MultiSelectionDialog : UIView, IUITableViewDelegate, IUITableViewDataSource
	{
		public MultiSelectionDialog(IntPtr handle) : base(handle)
		{
		}
		List<NotesTypeResponse> Data;

		public event EventHandler<List<NotesTypeResponse>> SaveClicked;

		public static readonly UINib Nib = UINib.FromName("MultiSelectionDialog", NSBundle.MainBundle);
		public static MultiSelectionDialog Create()
		{

			return Nib.Instantiate(null, null)[0] as MultiSelectionDialog;
		}

		public void Configure(List<NotesTypeResponse> types,String Title)
		{
			IosUtils.IosUtility.setcornerRadius(this.ContntVw);
			IosUtils.IosUtility.setcornerRadius(this.IBSaveBtn);
			IosUtils.IosUtility.setcornerRadius(this.IBCancelBtn);

			IBTtitleLbl.Text = Title;
			Data = types;
			IBContntTable.ReloadData();
			this.Hidden = true;
			Frame = AppDelegate.GetMainWindow().Frame;
			AppDelegate.GetMainWindow().AddSubview(this);
			UIView.Animate(0.5, () =>
			{
				this.Hidden = false;
			});
		}

		void Hide()
		{
			UIView.Animate(0.5, () =>
				{
					this.RemoveFromSuperview();
				});
		}

		partial void IBCancelClicked(Foundation.NSObject sender)
		{
			Hide();
		}


		partial void IBSaveClicked(Foundation.NSObject sender)
		{
			if (SaveClicked != null)
			{
				SaveClicked(this, Data);
			}
			Hide();
		}


		#region Tableview delegate and database methods

		[Export("numberOfSectionsInTableView:")]
		public nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public nint RowsInSection(UITableView tableView, nint section)
		{
			return Data == null ? 0 : Data.Count;
		}

		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = new UITableViewCell(UITableViewCellStyle.Default, "Cell");
			cell.TextLabel.Text = Data[indexPath.Row].NotesTypeName;
			if (Data[indexPath.Row].IsSelected)
			{
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			}
			else
			{
				cell.Accessory = UITableViewCellAccessory.None;
			}
			return cell;
		}
		[Export("tableView:heightForRowAtIndexPath:")]
		public nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return UITableView.AutomaticDimension;
		}

		[Export("tableView:didSelectRowAtIndexPath:")]
		public void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			Data[indexPath.Row].IsSelected = !Data[indexPath.Row].IsSelected;
			tableView.ReloadData();
		}


		#endregion

	}
}