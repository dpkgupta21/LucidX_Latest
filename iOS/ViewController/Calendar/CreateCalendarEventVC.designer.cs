// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LucidX.iOS.Calendar
{
    [Register ("CreateCalendarEventVC")]
    partial class CreateCalendarEventVC
    {
        [Outlet]
        UIKit.UIToolbar IBAssignedToDoneBar { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem IBAssignedToDoneBtn { get; set; }


        [Outlet]
        UIKit.UIPickerView IBAssignedToPicker { get; set; }


        [Outlet]
        UIKit.UITextField IBAssignedToTxt { get; set; }


        [Outlet]
        UIKit.UIButton IBCancelBtn { get; set; }


        [Outlet]
        UIKit.UIToolbar IBDateTimeDoneBar { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem IBDateTimeDoneBtn { get; set; }


        [Outlet]
        UIKit.UIDatePicker IBDateTimePicker { get; set; }


        [Outlet]
        UIKit.UILabel IBDetailsLbl { get; set; }


        [Outlet]
        UIKit.UITextView IBDetailsTxt { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem IBEventTypeBtn { get; set; }


        [Outlet]
        UIKit.UIToolbar IBEventTypeDoneBar { get; set; }


        [Outlet]
        UIKit.UIPickerView IBEventTypePicker { get; set; }


        [Outlet]
        UIKit.UITextField IBEventTypeTxt { get; set; }


        [Outlet]
        UIKit.UILabel IBFromDateTimeLbl { get; set; }


        [Outlet]
        UIKit.UITextField IBFromDateTxt { get; set; }


        [Outlet]
        UIKit.UIButton IBSaveBtn { get; set; }


        [Outlet]
        UIKit.UILabel IBSubjectLbl { get; set; }


        [Outlet]
        UIKit.UITextField IBSubjectTxt { get; set; }


        [Outlet]
        UIKit.UILabel IBToDateTimeLbl { get; set; }


        [Outlet]
        UIKit.UITextField IBToDateTxt { get; set; }


        [Action ("BtnCancelClicked:")]
        partial void BtnCancelClicked (Foundation.NSObject sender);


        [Action ("BtnSaveClicked:")]
        partial void BtnSaveClicked (Foundation.NSObject sender);


        [Action ("IBAssignedToDoneClicked:")]
        partial void IBAssignedToDoneClicked (Foundation.NSObject sender);


        [Action ("IBDateTimeDoneClicked:")]
        partial void IBDateTimeDoneClicked (Foundation.NSObject sender);


        [Action ("IBEventTypeDoneClicked:")]
        partial void IBEventTypeDoneClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (IBAssignedToDoneBar != null) {
                IBAssignedToDoneBar.Dispose ();
                IBAssignedToDoneBar = null;
            }

            if (IBAssignedToDoneBtn != null) {
                IBAssignedToDoneBtn.Dispose ();
                IBAssignedToDoneBtn = null;
            }

            if (IBAssignedToPicker != null) {
                IBAssignedToPicker.Dispose ();
                IBAssignedToPicker = null;
            }

            if (IBAssignedToTxt != null) {
                IBAssignedToTxt.Dispose ();
                IBAssignedToTxt = null;
            }

            if (IBCancelBtn != null) {
                IBCancelBtn.Dispose ();
                IBCancelBtn = null;
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

            if (IBDetailsLbl != null) {
                IBDetailsLbl.Dispose ();
                IBDetailsLbl = null;
            }

            if (IBDetailsTxt != null) {
                IBDetailsTxt.Dispose ();
                IBDetailsTxt = null;
            }

            if (IBEventTypeBtn != null) {
                IBEventTypeBtn.Dispose ();
                IBEventTypeBtn = null;
            }

            if (IBEventTypeDoneBar != null) {
                IBEventTypeDoneBar.Dispose ();
                IBEventTypeDoneBar = null;
            }

            if (IBEventTypePicker != null) {
                IBEventTypePicker.Dispose ();
                IBEventTypePicker = null;
            }

            if (IBEventTypeTxt != null) {
                IBEventTypeTxt.Dispose ();
                IBEventTypeTxt = null;
            }

            if (IBFromDateTimeLbl != null) {
                IBFromDateTimeLbl.Dispose ();
                IBFromDateTimeLbl = null;
            }

            if (IBFromDateTxt != null) {
                IBFromDateTxt.Dispose ();
                IBFromDateTxt = null;
            }

            if (IBSaveBtn != null) {
                IBSaveBtn.Dispose ();
                IBSaveBtn = null;
            }

            if (IBSubjectLbl != null) {
                IBSubjectLbl.Dispose ();
                IBSubjectLbl = null;
            }

            if (IBSubjectTxt != null) {
                IBSubjectTxt.Dispose ();
                IBSubjectTxt = null;
            }

            if (IBToDateTimeLbl != null) {
                IBToDateTimeLbl.Dispose ();
                IBToDateTimeLbl = null;
            }

            if (IBToDateTxt != null) {
                IBToDateTxt.Dispose ();
                IBToDateTxt = null;
            }
        }
    }
}