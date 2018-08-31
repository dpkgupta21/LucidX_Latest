using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.SWRevealViewController;
using LucidX.iOS.CustomCells;
using LucidX.iOS.Notes;
using LucidX.iOS.Calendar;

namespace LucidX.iOS
{
    public partial class SettingsVc : UIViewController, IUITableViewDelegate, IUITableViewDataSource
    {
        public SWRevealViewController revealVC;

        CalendarVC calendarVc;
        Orders.OrderListVC orderVc;



        public Inbox.InboxVC inboxVc;
        public Notes.ViewNotesVC notesVC;
        bool isfirstTime;

        NSIndexPath selectedIndex = NSIndexPath.FromRowSection(0, 0);
        public SettingsVc() : base("SettingsVc", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            isfirstTime = true;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (isfirstTime)
            {
                configureView();
                isfirstTime = false;
            }
            IBContntTbl.ReloadData();
        }
        #region HelperMethods

        void configureView()
        {
            IBProfileImg.Layer.CornerRadius = IBProfileImg.Frame.Width / 2;
            IBProfileImg.Layer.BorderWidth = 1;
            IBProfileImg.Layer.BorderColor = UIColor.White.CGColor;
            IBProfileImg.ClipsToBounds = true;
            GetLaguageStrings();
            IBNameLbl.Text = IosUtils.Settings.Name;
            IBMailAddLbl.Text = IosUtils.Settings.UserMail;
            IBContntTbl.RegisterNibForCellReuse(MenuCell.Nib, MenuCell.Key);
            GetCount();
            IBContntTbl.EstimatedRowHeight = 50;
        }

        async void GetCount()
        {
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                var res = await Webservices.WebServiceMethods.EmailCount(IosUtils.Settings.UserId);
                if (res != null)
                {
                }
            }
        }


        void GetLaguageStrings()
        {
            SectionTitle.Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSMailTitle", ""));
            SectionTitle.Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSCalendar", ""));
            SectionTitle.Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Order", ""));
            SectionTitle.Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Notes", ""));
            SectionTitle.Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Logout", ""));

            RowsTitle[0].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSInbox", ""));
            RowsImgs[0].Add(UIImage.FromBundle("Inbox"));

            RowsTitle[0].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSDrafts", ""));
            RowsImgs[0].Add(UIImage.FromBundle("Drafts"));

            RowsTitle[0].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSSent", ""));
            RowsImgs[0].Add(UIImage.FromBundle("Sent"));

            RowsTitle[0].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSTrash", ""));
            RowsImgs[0].Add(UIImage.FromBundle("Trash"));

            RowsTitle[1].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("List", ""));
            RowsImgs[1].Add(UIImage.FromBundle("Calendar"));

            RowsTitle[1].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Add", ""));
            RowsImgs[1].Add(UIImage.FromBundle("CalendarAdd"));


            RowsTitle[2].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("View Order", ""));
            RowsImgs[2].Add(UIImage.FromBundle("Orders"));

            RowsTitle[2].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Amend Order", ""));
            RowsImgs[2].Add(UIImage.FromBundle("OrderAmend"));

            RowsTitle[2].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Create Order", ""));
            RowsImgs[2].Add(UIImage.FromBundle("OrderAdd"));

            RowsTitle[2].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Convert Order", ""));
            RowsImgs[2].Add(UIImage.FromBundle("OrderConvert"));

            RowsTitle[2].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Delete Order", ""));
            RowsImgs[2].Add(UIImage.FromBundle("Trash"));


            RowsTitle[3].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("View Notes", ""));
            RowsImgs[3].Add(UIImage.FromBundle("Notes"));

            RowsTitle[3].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Amend Notes", ""));
            RowsImgs[3].Add(UIImage.FromBundle("NotesAmend"));

            RowsTitle[3].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Create Notes", ""));
            RowsImgs[3].Add(UIImage.FromBundle("NotesAdd"));

            RowsTitle[3].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Delete Notes", ""));
            RowsImgs[3].Add(UIImage.FromBundle("NotesDelete"));

            RowsTitle[4].Add(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Log Out", ""));
            RowsImgs[4].Add(UIImage.FromBundle("Logout"));

            IBContntTbl.ReloadData();
        }

        #endregion

        #region IBAction Methods

        void ShowVC(UIViewController VC)
        {
            var navVc = new UINavigationController(VC);
            var fixitVw = new UIView(new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 20));
            fixitVw.BackgroundColor = IosUtils.IosColorConstant.ThemeNavBlue;
            navVc.View.AddSubview(fixitVw);
            revealVC.FrontViewController = navVc;
            revealVC.RevealToggleAnimated(true);
        }


        #endregion


        #region TableView Delegate and data source methods

        List<int> RowsCount = new List<int> { 4, 2, 5, 4, 0 };
        bool IsExpanded = true;
        int SelectedSection = 0;
        List<string> SectionTitle = new List<string>();
        List<List<string>> RowsTitle = new List<List<string>>() { new List<string>(),
            new List<string> (),
            new List<string> (),
            new List<string> (),
            new List<string> (),
        };

        List<List<UIImage>> RowsImgs = new List<List<UIImage>>() { new List<UIImage>(),
            new List<UIImage> (),
            new List<UIImage> (),
            new List<UIImage> (),
            new List<UIImage> (),
        };

        [Export("numberOfSectionsInTableView:")]
        public nint NumberOfSections(UITableView tableView)
        {
            return SectionTitle.Count;
        }


        [Export("tableView:viewForHeaderInSection:")]
        public UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var header = MenuHeader.Create();
            header.BackgroundColor = IosUtils.IosColorConstant.ThemeDarkBlue;
            header.Frame = new CGRect(0, 0, tableView.Bounds.Width, 70);
            if (section == SectionTitle.Count - 1)
            {
                header.Configure(SectionTitle[(int)section], (int)section, false, true);
            }
            else
            {
                if (section == SelectedSection)
                {
                    header.Configure(SectionTitle[(int)section], (int)section, IsExpanded);

                }
                else
                {
                    header.Configure(SectionTitle[(int)section], (int)section, false);
                }
            }


            header.Clicked -= Header_Clicked;
            header.Clicked += Header_Clicked;
            return header;
        }

        [Export("tableView:heightForHeaderInSection:")]
        public nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 50;
        }

        [Export("tableView:cellForRowAtIndexPath:")]
        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(MenuCell.Key) as MenuCell;
            cell.ConfigureCell(RowsTitle[indexPath.Section][indexPath.Row], RowsImgs[indexPath.Section][indexPath.Row], 0, indexPath == selectedIndex);
            return cell;
        }

        [Export("tableView:numberOfRowsInSection:")]
        public nint RowsInSection(UITableView tableView, nint section)
        {
            if (section == SelectedSection)
            {
                return IsExpanded ? RowsCount[(int)section] : 0;
            }
            return 0;
        }

        [Export("tableView:heightForRowAtIndexPath:")]
        public nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 40;
        }

        void Header_Clicked(object sender, bool e)
        {
            var vw = sender as MenuHeader;
            IsExpanded = e;
            SelectedSection = (int)vw.Tag;
            this.IBContntTbl.ReloadData();

            if (SelectedSection == SectionTitle.Count - 1)
            {
                IosUtils.Settings.RemoveLoginInfo();
                AppDelegate.GetSharedInstance().showLoginScreen();
            }
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            selectedIndex = indexPath;
            if (indexPath.Section == 0)
            {
                if (inboxVc == null)
                {
                    inboxVc = new Inbox.InboxVC();
                    inboxVc.revealVC = revealVC;
                }

                //if (indexPath.Row == 0)
                //{
                //	inboxVc.MailTypeId = 1;
                //}
                //else if (indexPath.Row == 1)
                //{
                //	inboxVc.MailTypeId = 2;
                //}
                //else if (indexPath.Row == 2)
                //{
                //	inboxVc.
                //}
                //else if (indexPath.Row == 3)
                //{

                //}

                ShowVC(inboxVc);
                inboxVc.MailTypeId = indexPath.Row + 1;

            }
            else if (indexPath.Section == 1)
            {
                if (indexPath.Row == 0)
                {
                    if (calendarVc == null)
                    {
                        calendarVc = new CalendarVC();
                    }
                    calendarVc.revealVC = revealVC;
                    ShowVC(calendarVc);
                }
                else if (indexPath.Row == 1)
                {
                    // add calendar event
                    if (calendarVc != null)
                    {
                        var createNotesVc = new CreateCalendarEventVC();
                        calendarVc.NavigationController.PushViewController(createNotesVc, true);
                        revealVC.RevealToggleAnimated(true);
                    }
                    else if (notesVC == null)
                    {
                        calendarVc = new CalendarVC();
                        calendarVc.revealVC = revealVC;
                        ShowVC(calendarVc);
                        var createNotesVc = new CreateCalendarEventVC();
                        calendarVc.NavigationController.PushViewController(createNotesVc, true);
                    }
                }
            }
            else if (indexPath.Section == 2)
            {
                if (indexPath.Row == 0)
                {
                    if (orderVc == null)
                    {
                        orderVc = new Orders.OrderListVC();
                        orderVc.revealVC = revealVC;
                    }
                    ShowVC(orderVc);

                }
            }
            else if (indexPath.Section == 3)
            {
                if (indexPath.Row == 0)
                {
                    if (notesVC == null)
                    {
                        notesVC = new Notes.ViewNotesVC();
                        notesVC.revealVC = revealVC;
                    }
                    ShowVC(notesVC);

                }
                else if (indexPath.Row == 1)
                {
                    if (notesVC == null)
                    {
                        notesVC = new Notes.ViewNotesVC();
                        notesVC.revealVC = revealVC;
                    }
                    ShowVC(notesVC);
                }
                else if (indexPath.Row == 2)
                {
                    if (notesVC != null)
                    {
                        var createNotesVc = new CreateNotesVC();
                        notesVC.NavigationController.PushViewController(createNotesVc, true);
                        revealVC.RevealToggleAnimated(true);
                    }
                    else if (notesVC == null)
                    {
                        notesVC = new Notes.ViewNotesVC();
                        notesVC.revealVC = revealVC;
                        ShowVC(notesVC);
                        var createNotesVc = new CreateNotesVC();
                        notesVC.NavigationController.PushViewController(createNotesVc, true);
                    }
                }
                else if (indexPath.Row == 3)
                {
                    if (notesVC == null)
                    {
                        notesVC = new Notes.ViewNotesVC();
                        notesVC.revealVC = revealVC;
                    }
                    ShowVC(notesVC);
                }

            }
            else if (indexPath.Section == 4)
            {

            }


        }




        #endregion

    }
}

