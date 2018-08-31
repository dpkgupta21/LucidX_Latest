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

namespace LucidX.iOS.Notes
{
    public partial class ViewNotesVC : UIViewController, IUITabBarDelegate, IUITableViewDataSource, IUITextFieldDelegate
    {

        public SWRevealViewController revealVC;

        List<CrmNotesResponse> notes = new List<CrmNotesResponse>();
        List<EntityCodesResponse> entitylst = new List<EntityCodesResponse>();
        List<AccountCodesResponse> accountlst = new List<AccountCodesResponse>();


        EntityCodesResponse EntityCode;
        AccountCodesResponse AccountCode;
        UITextField selectedTextFeild;

        EntityPickerModel entityModel;
        AccountCodePickerModel accountModel;

        DateTime StartDate;
        DateTime EndDate;

        public ViewNotesVC() : base("ViewNotesVC", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();
            GetEntityCodeList();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            // GetNotes();
        }


        #region helping Methods

        async void GetNotes()
        {
            if (IosUtils.IosUtility.IsReachable())
            {
                IosUtils.IosUtility.showProgressHud("");
                try
                {
                    var res = await WebServiceMethods.ShowNotes(EntityCode.CompCode, AccountCode.AccountCode, IBFromDateTxt.Text, IBToDateTxt.Text);
                    InvokeOnMainThread(() =>
                        {
                            if (res != null)
                            {
                                notes = res;
                                if (notes.Count == 0)
                                {
                                    IBEmptyLbl.Hidden = false;
                                }
                                else
                                {
                                    IBEmptyLbl.Hidden = true;
                                }
                            }
                            else
                            {
                                IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                                      IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
                            }
                            IBNotesTbl.ReloadData();
                        });
                }
                catch (Exception e)
                {
                    IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"), e.Message);

                }
                IosUtils.IosUtility.hideProgressHud();
            }
        }

        async void GetEntityCodeList()
        {
            if (IosUtils.IosUtility.IsReachable())
            {
                IosUtils.IosUtility.showProgressHud("");
                try
                {
                    var res = await WebServiceMethods.GetEntityCode(Convert.ToInt32(IosUtils.Settings.UserId), IosUtils.Settings.UserCompCode);
                    if (res != null)
                    {
                        InvokeOnMainThread(() =>
                        {
                            entitylst = res;
                            entityModel = new EntityPickerModel(entitylst, IBEntityTxt, entitylst[0]);
                            EntityCode = entitylst[0];
                            IBEntityTxt.Text = entitylst[0].CompName;
                            IBEntityPicker.Model = entityModel;
                        });
                        //IosUtils.Utility.hideProgressHud();
                        GetAccountCodeList();
                    }
                    else
                    {
                        IosUtils.IosUtility.hideProgressHud();
                        IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                          IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
                    }
                }
                catch (Exception e)
                {
                    IosUtils.IosUtility.hideProgressHud();
                    IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                       e.Message);
                }

            }

        }


        async void GetAccountCodeList()
        {

            if (IosUtils.IosUtility.IsReachable())
            {
                IosUtils.IosUtility.showProgressHud("");
                try
                {
                    var res = await WebServiceMethods.GetAccountCodes(EntityCode.CompCode);
                    if (res != null)
                    {
                        InvokeOnMainThread(() =>
                        {
                            accountlst = res;
                            AccountCode = accountlst[0];
                            accountModel = new AccountCodePickerModel(accountlst, IBAccountCodeTxt, accountlst[0]);
                            IBAccountCodePicker.Model = accountModel;
                            IBAccountCodeTxt.Text = AccountCode.AccountName;
                        });
                        SetDateTime();
                    }
                    else
                    {
                        IosUtils.IosUtility.hideProgressHud();
                        IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                          IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
                    }
                }
                catch (Exception e)
                {
                    IosUtils.IosUtility.hideProgressHud();
                    IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"), e.Message);
                }

            }

        }

        void SetDateTime()
        {
            DateTime now = DateTime.Now;
            StartDate = new DateTime(now.Year, now.Month, 1);
            EndDate = StartDate.AddMonths(1).AddDays(-1);
            IBToDateTxt.Text = EndDate.ToString(Utils.Utilities.CALENDAR_DATE_FORMAT);
            IBFromDateTxt.Text = StartDate.ToString(Utils.Utilities.CALENDAR_DATE_FORMAT);
            GetNotes();
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

            IBNotesTbl.RegisterNibForCellReuse(NotesCell.Nib, NotesCell.Key);
            IBNotesTbl.TableFooterView = new UIView();
            IBNotesTbl.EstimatedRowHeight = HeightConstants.CellHeight70;
            this.NavigationItem.Title = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSNotesTitle", "");
            this.NavigationItem.BackBarButtonItem = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, null, null);

            var createBtn = new UIBarButtonItem(UIImage.FromBundle("Add"),
                                              UIBarButtonItemStyle.Plain,
                                                EditClicked);
            this.NavigationItem.RightBarButtonItem = createBtn;

            //IosUtils.Utility.setLeftPadding(IBEntityTxt);
            //IosUtils.Utility.setLeftPadding(IBAccountCodeTxt);
            //IosUtils.Utility.setLeftPadding(IBFromDateTxt);
            //IosUtils.Utility.setLeftPadding(IBToDateTxt);

            IBEntityTxt.InputView = IBEntityPicker;
            IBEntityTxt.InputAccessoryView = IBEntityDoneBar;

            IBAccountCodeTxt.InputView = IBAccountCodePicker;
            IBAccountCodeTxt.InputAccessoryView = IBAccountCodeDoneBar;

            IBDateTimePicker.Mode = UIDatePickerMode.Date;
            IBFromDateTxt.InputView = IBDateTimePicker;
            IBFromDateTxt.InputAccessoryView = IBDateTimeDoneBar;
            IBToDateTxt.InputView = IBDateTimePicker;
            IBToDateTxt.InputAccessoryView = IBDateTimeDoneBar;
            SetupLanguageString();
        }

        void SetupLanguageString()
        {
            IBToLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSToTitle", "");
            IBEntityCodeTiltleLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSEntityTitle", "");
            IBAccountCodeTitleLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSAccountTitle", "");
            IBEmptyLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSNoNotes", "");
        }

        #endregion

        [Export("textFieldShouldBeginEditing:")]
        public bool ShouldBeginEditing(UITextField textField)
        {
            if (entitylst.Count > 0 && accountlst.Count > 0)
            {
                selectedTextFeild = textField;
                if (textField == IBAccountCodeTxt && string.IsNullOrEmpty(IBEntityTxt.Text))
                {
                    IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                       IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSEntityCodeEmpty", "LSErrorTitle"));
                    return false;
                }
                else if (textField == IBFromDateTxt && string.IsNullOrEmpty(IBAccountCodeTxt.Text))
                {
                    IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                       IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSAccountCodeEmpty", "LSErrorTitle"));
                    return false;
                }
                else if (textField == IBFromDateTxt)
                {
                    IBDateTimePicker.Date = IosUtils.IosUtility.ConvertToNSDate(StartDate);
                    IBDateTimePicker.MinimumDate = NSDate.DistantPast; //IosUtils.Utility.ConvertToNSDate(new DateTime(2001, 1, 1, 0, 0, 0));
                }
                else if (textField == IBToDateTxt && string.IsNullOrEmpty(IBFromDateTxt.Text))
                {

                    IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                       IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSFromDateEmpty", "LSErrorTitle"));
                    return false;
                }
                else if (textField == IBToDateTxt)
                {
                    IBDateTimePicker.Date = IosUtils.IosUtility.ConvertToNSDate(EndDate);
                    IBDateTimePicker.MinimumDate = IosUtils.IosUtility.ConvertToNSDate(StartDate);

                }
            }
            else
            {
                IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                       IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSReloadMsg", "LSErrorTitle"));
                return false;
            }
            return true;
        }

        #region IBAction Methods


        void MenuClicked(object sender, EventArgs e)
        {
            revealVC.RevealToggleAnimated(true);
        }

        partial void IBSearchBtnClicked(Foundation.NSObject sender)
        {
            if (string.IsNullOrEmpty(IBEntityTxt.Text) ||
               string.IsNullOrEmpty(IBAccountCodeTxt.Text) ||
               string.IsNullOrEmpty(IBFromDateTxt.Text) ||
               string.IsNullOrEmpty(IBToDateTxt.Text)
              )
            {
                IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                       IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSBlankMsg", "LSErrorTitle"));


            }
            else
            {
                GetNotes();
            }

        }

        void EditClicked(object sender, EventArgs e)
        {
            var createNotesVc = new CreateNotesVC();
            createNotesVc.isEdit = false;
            this.NavigationController.PushViewController(createNotesVc, true);
        }

        partial void IBAccountDoneClicked(Foundation.NSObject sender)
        {
            AccountCode = accountModel.selectedModel;
            selectedTextFeild.EndEditing(true);
            GetNotes();

        }

        partial void IBDateTimeDoneClicked(Foundation.NSObject sender)
        {
            selectedTextFeild.Text = IosUtils.IosUtility.ConvertToDateTime(IBDateTimePicker.Date).ToString(Utils.Utilities.CALENDAR_DATE_FORMAT);
            if (selectedTextFeild == IBFromDateTxt)
            {
                StartDate = IosUtils.IosUtility.ConvertToDateTime(IBDateTimePicker.Date);
            }
            else
            {
                EndDate = IosUtils.IosUtility.ConvertToDateTime(IBDateTimePicker.Date);
            }
            selectedTextFeild.EndEditing(true);
        }

        partial void IBEntityDoneClicked(Foundation.NSObject sender)
        {
            EntityCode = entityModel.selectedModel;
            GetAccountCodeList();
            selectedTextFeild.EndEditing(true);

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
            return notes.Count;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(NotesCell.Key) as NotesCell;
            cell.ConfigureCell(notes[indexPath.Row]);
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
            if (IosUtils.IosUtility.IsReachable())
            {
                var createNotesVc = new CreateNotesVC();
                createNotesVc.isEdit = true;
                createNotesVc.notes = notes[indexPath.Row];
                createNotesVc.accountId = AccountCode.AccountId;
                createNotesVc.accountCode = AccountCode.AccountCode;
                createNotesVc.entityCode = EntityCode.CompCode;

                this.NavigationController.PushViewController(createNotesVc, true);
            }
        }


        #endregion




    }
}

