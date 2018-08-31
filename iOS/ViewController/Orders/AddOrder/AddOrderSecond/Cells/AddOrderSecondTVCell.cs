// ***********************************************************************
// Assembly			: ComOps.IOS
// Author				: Akash Deep Kaushik
// CreatedAt			: 8/28/2017
//
// ***********************************************************************
// <copyright file="AddOrderSecondTVCell.cs" company="Pratham Software ">
//     Pratham Software Pvt. Ltd. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

using Foundation;
using LucidX.ResponseModels;
using UIKit;

namespace LucidX.iOS
{
    public partial class AddOrderSecondTVCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("AddOrderSecondTVCell");
        public static readonly UINib Nib;
		int Index;
		public EventHandler EditClicked;
		public EventHandler DeleteClicked;
		

        static AddOrderSecondTVCell()
        {
            Nib = UINib.FromName("AddOrderSecondTVCell", NSBundle.MainBundle);
        }

        protected AddOrderSecondTVCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		public void configure(LedgerOrderItem data,int index)
		{
			Index = index;
			LblAccountNameValue.Text = data.AccountName;
			LblTaxValue.Text = data.TaxAmount.ToString();
			LblItemTitle.Text = data.LineDescription;
			LblBaseAmountValue.Text = data.BaseAmount.ToString();
		}

		partial void BtndeleteClicked(Foundation.NSObject sender) { 
			if (DeleteClicked != null)
			{
				DeleteClicked(Index, null);
			}
		}

		partial void BtnEditClicked(Foundation.NSObject sender) {
			if (EditClicked != null) {
				EditClicked(Index, null);
			}
		}

	}
}
