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
    [Register ("MultiSelectionDialog")]
    partial class MultiSelectionDialog
    {
        [Outlet]
        UIKit.UIView ContntVw { get; set; }


        [Outlet]
        UIKit.UIButton IBCancelBtn { get; set; }


        [Outlet]
        UIKit.UITableView IBContntTable { get; set; }


        [Outlet]
        UIKit.UIButton IBSaveBtn { get; set; }


        [Outlet]
        UIKit.UILabel IBTtitleLbl { get; set; }


        [Action ("IBCancelClicked:")]
        partial void IBCancelClicked (Foundation.NSObject sender);


        [Action ("IBSaveClicked:")]
        partial void IBSaveClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (ContntVw != null) {
                ContntVw.Dispose ();
                ContntVw = null;
            }

            if (IBCancelBtn != null) {
                IBCancelBtn.Dispose ();
                IBCancelBtn = null;
            }

            if (IBContntTable != null) {
                IBContntTable.Dispose ();
                IBContntTable = null;
            }

            if (IBSaveBtn != null) {
                IBSaveBtn.Dispose ();
                IBSaveBtn = null;
            }

            if (IBTtitleLbl != null) {
                IBTtitleLbl.Dispose ();
                IBTtitleLbl = null;
            }
        }
    }
}