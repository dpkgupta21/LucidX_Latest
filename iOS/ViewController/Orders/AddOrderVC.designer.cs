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
    [Register ("AddOrderVC")]
    partial class AddOrderVC
    {
        [Outlet]
        UIKit.UIButton BtnPre { get; set; }


        [Outlet]
        UIKit.UIView ContentVw { get; set; }


        [Outlet]
        UIKit.UILabel LblPageCount { get; set; }


        [Action ("BtnPreClicked:")]
        partial void BtnPreClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (ContentVw != null) {
                ContentVw.Dispose ();
                ContentVw = null;
            }

            if (LblPageCount != null) {
                LblPageCount.Dispose ();
                LblPageCount = null;
            }
        }
    }
}