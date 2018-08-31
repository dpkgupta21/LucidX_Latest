// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LucidX.iOS.OrderCell
{
    [Register ("OrderListCell")]
    partial class OrderListCell
    {
        [Outlet]
        UIKit.UILabel IBCostTitleLbl { get; set; }


        [Outlet]
        UIKit.UILabel IBCostValueLbl { get; set; }


        [Outlet]
        UIKit.UILabel IBDateLbl { get; set; }


        [Outlet]
        UIKit.UILabel IBNameLbl { get; set; }


        [Outlet]
        UIKit.UILabel IBTitleLbl { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IBCostTitleLbl != null) {
                IBCostTitleLbl.Dispose ();
                IBCostTitleLbl = null;
            }

            if (IBCostValueLbl != null) {
                IBCostValueLbl.Dispose ();
                IBCostValueLbl = null;
            }

            if (IBDateLbl != null) {
                IBDateLbl.Dispose ();
                IBDateLbl = null;
            }

            if (IBNameLbl != null) {
                IBNameLbl.Dispose ();
                IBNameLbl = null;
            }

            if (IBTitleLbl != null) {
                IBTitleLbl.Dispose ();
                IBTitleLbl = null;
            }
        }
    }
}