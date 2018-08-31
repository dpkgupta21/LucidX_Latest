using System;
using System.Collections.Generic;
using Foundation;
using IosUtils;
using LucidX.ResponseModels;
using LucidX.Webservices;
using UIKit;
using Xamarin.SWRevealViewController;
using LucidX.iOS.CustomCells;
using LucidX.iOS.PickerModels;
using LucidX.iOS.OrderCell;

namespace LucidX.iOS.Orders
{
    public partial class OrderListVC : UIViewController, IUITableViewDelegate, IUITableViewDataSource, IUITextFieldDelegate
    {
        public SWRevealViewController revealVC;
        DateTime StartDate;
        DateTime EndDate;

        UITextField selectedField;

        /// <summary>
        /// The Orders list object
        /// </summary>
        private List<LedgerOrder> ledgerOrderList = new List<LedgerOrder>();

        public OrderListVC() : base("OrderListVC", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();
            SetDateTime();

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            CallWebserviceForOrdersList();
        }

        #region Helping Methods

        void SetDateTime()
        {
            DateTime now = DateTime.Now;
            StartDate = new DateTime(now.Year, now.Month, 1);
            EndDate = StartDate.AddMonths(1).AddDays(-1);
            IBStartDateTxt.Text = StartDate.ToString(Utils.Utilities.CALENDAR_DATE_FORMAT);
            IBEndDateTxt.Text = EndDate.ToString(Utils.Utilities.CALENDAR_DATE_FORMAT);

        }

        /// <summary>
        /// Configures the view.
        /// </summary>
        void ConfigureView()
        {
            this.EdgesForExtendedLayout = UIRectEdge.None;

            var menuBtn = new UIBarButtonItem(UIImage.FromBundle("Menu"),
                                              UIBarButtonItemStyle.Plain,
                                              MenuClicked);
            this.NavigationItem.LeftBarButtonItem = menuBtn;

            IBContntTbl.RegisterNibForCellReuse(OrderListCell.Nib, OrderListCell.Key);
            IBContntTbl.TableFooterView = new UIView();
            IBContntTbl.EstimatedRowHeight = HeightConstants.CellHeight70;

            this.NavigationItem.Title = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Order", "");
            this.NavigationItem.BackBarButtonItem = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, null, null);

            var createBtn = new UIBarButtonItem(UIImage.FromBundle("Add"),
                                              UIBarButtonItemStyle.Plain,
                                                AddClicked);
            this.NavigationItem.RightBarButtonItem = createBtn;


            IBDatePicker.Mode = UIDatePickerMode.Date;
            IBStartDateTxt.InputView = IBDatePicker;
            IBStartDateTxt.InputAccessoryView = IBDateDoneBar;
            IBEndDateTxt.InputView = IBDatePicker;
            IBEndDateTxt.InputAccessoryView = IBDateDoneBar;
            SetupLanguageString();
        }

        void SetupLanguageString()
        {
            IBToLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSToTitle", "");
        }


        public async void CallWebserviceForOrdersList()
        {
            try
            {
                if (IosUtility.IsReachable())
                {
                    IosUtility.showProgressHud("");

                    ledgerOrderList = await WebServiceMethods.GetOrders(Settings.UserId,
                                                                        IBStartDateTxt.Text, IBEndDateTxt.Text);

                    IBContntTbl.ReloadData();
                    IosUtility.hideProgressHud();
                }
            }
            catch (Exception ex)
            {
                IosUtility.hideProgressHud();
                IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                              IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(IBStartDateTxt.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(IBEndDateTxt.Text))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region IBAction Methods


        void MenuClicked(object sender, EventArgs e)
        {
            revealVC.RevealToggleAnimated(true);
        }


        void AddClicked(object sender, EventArgs e)
        {
            var VC = new AddOrderVC();
            this.NavigationController.PushViewController(VC, true);
        }

        partial void DoneClicked(Foundation.NSObject sender)
        {

            selectedField.Text = IosUtils.IosUtility.ConvertToDateTime(IBDatePicker.Date).
                ToString(Utils.Utilities.CALENDAR_DATE_FORMAT).
                Replace("-", "/");
            if (selectedField == IBStartDateTxt)
            {
                StartDate = IosUtils.IosUtility.ConvertToDateTime(IBDatePicker.Date);
            }
            else
            {
                EndDate = IosUtils.IosUtility.ConvertToDateTime(IBDatePicker.Date);
            }
            this.View.EndEditing(true);
        }

        partial void IBDateChanged(Foundation.NSObject sender)
        {


        }

        partial void IBSearchClicked(Foundation.NSObject sender)
        {
            CallWebserviceForOrdersList();
        }


        #endregion
        #region textfeild delegate methods

        [Export("textFieldShouldBeginEditing:")]
        public bool ShouldBeginEditing(UITextField textField)
        {

            selectedField = textField;
            return true;
        }


        #endregion


        #region Tableview delegate and database methods

        [Export("numberOfSectionsInTableView:")]
        public nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public nint RowsInSection(UITableView tableView, nint section)
        {
            return ledgerOrderList.Count;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(OrderListCell.Key) as OrderListCell;
            cell.configure(ledgerOrderList[indexPath.Row]);
            return cell;
        }
        [Export("tableView:heightForRowAtIndexPath:")]
        public nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var VC = new AddOrderVC();
            VC.LedgerOrderObj = ledgerOrderList[indexPath.Row];
            this.NavigationController.PushViewController(VC, true);
        }


        #endregion

    }
}

