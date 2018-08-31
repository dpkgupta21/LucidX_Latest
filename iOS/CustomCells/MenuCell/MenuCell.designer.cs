// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LucidX.iOS.CustomCells
{
    [Register ("MenuCell")]
    partial class MenuCell
    {
        [Outlet]
        UIKit.UIView IBBackVw { get; set; }


        [Outlet]
        UIKit.UILabel IBCountLbl { get; set; }


        [Outlet]
        UIKit.UIImageView IBIconCell { get; set; }


        [Outlet]
        UIKit.UILabel IBTitleLbl { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IBBackVw != null) {
                IBBackVw.Dispose ();
                IBBackVw = null;
            }

            if (IBCountLbl != null) {
                IBCountLbl.Dispose ();
                IBCountLbl = null;
            }

            if (IBIconCell != null) {
                IBIconCell.Dispose ();
                IBIconCell = null;
            }

            if (IBTitleLbl != null) {
                IBTitleLbl.Dispose ();
                IBTitleLbl = null;
            }
        }
    }
}