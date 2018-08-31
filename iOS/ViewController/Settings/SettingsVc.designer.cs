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
    [Register ("SettingsVc")]
    partial class SettingsVc
    {
        [Outlet]
        UIKit.UITableView IBContntTbl { get; set; }


        [Outlet]
        UIKit.UILabel IBMailAddLbl { get; set; }


        [Outlet]
        UIKit.UILabel IBNameLbl { get; set; }


        [Outlet]
        UIKit.UIImageView IBProfileImg { get; set; }


        [Outlet]
        UIKit.UIView IBProfileVw { get; set; }


        [Action ("CloseBtnClicked:")]
        partial void CloseBtnClicked (Foundation.NSObject sender);


        [Action ("IBCalendarClicked:")]
        partial void IBCalendarClicked (Foundation.NSObject sender);


        [Action ("IBDraftClicked:")]
        partial void IBDraftClicked (Foundation.NSObject sender);


        [Action ("IBInboxClicked:")]
        partial void IBInboxClicked (Foundation.NSObject sender);


        [Action ("IBInvoiceClicked:")]
        partial void IBInvoiceClicked (Foundation.NSObject sender);


        [Action ("IBSentClicked:")]
        partial void IBSentClicked (Foundation.NSObject sender);


        [Action ("IBTrashClicked:")]
        partial void IBTrashClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (IBContntTbl != null) {
                IBContntTbl.Dispose ();
                IBContntTbl = null;
            }

            if (IBMailAddLbl != null) {
                IBMailAddLbl.Dispose ();
                IBMailAddLbl = null;
            }

            if (IBNameLbl != null) {
                IBNameLbl.Dispose ();
                IBNameLbl = null;
            }

            if (IBProfileImg != null) {
                IBProfileImg.Dispose ();
                IBProfileImg = null;
            }

            if (IBProfileVw != null) {
                IBProfileVw.Dispose ();
                IBProfileVw = null;
            }
        }
    }
}