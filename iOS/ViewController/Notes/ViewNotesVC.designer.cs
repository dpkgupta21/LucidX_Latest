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
    [Register ("ViewNotesVC")]
    partial class ViewNotesVC
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
        UIKit.UIToolbar IBDateTimeDoneBar { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem IBDateTimeDoneBtn { get; set; }


        [Outlet]
        UIKit.UIDatePicker IBDateTimePicker { get; set; }


        [Outlet]
        UIKit.UILabel IBEmptyLbl { get; set; }


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
        UIKit.UITextField IBFromDateTxt { get; set; }


        [Outlet]
        UIKit.UITableView IBNotesTbl { get; set; }


        [Outlet]
        UIKit.UIButton IBSearchBtn { get; set; }


        [Outlet]
        UIKit.UITextField IBToDateTxt { get; set; }


        [Outlet]
        UIKit.UILabel IBToLbl { get; set; }


        [Action ("IBAccountDoneClicked:")]
        partial void IBAccountDoneClicked (Foundation.NSObject sender);


        [Action ("IBDateTimeDoneClicked:")]
        partial void IBDateTimeDoneClicked (Foundation.NSObject sender);


        [Action ("IBEntityDoneClicked:")]
        partial void IBEntityDoneClicked (Foundation.NSObject sender);


        [Action ("IBSearchBtnClicked:")]
        partial void IBSearchBtnClicked (Foundation.NSObject sender);

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

            if (IBDateTimeDoneBar != null) {
                IBDateTimeDoneBar.Dispose ();
                IBDateTimeDoneBar = null;
            }

            if (IBDateTimeDoneBtn != null) {
                IBDateTimeDoneBtn.Dispose ();
                IBDateTimeDoneBtn = null;
            }

            if (IBDateTimePicker != null) {
                IBDateTimePicker.Dispose ();
                IBDateTimePicker = null;
            }

            if (IBEmptyLbl != null) {
                IBEmptyLbl.Dispose ();
                IBEmptyLbl = null;
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

            if (IBFromDateTxt != null) {
                IBFromDateTxt.Dispose ();
                IBFromDateTxt = null;
            }

            if (IBNotesTbl != null) {
                IBNotesTbl.Dispose ();
                IBNotesTbl = null;
            }

            if (IBSearchBtn != null) {
                IBSearchBtn.Dispose ();
                IBSearchBtn = null;
            }

            if (IBToDateTxt != null) {
                IBToDateTxt.Dispose ();
                IBToDateTxt = null;
            }

            if (IBToLbl != null) {
                IBToLbl.Dispose ();
                IBToLbl = null;
            }
        }
    }
}