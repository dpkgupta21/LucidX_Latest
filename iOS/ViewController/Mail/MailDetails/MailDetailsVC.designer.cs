// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace MailDetails
{
    [Register ("MailDetailsVC")]
    partial class MailDetailsVC
    {
        [Outlet]
        UIKit.UIButton IBBtmReplyBtn { get; set; }


        [Outlet]
        UIKit.UILabel IBBtmReplyLbl { get; set; }


        [Outlet]
        UIKit.UITextView IBContnTxt { get; set; }


        [Outlet]
        UIKit.UIButton IBForwardBtn { get; set; }


        [Outlet]
        UIKit.UILabel IBForwardLbl { get; set; }


        [Outlet]
        UIKit.UIButton IBInboxbtn { get; set; }


        [Outlet]
        UIKit.UIButton IBMenuBtn { get; set; }


        [Outlet]
        UIKit.UILabel IBNameIconLbl { get; set; }


        [Outlet]
        UIKit.UILabel IBNameLbl { get; set; }


        [Outlet]
        UIKit.UIButton IBReplyAllBtn { get; set; }


        [Outlet]
        UIKit.UILabel IBReplyAllLbl { get; set; }


        [Outlet]
        UIKit.UIButton IBReplyBtn { get; set; }


        [Outlet]
        UIKit.UILabel IBSubjectLbl { get; set; }


        [Action ("ForwardBtnClicked:")]
        partial void ForwardBtnClicked (Foundation.NSObject sender);


        [Action ("InboxBtnClicked:")]
        partial void InboxBtnClicked (Foundation.NSObject sender);


        [Action ("MenuBtnClicked:")]
        partial void MenuBtnClicked (Foundation.NSObject sender);


        [Action ("ReplyAllBtnClicked:")]
        partial void ReplyAllBtnClicked (Foundation.NSObject sender);


        [Action ("ReplyBtnClicked:")]
        partial void ReplyBtnClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (IBBtmReplyBtn != null) {
                IBBtmReplyBtn.Dispose ();
                IBBtmReplyBtn = null;
            }

            if (IBBtmReplyLbl != null) {
                IBBtmReplyLbl.Dispose ();
                IBBtmReplyLbl = null;
            }

            if (IBContnTxt != null) {
                IBContnTxt.Dispose ();
                IBContnTxt = null;
            }

            if (IBForwardBtn != null) {
                IBForwardBtn.Dispose ();
                IBForwardBtn = null;
            }

            if (IBForwardLbl != null) {
                IBForwardLbl.Dispose ();
                IBForwardLbl = null;
            }

            if (IBInboxbtn != null) {
                IBInboxbtn.Dispose ();
                IBInboxbtn = null;
            }

            if (IBMenuBtn != null) {
                IBMenuBtn.Dispose ();
                IBMenuBtn = null;
            }

            if (IBNameIconLbl != null) {
                IBNameIconLbl.Dispose ();
                IBNameIconLbl = null;
            }

            if (IBNameLbl != null) {
                IBNameLbl.Dispose ();
                IBNameLbl = null;
            }

            if (IBReplyAllBtn != null) {
                IBReplyAllBtn.Dispose ();
                IBReplyAllBtn = null;
            }

            if (IBReplyAllLbl != null) {
                IBReplyAllLbl.Dispose ();
                IBReplyAllLbl = null;
            }

            if (IBReplyBtn != null) {
                IBReplyBtn.Dispose ();
                IBReplyBtn = null;
            }

            if (IBSubjectLbl != null) {
                IBSubjectLbl.Dispose ();
                IBSubjectLbl = null;
            }
        }
    }
}