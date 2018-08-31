// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LucidX.iOS.Notes
{
    [Register ("CreateNotesVC")]
    partial class CreateNotesVC
    {
        [Outlet]
        UIKit.UIToolbar IBAccountCodeDoneBar { get; set; }


        [Outlet]
        UIKit.UIPickerView IBAccountCodePicker { get; set; }


        [Outlet]
        UIKit.UILabel IBAccountCodeTitleLbl { get; set; }


        [Outlet]
        UIKit.UITextField IBAccountCodeTxt { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem IBaccountDoneBtn { get; set; }


        [Outlet]
        UIKit.UIButton IBCancelBtn { get; set; }


        [Outlet]
        UIKit.UILabel IBEntityCodeTiltleLbl { get; set; }


        [Outlet]
        UIKit.UIToolbar IBEntityDoneBar { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem IBEntityDoneBtn { get; set; }


        [Outlet]
        UIKit.UIPickerView IBEntityPicker { get; set; }


        [Outlet]
        UIKit.UITextField IBEntityTxt { get; set; }


        [Outlet]
        UIKit.UIButton IBMeBtn { get; set; }


        [Outlet]
        UIKit.UILabel IBNotesDetailsLbl { get; set; }


        [Outlet]
        UIKit.UITextView IBNotesDetailsTxt { get; set; }


        [Outlet]
        UIKit.UILabel IBNotesHeaderLbl { get; set; }


        [Outlet]
        UIKit.UITextField IBNotesHeaderTxt { get; set; }


        [Outlet]
        UIKit.UIButton IBPubicBtn { get; set; }


        [Outlet]
        UIKit.UIButton IBSaveBtn { get; set; }


        [Outlet]
        UIKit.UIButton IBSelectedBtn { get; set; }


        [Outlet]
        TPKeyboardAvoiding.TPKeyboardAvoidingScrollView ScrollVw { get; set; }


        [Action ("BtnCancelClicked:")]
        partial void BtnCancelClicked (Foundation.NSObject sender);


        [Action ("BtnSaveClicked:")]
        partial void BtnSaveClicked (Foundation.NSObject sender);


        [Action ("IBAccountDoneClicked:")]
        partial void IBAccountDoneClicked (Foundation.NSObject sender);


        [Action ("IBEntityDoneClicked:")]
        partial void IBEntityDoneClicked (Foundation.NSObject sender);


        [Action ("IBRadioBtnClicked:")]
        partial void IBRadioBtnClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (IBAccountCodeDoneBar != null) {
                IBAccountCodeDoneBar.Dispose ();
                IBAccountCodeDoneBar = null;
            }

            if (IBAccountCodePicker != null) {
                IBAccountCodePicker.Dispose ();
                IBAccountCodePicker = null;
            }

            if (IBAccountCodeTitleLbl != null) {
                IBAccountCodeTitleLbl.Dispose ();
                IBAccountCodeTitleLbl = null;
            }

            if (IBAccountCodeTxt != null) {
                IBAccountCodeTxt.Dispose ();
                IBAccountCodeTxt = null;
            }

            if (IBaccountDoneBtn != null) {
                IBaccountDoneBtn.Dispose ();
                IBaccountDoneBtn = null;
            }

            if (IBCancelBtn != null) {
                IBCancelBtn.Dispose ();
                IBCancelBtn = null;
            }

            if (IBEntityCodeTiltleLbl != null) {
                IBEntityCodeTiltleLbl.Dispose ();
                IBEntityCodeTiltleLbl = null;
            }

            if (IBEntityDoneBar != null) {
                IBEntityDoneBar.Dispose ();
                IBEntityDoneBar = null;
            }

            if (IBEntityDoneBtn != null) {
                IBEntityDoneBtn.Dispose ();
                IBEntityDoneBtn = null;
            }

            if (IBEntityPicker != null) {
                IBEntityPicker.Dispose ();
                IBEntityPicker = null;
            }

            if (IBEntityTxt != null) {
                IBEntityTxt.Dispose ();
                IBEntityTxt = null;
            }

            if (IBMeBtn != null) {
                IBMeBtn.Dispose ();
                IBMeBtn = null;
            }

            if (IBNotesDetailsLbl != null) {
                IBNotesDetailsLbl.Dispose ();
                IBNotesDetailsLbl = null;
            }

            if (IBNotesDetailsTxt != null) {
                IBNotesDetailsTxt.Dispose ();
                IBNotesDetailsTxt = null;
            }

            if (IBNotesHeaderLbl != null) {
                IBNotesHeaderLbl.Dispose ();
                IBNotesHeaderLbl = null;
            }

            if (IBNotesHeaderTxt != null) {
                IBNotesHeaderTxt.Dispose ();
                IBNotesHeaderTxt = null;
            }

            if (IBPubicBtn != null) {
                IBPubicBtn.Dispose ();
                IBPubicBtn = null;
            }

            if (IBSaveBtn != null) {
                IBSaveBtn.Dispose ();
                IBSaveBtn = null;
            }

            if (IBSelectedBtn != null) {
                IBSelectedBtn.Dispose ();
                IBSelectedBtn = null;
            }

            if (ScrollVw != null) {
                ScrollVw.Dispose ();
                ScrollVw = null;
            }
        }
    }
}