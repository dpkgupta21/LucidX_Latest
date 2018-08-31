// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace CustomCells
{
    [Register ("MailDetailsCell")]
    partial class MailDetailsCell
    {
        [Outlet]
        UIKit.UILabel IBDateTimeLbl { get; set; }


        [Outlet]
        UIKit.UIButton IBDeleteBtn { get; set; }


        [Outlet]
        UIKit.UILabel IBDescLbl { get; set; }


        [Outlet]
        UIKit.UILabel IBMailAddressLbl { get; set; }


        [Outlet]
        UIKit.UILabel IBTitleLbl { get; set; }


        [Action ("IBDeleteBtnClicked:")]
        partial void IBDeleteBtnClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (IBDateTimeLbl != null) {
                IBDateTimeLbl.Dispose ();
                IBDateTimeLbl = null;
            }

            if (IBDescLbl != null) {
                IBDescLbl.Dispose ();
                IBDescLbl = null;
            }

            if (IBMailAddressLbl != null) {
                IBMailAddressLbl.Dispose ();
                IBMailAddressLbl = null;
            }

            if (IBTitleLbl != null) {
                IBTitleLbl.Dispose ();
                IBTitleLbl = null;
            }
        }
    }
}