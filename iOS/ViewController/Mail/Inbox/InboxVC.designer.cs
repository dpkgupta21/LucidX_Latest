// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Inbox
{
    [Register ("InboxVC")]
    partial class InboxVC
    {
        [Outlet]
        UIKit.UIButton IBCancelBtn { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint IBCancelWidth { get; set; }


        [Outlet]
        UIKit.UITableView IBContntTbl { get; set; }


        [Outlet]
        UIKit.UISearchBar IBSearchBar { get; set; }


        [Action ("IBCancelClicked:")]
        partial void IBCancelClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (IBCancelBtn != null) {
                IBCancelBtn.Dispose ();
                IBCancelBtn = null;
            }

            if (IBCancelWidth != null) {
                IBCancelWidth.Dispose ();
                IBCancelWidth = null;
            }

            if (IBContntTbl != null) {
                IBContntTbl.Dispose ();
                IBContntTbl = null;
            }

            if (IBSearchBar != null) {
                IBSearchBar.Dispose ();
                IBSearchBar = null;
            }
        }
    }
}