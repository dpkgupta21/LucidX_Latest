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
    [Register ("AddOrderFirstVC")]
    partial class AddOrderFirstVC
    {
        [Outlet]
        UIKit.UIButton BtnNext { get; set; }


        [Outlet]
        UIKit.UIToolbar IBAccountDoneBar { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem IBAccountDoneBtn { get; set; }


        [Outlet]
        UIKit.UIPickerView IBAccountPicker { get; set; }


        [Outlet]
        UIKit.UIToolbar IBDateDoneBar { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem IBDateDoneBtn { get; set; }


        [Outlet]
        UIKit.UIDatePicker IBDatePicker { get; set; }


        [Outlet]
        UIKit.UILabel LblAddresTitle { get; set; }


        [Outlet]
        UIKit.UILabel LblCurrencyTitle { get; set; }


        [Outlet]
        UIKit.UILabel LblDateTitle { get; set; }


        [Outlet]
        UIKit.UILabel LblOrderNameTitle { get; set; }


        [Outlet]
        UIKit.UILabel LblSelectAcoountTilte { get; set; }


        [Outlet]
        TPKeyboardAvoiding.TPKeyboardAvoidingScrollView Scrollvw { get; set; }


        [Outlet]
        UIKit.UITextView TxtAddress { get; set; }


        [Outlet]
        UIKit.UITextField TxtCurrency { get; set; }


        [Outlet]
        UIKit.UITextField TxtOrderDate { get; set; }


        [Outlet]
        UIKit.UITextField TxtOrderName { get; set; }


        [Outlet]
        UIKit.UITextField TxtSelectAcoountTilte { get; set; }


        [Action ("BtnNextClicked:")]
        partial void BtnNextClicked (Foundation.NSObject sender);


        [Action ("DatePickerValueChanged:")]
        partial void DatePickerValueChanged (Foundation.NSObject sender);


        [Action ("IBDateDoneClicked:")]
        partial void IBDateDoneClicked (Foundation.NSObject sender);


        [Action ("IBDoneClicked:")]
        partial void IBDoneClicked (Foundation.NSObject sender);


        [Action ("TxtAcountEditingEnded:")]
        partial void TxtAcountEditingEnded (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (BtnNext != null) {
                BtnNext.Dispose ();
                BtnNext = null;
            }

            if (IBAccountDoneBar != null) {
                IBAccountDoneBar.Dispose ();
                IBAccountDoneBar = null;
            }

            if (IBAccountDoneBtn != null) {
                IBAccountDoneBtn.Dispose ();
                IBAccountDoneBtn = null;
            }

            if (IBAccountPicker != null) {
                IBAccountPicker.Dispose ();
                IBAccountPicker = null;
            }

            if (IBDateDoneBar != null) {
                IBDateDoneBar.Dispose ();
                IBDateDoneBar = null;
            }

            if (IBDateDoneBtn != null) {
                IBDateDoneBtn.Dispose ();
                IBDateDoneBtn = null;
            }

            if (IBDatePicker != null) {
                IBDatePicker.Dispose ();
                IBDatePicker = null;
            }

            if (LblAddresTitle != null) {
                LblAddresTitle.Dispose ();
                LblAddresTitle = null;
            }

            if (LblCurrencyTitle != null) {
                LblCurrencyTitle.Dispose ();
                LblCurrencyTitle = null;
            }

            if (LblDateTitle != null) {
                LblDateTitle.Dispose ();
                LblDateTitle = null;
            }

            if (LblOrderNameTitle != null) {
                LblOrderNameTitle.Dispose ();
                LblOrderNameTitle = null;
            }

            if (LblSelectAcoountTilte != null) {
                LblSelectAcoountTilte.Dispose ();
                LblSelectAcoountTilte = null;
            }

            if (Scrollvw != null) {
                Scrollvw.Dispose ();
                Scrollvw = null;
            }

            if (TxtAddress != null) {
                TxtAddress.Dispose ();
                TxtAddress = null;
            }

            if (TxtCurrency != null) {
                TxtCurrency.Dispose ();
                TxtCurrency = null;
            }

            if (TxtOrderDate != null) {
                TxtOrderDate.Dispose ();
                TxtOrderDate = null;
            }

            if (TxtOrderName != null) {
                TxtOrderName.Dispose ();
                TxtOrderName = null;
            }

            if (TxtSelectAcoountTilte != null) {
                TxtSelectAcoountTilte.Dispose ();
                TxtSelectAcoountTilte = null;
            }
        }
    }
}