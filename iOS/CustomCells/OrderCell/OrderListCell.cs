using System;

using Foundation;
using LucidX.ResponseModels;
using UIKit;

namespace LucidX.iOS.OrderCell
{
	public partial class OrderListCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("OrderListCell");
		public static readonly UINib Nib;

		static OrderListCell()
		{
			Nib = UINib.FromName("OrderListCell", NSBundle.MainBundle);
		}

		protected OrderListCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}


		public void configure(LedgerOrder data) {
			IBNameLbl.Text = data.TransactionReference;
			IBTitleLbl.Text = data.AccountName;
			IBCostValueLbl.Text = data.CompleteTotal.ToString();
			IBDateLbl.Text = data.TransDate;
		} 
	}
}
