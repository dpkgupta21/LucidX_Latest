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
    [Register ("AddOrderThird")]
    partial class AddOrderThird
    {
        [Outlet]
        UIKit.UIButton BtnCancel { get; set; }

        [Outlet]
        UIKit.UIButton BtnSave { get; set; }

        [Outlet]
        UIKit.UILabel LblGross { get; set; }

        [Outlet]
        UIKit.UILabel LblNet { get; set; }

        [Outlet]
        UIKit.UILabel LblVat { get; set; }

        [Outlet]
        UIKit.UITextField TxtGross { get; set; }

        [Outlet]
        UIKit.UITextField TxtNet { get; set; }

        [Outlet]
        UIKit.UITextField txtVat { get; set; }

        [Action ("BtnCancelClicked:")]
        partial void BtnCancelClicked (Foundation.NSObject sender);

        [Action ("BtnSaveClicked:")]
        partial void BtnSaveClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (BtnCancel != null) {
                BtnCancel.Dispose ();
                BtnCancel = null;
            }

            if (BtnSave != null) {
                BtnSave.Dispose ();
                BtnSave = null;
            }

            if (LblGross != null) {
                LblGross.Dispose ();
                LblGross = null;
            }

            if (LblNet != null) {
                LblNet.Dispose ();
                LblNet = null;
            }

            if (LblVat != null) {
                LblVat.Dispose ();
                LblVat = null;
            }

            if (TxtGross != null) {
                TxtGross.Dispose ();
                TxtGross = null;
            }

            if (TxtNet != null) {
                TxtNet.Dispose ();
                TxtNet = null;
            }

            if (txtVat != null) {
                txtVat.Dispose ();
                txtVat = null;
            }
        }
    }
}