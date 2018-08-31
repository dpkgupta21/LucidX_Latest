using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using LucidX.iOS;
using LucidX.ResponseModels;
using LucidX.Webservices;
using Plugin.Connectivity;
using UIKit;
using Xamarin.SWRevealViewController;
using LucidX.iOS.PickerModels;
using LucidX.iOS.CustomCells;
using LucidX.RequestModels;
using LucidX.Constants;
using LucidX.Utils;

namespace LucidX.iOS.Calendar
{
    public partial class CreateCalendarEventVC : UIViewController, IUITextFieldDelegate
    {
        public SWRevealViewController revealVC;

        List<RefUsersResponse> responseList = new List<RefUsersResponse>();
        List<NotesTypeResponse> _notesTypeList = new List<NotesTypeResponse>();
        RefUsersResponse selectedUser;
        UserRefPicker userPickerModel;
        public CalendarEventResponse Event;

        UITextField selectedFeild;

        NotesTypeResponse SelectedType;
        EventTypePickerModel eventPickerModel;

        DateTime StartDate;
        DateTime EndDate;

        public bool isEdit;



        public CreateCalendarEventVC() : base("CreateCalendarEventVC", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            GetValues();
        }

        /// <summary>
        /// Configures the view.
        /// </summary>
        void ConfigureView()
        {
            this.EdgesForExtendedLayout = UIRectEdge.None;
            this.NavigationItem.Title = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSCalendar", "");
            IosUtils.IosUtility.setBorderColor(IBDetailsTxt);
            IosUtils.IosUtility.setcornerRadius(IBSaveBtn);
            IosUtils.IosUtility.setcornerRadius(IBCancelBtn);

            IBAssignedToTxt.InputView = IBAssignedToPicker;
            IBAssignedToTxt.InputAccessoryView = IBAssignedToDoneBar;

            IBEventTypeTxt.InputView = IBEventTypePicker;
            IBEventTypeTxt.InputAccessoryView = IBEventTypeDoneBar;

            if (isEdit)
            {
                var createBtn = new UIBarButtonItem(UIImage.FromBundle("Delete"),
                                                  UIBarButtonItemStyle.Plain,
                                                    DeleteClicked);
                this.NavigationItem.RightBarButtonItem = createBtn;
            }

            IBDateTimePicker.Mode = UIDatePickerMode.DateAndTime;
            IBFromDateTxt.InputView = IBDateTimePicker;
            IBFromDateTxt.InputAccessoryView = IBDateTimeDoneBar;
            IBToDateTxt.InputView = IBDateTimePicker;
            IBToDateTxt.InputAccessoryView = IBDateTimeDoneBar;
            SetupLanguageString();
        }

        void SetupLanguageString()
        {

            IBSaveBtn.SetTitle(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSSave", ""),
                             UIControlState.Normal);
            IBCancelBtn.SetTitle(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSCancelKey", ""),
                             UIControlState.Normal);


        }

        private async void GetValues()
        {
            try
            {
                if (IosUtils.IosUtility.IsReachable())
                {
                    IosUtils.IosUtility.showProgressHud("");

                    SetDateTime();

                    responseList = await WebServiceMethods.ShowRefUsers(IosUtils.Settings.UserCompCode);

                    if (responseList != null && responseList.Count > 0)
                    {
                        if (isEdit)
                        {
                            selectedUser = responseList.Where(u => u.UserID == Event.AssignedTo).FirstOrDefault();
                            IBAssignedToTxt.Text = selectedUser != null ? selectedUser.UserName : "";
                            IBSubjectTxt.Text = Event.Subject;
                            IBDetailsTxt.Text = Event.Details;
                            IBFromDateTxt.Text = Event.DateStart.ToString(Utilities.CALENDAR_DATE_FORMAT);
                            IBToDateTxt.Text = Event.DateEnd.ToString(Utilities.CALENDAR_DATE_FORMAT);
                        }
                        else
                        {
                            selectedUser = responseList.Where(u => u.UserID.Equals(IosUtils.Settings.UserId)).
                                                       FirstOrDefault();
                            IBAssignedToTxt.Text = selectedUser.UserName;
                        }
                        userPickerModel = new UserRefPicker(responseList, IBAssignedToTxt, selectedUser);
                        IBAssignedToPicker.Model = userPickerModel;
                    }

                    _notesTypeList = await WebServiceMethods.ShowNotesType();

                    if (_notesTypeList != null && _notesTypeList.Count > 0)
                    {
                        if (isEdit)
                        {
                            SelectedType = _notesTypeList.Where(u => u.NotesTypeId == Convert.ToInt32(Event.NotesTypeId))
                                                        .FirstOrDefault();
                            IBEventTypeTxt.Text = SelectedType.NotesTypeName;
                            eventPickerModel = new EventTypePickerModel(_notesTypeList, IBEventTypeTxt, SelectedType);
                            IBEventTypePicker.Model = eventPickerModel;
                        }
                        else
                        {
                            SelectedType = _notesTypeList[0];
                            IBEventTypeTxt.Text = SelectedType.NotesTypeName;
                            eventPickerModel = new EventTypePickerModel(_notesTypeList, IBEventTypeTxt, SelectedType);
                            IBEventTypePicker.Model = eventPickerModel;
                        }
                    }

                    IosUtils.IosUtility.hideProgressHud();

                }
            }
            catch (Exception e)
            {
                IosUtils.IosUtility.hideProgressHud();
            }
        }

        async void AddCalendarEvent()
        {
            if (IosUtils.IosUtility.IsReachable())
            {
                IosUtils.IosUtility.showProgressHud("");
                try
                {
                    AddCalendarEventsAPIParams prams = new AddCalendarEventsAPIParams()
                    {
                        subject = IBSubjectTxt.Text,
                        details = IBDetailsTxt.Text,
                        startDate = StartDate.ToString(Utils.Utilities.CALENDAR_DATE_FORMAT),
                        endDate = EndDate.ToString(Utils.Utilities.CALENDAR_DATE_FORMAT),
                        entryId = (isEdit) ? (Event.EntryId) : "0",
                        compCode = "84451",
                        accCode = "1718904",
                        notesTypeId = SelectedType.NotesTypeId.ToString(),
                        entryTypeId = 0,
                        assignedTo = selectedUser.UserID.ToString(),
                        summaryItemId = "999999999",
                        isCompleted = false,
                        isPublic = false,
                        accountId = "216",
                        connectionName = WebserviceConstants.CONNECTION_NAME
                    };

                    var res = await WebServiceMethods.AddCalendarEvents(prams);
                    IosUtils.IosUtility.hideProgressHud();
                    if (res)
                    {
                        this.NavigationController.PopViewController(true);
                    }
                    else
                    {
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

        void SetDateTime()
        {
            DateTime now = DateTime.Now;
            StartDate = new DateTime(now.Year, now.Month, 1);
            EndDate = StartDate.AddMonths(1).AddDays(-1);
            IBToDateTxt.Text = EndDate.ToString(Utils.Utilities.CALENDAR_DATE_FORMAT);
            IBFromDateTxt.Text = StartDate.ToString(Utils.Utilities.CALENDAR_DATE_FORMAT);
        }

        #region IBAction Methods
        async void DeleteClicked(object sender, EventArgs e)
        {
            if (IosUtils.IosUtility.IsReachable())
            {
                IosUtils.IosUtility.showProgressHud("");
                try
                {
                    if (isEdit && Event != null)
                    {
                        var res = await WebServiceMethods.DeleteEvents(Event.EntryId);


                        IosUtils.IosUtility.hideProgressHud();
                        if (res)
                        {
                            this.NavigationController.PopViewController(true);
                        }
                        else
                        {
                            IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                              IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    IosUtils.IosUtility.hideProgressHud();
                    IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                       ex.Message);
                }
            }
        }

        partial void BtnCancelClicked(Foundation.NSObject sender)
        {
            this.NavigationController.PopViewController(true);
        }


        partial void BtnSaveClicked(Foundation.NSObject sender)
        {
            if (string.IsNullOrEmpty(IBAssignedToTxt.Text) ||
                string.IsNullOrEmpty(IBEventTypeTxt.Text) ||
                string.IsNullOrEmpty(IBSubjectTxt.Text) ||
                string.IsNullOrEmpty(IBDetailsTxt.Text) ||
                string.IsNullOrEmpty(IBFromDateTxt.Text) ||
                string.IsNullOrEmpty(IBToDateTxt.Text)
              )
            {
                IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                       IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSBlankMsg", "LSErrorTitle"));


            }
            else
            {
                AddCalendarEvent();
            }


        }


        partial void IBAssignedToDoneClicked(Foundation.NSObject sender)
        {
            selectedUser = userPickerModel.selectedModel;
            IBAssignedToTxt.EndEditing(true);

        }
        partial void IBDateTimeDoneClicked(Foundation.NSObject sender)
        {
            selectedFeild.Text = IosUtils.IosUtility.ConvertToDateTime(IBDateTimePicker.Date).ToLocalTime().ToString("g");
            if (selectedFeild == IBFromDateTxt)
            {
                StartDate = IosUtils.IosUtility.ConvertToDateTime(IBDateTimePicker.Date);
            }
            else
            {
                EndDate = IosUtils.IosUtility.ConvertToDateTime(IBDateTimePicker.Date);
            }
            selectedFeild.EndEditing(true);

        }

        partial void IBEventTypeDoneClicked(Foundation.NSObject sender)
        {
            SelectedType = eventPickerModel.selectedModel;
            IBEventTypeTxt.EndEditing(true);

        }
        #endregion


        #region textFeild methods

        [Export("textFieldShouldBeginEditing:")]
        public bool ShouldBeginEditing(UITextField textField)
        {
            if (responseList.Count > 0 && _notesTypeList.Count > 0)
            {
                if (textField == IBToDateTxt && string.IsNullOrEmpty(IBFromDateTxt.Text))
                {
                    IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                           IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSFromDateEmpty", "LSErrorTitle"));
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

                selectedFeild = textField;
                return true;
            }
            else
            {
                return false;
            }
        }

        [Export("textFieldDidEndEditing:")]
        public void EditingEnded(UITextField textField)
        {
            selectedUser = userPickerModel.selectedModel;
            SelectedType = eventPickerModel.selectedModel;
        }


        #endregion
    }
}

