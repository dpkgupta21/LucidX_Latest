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
    [Register ("AddOrderSecond")]
    partial class AddOrderSecond
    {
        [Outlet]
        UIKit.UIButton BtnAdd { get; set; }


        [Outlet]
        UIKit.UIButton BtnNext { get; set; }


        [Outlet]
        UIKit.UITableView ContntTbl { get; set; }


        [Action ("BtnAddClicked:")]
        partial void BtnAddClicked (Foundation.NSObject sender);


        [Action ("BtnNextClicked:")]
        partial void BtnNextClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (BtnAdd != null) {
                BtnAdd.Dispose ();
                BtnAdd = null;
            }

            if (BtnNext != null) {
                BtnNext.Dispose ();
                BtnNext = null;
            }

            if (ContntTbl != null) {
                ContntTbl.Dispose ();
                ContntTbl = null;
            }
        }
    }
}