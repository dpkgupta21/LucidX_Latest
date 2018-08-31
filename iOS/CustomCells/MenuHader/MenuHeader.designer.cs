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
    [Register ("MenuHeader")]
    partial class MenuHeader
    {
        [Outlet]
        UIKit.UIImageView IBDropImg { get; set; }


        [Outlet]
        UIKit.UILabel IBTitleLbl { get; set; }


        [Action ("IBHeaderClicked:")]
        partial void IBHeaderClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (IBDropImg != null) {
                IBDropImg.Dispose ();
                IBDropImg = null;
            }

            if (IBTitleLbl != null) {
                IBTitleLbl.Dispose ();
                IBTitleLbl = null;
            }
        }
    }
}