// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LucidX.iOS.Orders
{
    [Register ("OrderListVC")]
    partial class OrderListVC
    {
        [Outlet]
        UIKit.UITableView IBContntTbl { get; set; }


        [Outlet]
        UIKit.UIToolbar IBDateDoneBar { get; set; }


        [Outlet]
        UIKit.UIDatePicker IBDatePicker { get; set; }


        [Outlet]
        UIKit.UITextField IBEndDateTxt { get; set; }


        [Outlet]
        UIKit.UIButton IBSearchBtn { get; set; }


        [Outlet]
        UIKit.UITextField IBStartDateTxt { get; set; }


        [Outlet]
        UIKit.UILabel IBToLbl { get; set; }


        [Action ("DoneClicked:")]
        partial void DoneClicked (Foundation.NSObject sender);


        [Action ("IBDateChanged:")]
        partial void IBDateChanged (Foundation.NSObject sender);


        [Action ("IBSearchClicked:")]
        partial void IBSearchClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (IBContntTbl != null) {
                IBContntTbl.Dispose ();
                IBContntTbl = null;
            }

            if (IBDateDoneBar != null) {
                IBDateDoneBar.Dispose ();
                IBDateDoneBar = null;
            }

            if (IBDatePicker != null) {
                IBDatePicker.Dispose ();
                IBDatePicker = null;
            }

            if (IBEndDateTxt != null) {
                IBEndDateTxt.Dispose ();
                IBEndDateTxt = null;
            }

            if (IBSearchBtn != null) {
                IBSearchBtn.Dispose ();
                IBSearchBtn = null;
            }

            if (IBStartDateTxt != null) {
                IBStartDateTxt.Dispose ();
                IBStartDateTxt = null;
            }

            if (IBToLbl != null) {
                IBToLbl.Dispose ();
                IBToLbl = null;
            }
        }
    }
}