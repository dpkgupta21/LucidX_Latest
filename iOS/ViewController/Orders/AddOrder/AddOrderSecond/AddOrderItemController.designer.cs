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
    [Register ("AddOrderItemController")]
    partial class AddOrderItemController
    {
        [Outlet]
        UIKit.UIToolbar AmountDoneBar { get; set; }

        [Outlet]
        UIKit.UIBarButtonItem AmountDoneBtn { get; set; }

        [Outlet]
        UIKit.UIButton BtnCancel { get; set; }

        [Outlet]
        UIKit.UIButton BtnClose { get; set; }

        [Outlet]
        UIKit.UIButton BtnDelete { get; set; }

        [Outlet]
        UIKit.UIButton BtnOk { get; set; }

        [Outlet]
        UIKit.UIBarButtonItem BtnRevenueDone { get; set; }

        [Outlet]
        UIKit.UIBarButtonItem BtnTaxDone { get; set; }

        [Outlet]
        UIKit.UILabel LblAmount { get; set; }

        [Outlet]
        UIKit.UILabel LblDescription { get; set; }

        [Outlet]
        UIKit.UILabel LblRevenue { get; set; }

        [Outlet]
        UIKit.UILabel LblTaxType { get; set; }

        [Outlet]
        UIKit.UILabel LblTitle { get; set; }

        [Outlet]
        UIKit.UILabel LblVat { get; set; }

        [Outlet]
        UIKit.UIToolbar RevenueDoneBar { get; set; }

        [Outlet]
        UIKit.UIPickerView RevenuePicker { get; set; }

        [Outlet]
        TPKeyboardAvoiding.TPKeyboardAvoidingScrollView ScrollVw { get; set; }

        [Outlet]
        UIKit.UIPickerView TaxtTypePicker { get; set; }

        [Outlet]
        UIKit.UIToolbar TaxTypeDoneBar { get; set; }

        [Outlet]
        UIKit.UITextField TxtAmount { get; set; }

        [Outlet]
        UIKit.UITextField TxtDescription { get; set; }

        [Outlet]
        UIKit.UITextField TxtRevenue { get; set; }

        [Outlet]
        UIKit.UITextField TxtTaxType { get; set; }

        [Outlet]
        UIKit.UITextField TxtVat { get; set; }

        [Action ("AmountDoneClicked:")]
        partial void AmountDoneClicked (Foundation.NSObject sender);

        [Action ("AmountEditingEnded:")]
        partial void AmountEditingEnded (Foundation.NSObject sender);

        [Action ("BtnCloseClicked:")]
        partial void BtnCloseClicked (Foundation.NSObject sender);

        [Action ("BtnDeleteClicked:")]
        partial void BtnDeleteClicked (Foundation.NSObject sender);

        [Action ("BtnOkClicked:")]
        partial void BtnOkClicked (Foundation.NSObject sender);

        [Action ("CancelClicked:")]
        partial void CancelClicked (Foundation.NSObject sender);

        [Action ("RevenueDoneClicked:")]
        partial void RevenueDoneClicked (Foundation.NSObject sender);

        [Action ("RevenueEditingEnded:")]
        partial void RevenueEditingEnded (Foundation.NSObject sender);

        [Action ("TaxDoneClicked:")]
        partial void TaxDoneClicked (Foundation.NSObject sender);

        [Action ("TaxTypeEditingEnded:")]
        partial void TaxTypeEditingEnded (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (AmountDoneBar != null) {
                AmountDoneBar.Dispose ();
                AmountDoneBar = null;
            }

            if (AmountDoneBtn != null) {
                AmountDoneBtn.Dispose ();
                AmountDoneBtn = null;
            }

            if (BtnCancel != null) {
                BtnCancel.Dispose ();
                BtnCancel = null;
            }

            if (BtnClose != null) {
                BtnClose.Dispose ();
                BtnClose = null;
            }

            if (BtnDelete != null) {
                BtnDelete.Dispose ();
                BtnDelete = null;
            }

            if (BtnOk != null) {
                BtnOk.Dispose ();
                BtnOk = null;
            }

            if (BtnRevenueDone != null) {
                BtnRevenueDone.Dispose ();
                BtnRevenueDone = null;
            }

            if (BtnTaxDone != null) {
                BtnTaxDone.Dispose ();
                BtnTaxDone = null;
            }

            if (LblAmount != null) {
                LblAmount.Dispose ();
                LblAmount = null;
            }

            if (LblDescription != null) {
                LblDescription.Dispose ();
                LblDescription = null;
            }

            if (LblRevenue != null) {
                LblRevenue.Dispose ();
                LblRevenue = null;
            }

            if (LblTaxType != null) {
                LblTaxType.Dispose ();
                LblTaxType = null;
            }

            if (LblTitle != null) {
                LblTitle.Dispose ();
                LblTitle = null;
            }

            if (LblVat != null) {
                LblVat.Dispose ();
                LblVat = null;
            }

            if (RevenueDoneBar != null) {
                RevenueDoneBar.Dispose ();
                RevenueDoneBar = null;
            }

            if (RevenuePicker != null) {
                RevenuePicker.Dispose ();
                RevenuePicker = null;
            }

            if (ScrollVw != null) {
                ScrollVw.Dispose ();
                ScrollVw = null;
            }

            if (TaxtTypePicker != null) {
                TaxtTypePicker.Dispose ();
                TaxtTypePicker = null;
            }

            if (TaxTypeDoneBar != null) {
                TaxTypeDoneBar.Dispose ();
                TaxTypeDoneBar = null;
            }

            if (TxtAmount != null) {
                TxtAmount.Dispose ();
                TxtAmount = null;
            }

            if (TxtDescription != null) {
                TxtDescription.Dispose ();
                TxtDescription = null;
            }

            if (TxtRevenue != null) {
                TxtRevenue.Dispose ();
                TxtRevenue = null;
            }

            if (TxtTaxType != null) {
                TxtTaxType.Dispose ();
                TxtTaxType = null;
            }

            if (TxtVat != null) {
                TxtVat.Dispose ();
                TxtVat = null;
            }
        }
    }
}