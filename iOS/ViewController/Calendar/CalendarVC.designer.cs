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
    [Register ("CalendarVC")]
    partial class CalendarVC
    {
        [Outlet]
        UIKit.UIToolbar IBAssignedDoneBar { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem IBAssignedDoneBtn { get; set; }


        [Outlet]
        UIKit.UIPickerView IBAssignedToPicker { get; set; }


        [Outlet]
        UIKit.UITextField IBAssignedToTxt { get; set; }


        [Outlet]
        UIKit.UITextField IBCalendarTypeTxt { get; set; }


        [Outlet]
        UIKit.UITableView IBContntTbl { get; set; }


        [Outlet]
        UIKit.UIToolbar IBDateTimeDoneBar { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem IBDateTimeDoneBttn { get; set; }


        [Outlet]
        UIKit.UIDatePicker IBDateTimePicker { get; set; }


        [Outlet]
        UIKit.UILabel IBEmptyLbl { get; set; }


        [Outlet]
        UIKit.UITextField IBFromDateTxt { get; set; }


        [Outlet]
        UIKit.UIButton IBSearchBtn { get; set; }


        [Outlet]
        UIKit.UITextField IBToDateTxt { get; set; }


        [Outlet]
        UIKit.UILabel IBToLbl { get; set; }


        [Action ("IBAssignedDoneClicked:")]
        partial void IBAssignedDoneClicked (Foundation.NSObject sender);


        [Action ("IBDateTimeDoneClicked:")]
        partial void IBDateTimeDoneClicked (Foundation.NSObject sender);


        [Action ("SearchClicked:")]
        partial void SearchClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (IBAssignedDoneBar != null) {
                IBAssignedDoneBar.Dispose ();
                IBAssignedDoneBar = null;
            }

            if (IBAssignedDoneBtn != null) {
                IBAssignedDoneBtn.Dispose ();
                IBAssignedDoneBtn = null;
            }

            if (IBAssignedToPicker != null) {
                IBAssignedToPicker.Dispose ();
                IBAssignedToPicker = null;
            }

            if (IBAssignedToTxt != null) {
                IBAssignedToTxt.Dispose ();
                IBAssignedToTxt = null;
            }

            if (IBCalendarTypeTxt != null) {
                IBCalendarTypeTxt.Dispose ();
                IBCalendarTypeTxt = null;
            }

            if (IBContntTbl != null) {
                IBContntTbl.Dispose ();
                IBContntTbl = null;
            }

            if (IBDateTimeDoneBar != null) {
                IBDateTimeDoneBar.Dispose ();
                IBDateTimeDoneBar = null;
            }

            if (IBDateTimeDoneBttn != null) {
                IBDateTimeDoneBttn.Dispose ();
                IBDateTimeDoneBttn = null;
            }

            if (IBDateTimePicker != null) {
                IBDateTimePicker.Dispose ();
                IBDateTimePicker = null;
            }

            if (IBEmptyLbl != null) {
                IBEmptyLbl.Dispose ();
                IBEmptyLbl = null;
            }

            if (IBFromDateTxt != null) {
                IBFromDateTxt.Dispose ();
                IBFromDateTxt = null;
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