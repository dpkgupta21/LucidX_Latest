// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LucidX.iOS
{
    [Register ("AddOrderSecondTVCell")]
    partial class AddOrderSecondTVCell
    {
        [Outlet]
        UIKit.UIButton BtnDelete { get; set; }


        [Outlet]
        UIKit.UIButton BtnEdit { get; set; }


        [Outlet]
        UIKit.UILabel LblAccountNameTilte { get; set; }


        [Outlet]
        UIKit.UILabel LblAccountNameValue { get; set; }


        [Outlet]
        UIKit.UILabel LblBaseAmountTitle { get; set; }


        [Outlet]
        UIKit.UILabel LblBaseAmountValue { get; set; }


        [Outlet]
        UIKit.UILabel LblItemTitle { get; set; }


        [Outlet]
        UIKit.UILabel LblTaxTitle { get; set; }


        [Outlet]
        UIKit.UILabel LblTaxValue { get; set; }


        [Action ("BtndeleteClicked:")]
        partial void BtndeleteClicked (Foundation.NSObject sender);


        [Action ("BtnEditClicked:")]
        partial void BtnEditClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (BtnDelete != null) {
                BtnDelete.Dispose ();
                BtnDelete = null;
            }

            if (BtnEdit != null) {
                BtnEdit.Dispose ();
                BtnEdit = null;
            }

            if (LblAccountNameTilte != null) {
                LblAccountNameTilte.Dispose ();
                LblAccountNameTilte = null;
            }

            if (LblAccountNameValue != null) {
                LblAccountNameValue.Dispose ();
                LblAccountNameValue = null;
            }

            if (LblBaseAmountTitle != null) {
                LblBaseAmountTitle.Dispose ();
                LblBaseAmountTitle = null;
            }

            if (LblBaseAmountValue != null) {
                LblBaseAmountValue.Dispose ();
                LblBaseAmountValue = null;
            }

            if (LblItemTitle != null) {
                LblItemTitle.Dispose ();
                LblItemTitle = null;
            }

            if (LblTaxTitle != null) {
                LblTaxTitle.Dispose ();
                LblTaxTitle = null;
            }

            if (LblTaxValue != null) {
                LblTaxValue.Dispose ();
                LblTaxValue = null;
            }
        }
    }
}