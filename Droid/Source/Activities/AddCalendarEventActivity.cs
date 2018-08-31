
using Android.OS;
using Android.Views;
using LucidX.Droid.Source.SharedPreference;
using LucidX.Droid.Source.Utilities;
using System;
using Android.Support.V7.App;
using LucidX.Droid.Source.Picker;
using Android.Widget;
using Android.App;
using LucidX.Droid.Source.CustomSpinner.Model;
using LucidX.Droid.Source.CustomSpinner.Adapter;
using System.Collections.Generic;
using Android.Graphics;
using LucidX.Droid.Source.CustomViews;
using LucidX.ResponseModels;
using Plugin.Connectivity;
using LucidX.Webservices;
using LucidX.RequestModels;
using LucidX.Constants;
using Android.Content;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace LucidX.Droid.Source.Activities
{
    [Activity(Label = "LucidX", Icon = "@mipmap/icon", Theme = "@style/AppTheme",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AddCalendarEventActivity : AppCompatActivity
    {
        /// <summary>
        /// Calendar type dialog fragment request code
        /// </summary>
        public const int CALENDAR_TYPE_DIALOG_REQUEST_CODE = 1001;
        /// <summary>
        /// The toolbar
        /// </summary>
        private Android.Support.V7.Widget.Toolbar toolbar;

        private Activity mActivity;

        private SharedPreferencesManager mSharedPreferencesManager;

        private bool isAddEvent;
        private CalendarEventResponse eventObj = null;

        private TextView txt_subject_val;
        private TextView txt_description_val;
        private TextView txt_start_date_val;
        private TextView txt_end_date_val;
        private TextView txt_start_time_val;
        private TextView txt_end_time_val;
        private DateTime startDateTime = DateTime.Now;
        private DateTime endDateTime = DateTime.Now;

        private TimeSpan startTimeSpan;
        private TimeSpan endTimeSpan;

        private Spinner spin_calendar_type;
        private SpinnerItemModel _selectedCalendarTypeItem;
        private SpinnerAdapter _calendarTypeSpinnerAdapter;
        private List<SpinnerItemModel> _calendarTypeSpinnerItemModelList;
        private int _selectedCalendarTypeItemPosition;

        private Spinner spin_users;
        private SpinnerItemModel _selectedUsersItem;
        private SpinnerAdapter _userSpinnerAdapter;
        private List<SpinnerItemModel> _userSpinnerItemModelList;
        private int _selectedUserItemPosition;
        private LoginResponse loginResponse;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_add_calendar_event);

            mActivity = this;

            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);
            loginResponse = mSharedPreferencesManager.GetLoginResponse();
            isAddEvent = Intent.GetBooleanExtra("isAddEvent", false);
            if (!isAddEvent)
            {
                string eventObjString = Intent.GetStringExtra("eventObj");
                eventObj = JsonConvert.DeserializeObject<CalendarEventResponse>(eventObjString);

            }
            try
            {
                Init();
                if (!isAddEvent)
                {
                    SetEventDetails();
                }
            }
            catch (Exception)
            {

            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.delete, menu);


            IMenuItem menuItem = menu.FindItem(Resource.Id.menu_delete);
            if (isAddEvent)
            {
                menuItem.SetVisible(false);
            }
            else
            {
                menuItem.SetVisible(true);
            }
            return true;
        }

        private void SetEventDetails()
        {
            try
            {
                txt_description_val.Text = eventObj.Details;
                txt_subject_val.Text = eventObj.Subject;
                txt_start_date_val.Text = eventObj.DateStart.ToString(UtilityDroid.DATE_FORMAT);
                txt_end_date_val.Text = eventObj.DateEnd.ToString(UtilityDroid.DATE_FORMAT);
                txt_start_time_val.Text = eventObj.DateStart.ToString(UtilityDroid.DISPLAY_TIME_FORMAT);
                txt_end_time_val.Text = eventObj.DateEnd.ToString(UtilityDroid.DISPLAY_TIME_FORMAT);


            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        private void Init()
        {
            // Init toolbar
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);

            // Set toolbar title Add or Edit according to condition
            SetToolbarTitle();
            // Set font for title
            ApplyFontForToolbarTitle();

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            txt_start_date_val = FindViewById<TextView>(Resource.Id.txt_start_date_val);
            txt_start_date_val.Click += Txt_start_date_val_Click;

            txt_end_date_val = FindViewById<TextView>(Resource.Id.txt_end_date_val);
            txt_end_date_val.Click += Txt_end_date_val_Click;

            txt_start_time_val = FindViewById<TextView>(Resource.Id.txt_start_time_val);
            txt_start_time_val.Click += Txt_start_time_val_Click;

            txt_end_time_val = FindViewById<TextView>(Resource.Id.txt_end_time_val);
            txt_end_time_val.Click += Txt_end_time_val_Click;

            txt_subject_val = FindViewById<TextView>(Resource.Id.txt_subject_val);

            txt_description_val = FindViewById<TextView>(Resource.Id.txt_description_val);

            spin_users = FindViewById<Spinner>(Resource.Id.spin_users);
            spin_calendar_type = FindViewById<Spinner>(Resource.Id.spin_calendar_type);

            Button btn_cancel = FindViewById<Button>(Resource.Id.btn_cancel);
            btn_cancel.Click += Btn_cancel_Click;

            Button btn_save = FindViewById<Button>(Resource.Id.btn_save);
            btn_save.Click += Btn_save_Click;

            // Set users in spinner
            InitUserSpinnerValues();
            // Set CalendarTypes in spinner;
            InitCalendarTypeSpinnerValues();



            // Initialize listener for spinner
            InitializeListeners();
        }

        private async void Btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        CustomProgressDialog.ShowProgDialog(mActivity,
                            GetString(Resource.String.saving_message));


                        AddCalendarEventsAPIParams addEventsParam = new AddCalendarEventsAPIParams
                        {
                            entryId = (!isAddEvent) ? (eventObj.EntryId) : "0",
                            compCode = "84451",
                            accCode = "1718904",
                            notesTypeId = _selectedCalendarTypeItem.EXTRA_TEXT,
                            entryTypeId = 0,
                            startDate = txt_start_date_val.Text + " " + txt_start_time_val.Text,
                            endDate = txt_end_date_val.Text + " " + txt_end_time_val.Text,
                            subject = txt_subject_val.Text,
                            details = txt_description_val.Text,
                            assignedTo = _selectedUsersItem.EXTRA_TEXT,
                            summaryItemId = "999999999",
                            isCompleted = false,
                            isPublic = false,
                            accountId = "216",
                            connectionName = WebserviceConstants.CONNECTION_NAME
                        };

                        bool isUpdated = await WebServiceMethods.AddCalendarEvents(addEventsParam);
                        CustomProgressDialog.HideProgressDialog();

                        CallBackScreen();
                    }
                    else
                    {
                        UtilityDroid.GetInstance().ShowAlertDialog(mActivity,
                            Resources.GetString(Resource.String.error_alert_title),
                            Resources.GetString(Resource.String.alert_message_no_network_connection),
                            Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));

                    }
                }
                else
                {
                    UtilityDroid.GetInstance().ShowAlertDialog(mActivity,
                        Resources.GetString(Resource.String.error_alert_title),
                        Resources.GetString(Resource.String.alert_message_fill_all_Details),
                        Resources.GetString(Resource.String.alert_cancel_btn),
                        Resources.GetString(Resource.String.alert_ok_btn));
                }

            }
            catch (Exception ex)
            {
                CustomProgressDialog.HideProgressDialog();
            }
        }

        private void CallBackScreen()
        {
            Intent returnIntent = new Intent();
            returnIntent.PutExtra("result", "AddEvents");
            SetResult(Result.Ok, returnIntent);
            Finish();
        }


        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txt_start_date_val.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(txt_start_time_val.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(txt_end_date_val.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(txt_end_time_val.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(txt_subject_val.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(txt_description_val.Text))
            {
                return false;
            }

            return true;
        }

        private void SetToolbarTitle()
        {
            if (isAddEvent)
            {
                SupportActionBar.SetTitle(Resource.String.add_event_title);
            }
            else
            {
                SupportActionBar.SetTitle(Resource.String.edit_event_title);
            }

        }

        public void ApplyFontForToolbarTitle()
        {
            for (int i = 0; i < toolbar.ChildCount; i++)
            {
                View view = toolbar.GetChildAt(i);
                if (view is TextView)
                {
                    TextView tv = (TextView)view;
                    Typeface titleFont = Typeface.
                       CreateFromAsset(mActivity.Assets, "Fonts/century-gothic.ttf");
                    if (tv.Text.Equals(toolbar.Title))
                    {
                        tv.Typeface = titleFont;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Init values for Calendar Type spinner
        /// </summary>
        private async void InitCalendarTypeSpinnerValues()
        {
            try
            {
                List<NotesTypeResponse> responseList = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    responseList = await WebServiceMethods.ShowNotesType();

                    CustomProgressDialog.HideProgressDialog();
                }

                _calendarTypeSpinnerItemModelList = new List<SpinnerItemModel>();

                for (int i = 0; i < responseList.Count; i++)
                {
                    SpinnerItemModel item = new SpinnerItemModel
                    {
                        Id = (i + 1) + "",
                        TEXT = responseList[i].NotesTypeName,
                        STATE = false,
                        EXTRA_TEXT = responseList[i].NotesTypeId + "",
                    };
                    if (eventObj != null && eventObj.NotesTypeId.Equals(responseList[i].NotesTypeId + ""))
                    {
                        _selectedCalendarTypeItemPosition = i;
                    }

                    _calendarTypeSpinnerItemModelList.Add(item);
                }
                SetCalendarTypeSpinnerAdapter();
            }
            catch (Exception e)
            {
                CustomProgressDialog.HideProgressDialog();
            }
        }




        /// <summary>
        /// Set Calendar Type spinner adapter
        /// </summary>
        private void SetCalendarTypeSpinnerAdapter()
        {
            _calendarTypeSpinnerAdapter = new SpinnerAdapter(mActivity, Resource.Layout.spinner_row_item_lay,
                   _calendarTypeSpinnerItemModelList);
            spin_calendar_type.Adapter = _calendarTypeSpinnerAdapter;
            spin_calendar_type.SetSelection(_selectedCalendarTypeItemPosition);
        }



        /// <summary>
        /// Init values for User spinner
        /// </summary>
        private async void InitUserSpinnerValues()
        {
            try
            {

                List<RefUsersResponse> responseList = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));
                
                    responseList = await WebServiceMethods.ShowRefUsers(loginResponse.UserCompCode);

                    CustomProgressDialog.HideProgressDialog();
                }

                _userSpinnerItemModelList = new List<SpinnerItemModel>();

                for (int i = 0; i < responseList.Count; i++)
                {
                    SpinnerItemModel item = new SpinnerItemModel
                    {
                        Id = (i + 1) + "",
                        TEXT = responseList[i].UserName,
                        STATE = false,
                        EXTRA_TEXT = responseList[i].UserID
                    };
                    if (eventObj != null && eventObj.AssignedTo.Equals(responseList[i].UserID + ""))
                    {
                        _selectedUserItemPosition = i;
                    }
                    _userSpinnerItemModelList.Add(item);
                }
                SetUserSpinnerAdapter();
            }
            catch (Exception e)
            {
                CustomProgressDialog.HideProgressDialog();
            }
        }

        /// <summary>
        /// Set User spinner adapter
        /// </summary>
        private void SetUserSpinnerAdapter()
        {
            _userSpinnerAdapter = new SpinnerAdapter(mActivity, Resource.Layout.spinner_row_item_lay,
                   _userSpinnerItemModelList);
            spin_users.Adapter = _userSpinnerAdapter;
            spin_users.SetSelection(_selectedUserItemPosition);
        }



        private void InitializeListeners()
        {
            // Calendar Type Spinner
            spin_calendar_type.ItemSelected += (sender, args) =>
            {
                _selectedCalendarTypeItem = _calendarTypeSpinnerItemModelList[args.Position];

                _calendarTypeSpinnerItemModelList[args.Position].STATE = true;
                // update spinner item list state
                for (int i = 0; i < _calendarTypeSpinnerItemModelList.Count; i++)
                {
                    if (i == args.Position)
                    {
                        _calendarTypeSpinnerItemModelList[i].STATE = true;
                    }
                    else
                    {
                        _calendarTypeSpinnerItemModelList[i].STATE = false;
                    }
                }
                _calendarTypeSpinnerAdapter.NotifyDataSetChanged();
            };


            // User Spinner
            spin_users.ItemSelected += (sender, args) =>
            {
                _selectedUsersItem = _userSpinnerItemModelList[args.Position];
                _userSpinnerItemModelList[args.Position].STATE = true;
                // update spinner item list state
                for (int i = 0; i < _userSpinnerItemModelList.Count; i++)
                {
                    if (_userSpinnerItemModelList[i].EXTRA_TEXT.Equals(loginResponse.UserId))
                    {
                        _selectedUsersItem = _userSpinnerItemModelList[i];
                        _userSpinnerItemModelList[i].STATE = true;
                        spin_users.SetSelection(i);
                    }
                    else
                    {
                        _userSpinnerItemModelList[i].STATE = false;
                    }
                }
                _userSpinnerAdapter.NotifyDataSetChanged();
            };
        }




        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            Finish();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    CallBackScreen();
                    break;
                case Resource.Id.menu_delete:
                    ShowAlertDialog(Resources.GetString(Resource.String.error_alert_title),
                        GetString(Resource.String.alert_message_delete_notes_confirmation),
                        GetString(Resource.String.alert_cancel_btn),
                        GetString(Resource.String.alert_ok_btn)
                        );
                    break;
            }
            return true;
        }



        private async Task<bool> DeleteCRMNotes()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                           GetString(Resource.String.processing_message));
                    bool isDelete = await WebServiceMethods.DeleteEvents(eventObj.EntryId);
                    CustomProgressDialog.HideProgressDialog();
                    return isDelete;
                }

            }
            catch (Exception ex)
            {
                CustomProgressDialog.HideProgressDialog();

            }
            return false;
        }

        /// <summary>
        /// Show Alert Dialog
        /// </summary>
        /// <param name="activity">Activity instance</param>
        /// <param name="title">title of alert dialog</param>
        /// <param name="messsage">message to show alert dialog</param>
        /// <returns>Dialog</returns>
        public Dialog ShowAlertDialog(string title, string messsage,
            string positivebtn, string negativeBtn)
        {
            try
            {

                Dialog dialog = null;
                dialog = new CustomDialog().CreateDialog(mActivity,
                                title,
                                messsage,
                                negativeBtn, positivebtn,
                                   new EventHandler(async delegate (Object o, EventArgs a)
                                   {

                                       if (dialog != null)
                                       {
                                           dialog.Dismiss();
                                           dialog = null;
                                       }
                                       bool isDeleted = await DeleteCRMNotes();
                                       if (isDeleted)
                                       {

                                           CallBackScreen();
                                       }
                                   }),
                                 new EventHandler(delegate (Object o, EventArgs a)
                                 {
                                     if (dialog != null)
                                     {
                                         dialog.Dismiss();
                                         dialog = null;
                                     }
                                 }),
                              true, true);

                dialog.SetCancelable(true);
                dialog.Show();
                return dialog;


            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Shows Date picker for start Date time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_start_date_val_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                try
                {

                    startDateTime = date;
                    txt_start_date_val.Text = Utils.Utilities.DateIntoWebserviceFormat(date);

                }
                catch (Exception ex)
                {
                }
            }, startDateTime);
            frag.Show(SupportFragmentManager, DatePickerFragment.TAG);
        }


        /// <summary>
        /// Shows Date picker for end Date time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_end_date_val_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                try
                {
                    if (date.Date < startDateTime.Date)
                    {
                        UtilityDroid.GetInstance().ShowAlertDialog(mActivity,
                           Resources.GetString(Resource.String.error_alert_title),
                           Resources.GetString(Resource.String.alert_message_not_less_than_from_date),
                           Resources.GetString(Resource.String.alert_cancel_btn),
                           Resources.GetString(Resource.String.alert_ok_btn));
                    }
                    else
                    {
                        endDateTime = date;
                        txt_end_date_val.Text = Utils.Utilities.DateIntoWebserviceFormat(date);
                    }
                }
                catch (Exception ex)
                {
                }
            }, endDateTime);
            frag.Show(SupportFragmentManager, DatePickerFragment.TAG);
        }

        private void Txt_end_time_val_Click(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (TimeSpan time)
            {
                endTimeSpan = time;
                txt_end_time_val.Text = endTimeSpan.ToString();


            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void Txt_start_time_val_Click(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (TimeSpan time)
            {
                startTimeSpan = time;
                txt_start_time_val.Text = startTimeSpan.ToString();


            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }


    }

}

