
using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using LucidX.ResponseModels;
using LucidX.Droid.Source.Adapters;
using LucidX.Droid.Source.Activities;
using Android.Support.V4.App;
using Activity = Android.App.Activity;
using LucidX.Droid.Source.CustomDialogFragment;
using LucidX.Droid.Source.Picker;
using LucidX.Droid.Source.Utilities;
using LucidX.Droid.Source.CustomSpinner.Adapter;
using LucidX.Droid.Source.Models;
using Newtonsoft.Json;
using LucidX.Droid.Source.CustomSpinner.Model;
using Plugin.Connectivity;
using LucidX.Droid.Source.CustomViews;
using LucidX.Webservices;
using LucidX.Droid.Source.Global;
using System.Threading.Tasks;
using System.Linq;
using LucidX.Droid.Source.SharedPreference;

namespace LucidX.Droid.Source.Fragments
{
    /// <summary>
    /// Class CalendarFragment.
    /// </summary>
    public class CalendarFragment : Fragment
    {
        /// <summary>
        /// Calendar type dialog fragment request code
        /// </summary>
        public const int CALENDAR_TYPE_DIALOG_REQUEST_CODE = 1001;
        /// <summary>
        /// The tag
        /// </summary>
        private const string TAG = "CalendarFragment";
        /// <summary>
        /// The list view
        /// </summary>
        private ListView listView;

        /// <summary>
        /// The Calendar event list object
        /// </summary>
        private List<CalendarEventResponse> _calendarList;

        /// <summary>
        /// The run list adapter
        /// </summary>
        private CalendarEventListAdapter mAdapter;
        /// <summary>
        /// The m activity
        /// </summary>
        private Activity mActivity;

        private View view;

        private LinearLayout linear_user_and_type;
        private RelativeLayout relative_selected_layout;
        private RelativeLayout relative_date_layout;
        private TextView txt_from_date;
        private TextView txt_to_date;
        private TextView txt_no_events;
        private TextView txt_calendar_type;
        private TextView txt_user_calendar_val;

        private DateTime fromDateTime = DateTime.Now;
        private DateTime toDateTime = DateTime.Now;



        private Spinner spin_users;
        private SpinnerItemModel _selectedUsersItem;
        private SpinnerAdapter _userSpinnerAdapter;
        private List<SpinnerItemModel> _userSpinnerItemModelList;

        private List<NotesTypeResponse> _notesTypeList;
        private SharedPreferencesManager mSharedPreferencesManager;
        private LoginResponse loginResponse;
        #region "Functions"
        public static Fragment GetInstance()
        {
            CalendarFragment fragment = new CalendarFragment();
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.fragment_calendar_expandable, container, false);
            HasOptionsMenu = true;

            mActivity = Activity;
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                      GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);
            loginResponse = mSharedPreferencesManager.GetLoginResponse();
            linear_user_and_type = view.FindViewById<LinearLayout>(
                Resource.Id.linear_user_and_type);
            relative_date_layout = view.FindViewById<RelativeLayout>(
               Resource.Id.relative_date_layout);
            relative_selected_layout = view.FindViewById<RelativeLayout>(
             Resource.Id.relative_selected_layout);
            txt_no_events = view.FindViewById<TextView>(
             Resource.Id.txt_no_events);
            txt_user_calendar_val = view.FindViewById<TextView>(
             Resource.Id.txt_user_calendar_val);
            ImageView img_search = view.FindViewById<ImageView>(
               Resource.Id.img_search);
            img_search.Click += Img_search_Click;

            ImageView img_return = view.FindViewById<ImageView>(
               Resource.Id.img_return);
            img_return.Click += Img_return_Click;

            txt_calendar_type = view.FindViewById<TextView>(
                Resource.Id.txt_calendar_type);
            txt_calendar_type.Click += Txt_calendar_type_Click;

            txt_from_date = view.FindViewById<TextView>(
                Resource.Id.txt_from_date);
            txt_from_date.Click += Edt_from_date_Click;

            txt_to_date = view.FindViewById<TextView>(
                Resource.Id.txt_to_date);
            txt_to_date.Click += Edt_to_date_Click;

            DateTime now = DateTime.Now;

            DateTime startDate = new DateTime(now.Year, now.Month, 1);
            string startDateString = startDate.ToString(UtilityDroid.CALENDAR_DATE_FORMAT);
            txt_from_date.Text = startDateString.Replace("-", "/");

            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            string endDateString = endDate.ToString(UtilityDroid.CALENDAR_DATE_FORMAT);
            txt_to_date.Text = endDateString.Replace("-", "/");

            spin_users = view.FindViewById<Spinner>(
              Resource.Id.spin_users);

            ((HomeActivity)mActivity).SetTitle(GetString(Resource.String.calendar_title));

            // Set users in spinner
            InitUserSpinnerValues();

            //GetNotesTypes();
            return view;
        }

        private void Img_search_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                relative_selected_layout.Visibility = ViewStates.Visible;
                linear_user_and_type.Visibility = ViewStates.Gone;
                relative_date_layout.Visibility = ViewStates.Gone;
                txt_user_calendar_val.Text = _selectedUsersItem.TEXT;

                // Call webservice for get calendar events
                CallCalendarEventsWebservice();
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




        ///// <summary>
        ///// Init values for User spinner
        ///// </summary>
        //private async void GetNotesTypes()
        //{
        //    try
        //    {

        //        if (CrossConnectivity.Current.IsConnected)
        //        {
        //            CustomProgressDialog.ShowProgDialog(mActivity,
        //                mActivity.Resources.GetString(Resource.String.loading));


        //            CustomProgressDialog.HideProgressDialog();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        CustomProgressDialog.HideProgressDialog();
        //    }
        //}

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
              
                    _notesTypeList = await WebServiceMethods.ShowNotesType();

                    GetNotesTypeResult(_notesTypeList);

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
        /// Set Account Code spinner adapter
        /// </summary>
        private void SetUserSpinnerAdapter()
        {
            _userSpinnerAdapter = new SpinnerAdapter(mActivity, Resource.Layout.spinner_row_item_lay,
                   _userSpinnerItemModelList);
            spin_users.Adapter = _userSpinnerAdapter;

            // Initialize listener for spinner
            InitializeListeners();
        }



        private void InitializeListeners()
        {
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
                        spin_users.SetSelection (i);
                    }
                    else
                    {
                        _userSpinnerItemModelList[i].STATE = false;
                    }
                }
                _userSpinnerAdapter.NotifyDataSetChanged();

                CallCalendarEventsWebservice();
            };
        }


        /// <summary>
        /// Initializes the contents of the Activity's standard options menu
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="inflater"></param>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu_calendar, menu);
            try
            {

            }
            catch (Exception e)
            {

            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_add:
                    // Show Add event screen
                    Intent intent = new Intent(mActivity, typeof(AddCalendarEventActivity));
                    intent.PutExtra("isAddEvent", true);
                    mActivity.StartActivityForResult(intent, ConstantsDroid.CALENDAR_LIST_REQUEST_CODE);
                    mActivity.OverridePendingTransition(Resource.Animation.animation_enter,
                                Resource.Animation.animation_leave);
                    break;
            }
            return true;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            listView = view.FindViewById<ListView>(Resource.Id.listview);
            listView.ItemClick += ListView_ItemClick;
        }


        public void CallCalendarEventsWebservice()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    GetCalendarEventsList();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txt_calendar_type.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(txt_from_date.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(txt_to_date.Text))
            {
                return false;
            }

            return true;
        }


        private async void GetCalendarEventsList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {

                    List<NotesTypeResponse> selectedNotesList = _notesTypeList.Where(x => x.IsSelected == true).ToList();
                    string calendarTypeString = "";
                    foreach (NotesTypeResponse notesType in selectedNotesList)
                    {
                        calendarTypeString += notesType.NotesTypeId + ",";
                    }
                    string calendarType = calendarTypeString.Substring(0, calendarTypeString.Length - 1);
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    _calendarList = await WebServiceMethods.GetCalendarEvents(_selectedUsersItem.EXTRA_TEXT,
                        calendarType, txt_from_date.Text, txt_to_date.Text);



                    InitailizeCalendarEventListAdapter(_calendarList);

                    CustomProgressDialog.HideProgressDialog();
                }
                else
                {
                    UtilityDroid.GetInstance().ShowAlertDialog(mActivity, Resources.GetString(Resource.String.error_alert_title),
                        Resources.GetString(Resource.String.alert_message_no_network_connection),
                        Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));
                }
            }
            catch (Exception ex)
            {
                CustomProgressDialog.HideProgressDialog();
                UtilityDroid.GetInstance().ShowAlertDialog(mActivity, Resources.GetString(Resource.String.error_alert_title),
                   Resources.GetString(Resource.String.alert_message_error),
                   Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));
            }


        }

        /// <summary>
        /// Sets Calendar event list in listview using adapter,
        /// In case of no events "No Events" will be displayed
        /// </summary>
        /// <param name="CalendarListObj">List of run</param>
        private void InitailizeCalendarEventListAdapter(List<CalendarEventResponse> eventsList)
        {
            try
            {
                if (eventsList != null && eventsList.Count > 0)
                {
                    listView.Visibility = ViewStates.Visible;
                    txt_no_events.Visibility = ViewStates.Gone;
                    mAdapter = new CalendarEventListAdapter(eventsList, mActivity);
                    listView.Adapter = mAdapter;
                }
                else
                {
                    txt_no_events.Visibility = ViewStates.Visible;
                    listView.Visibility = ViewStates.Gone;
                }
            }
            catch (Exception e)
            {
            }
        }


        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int pos = e.Position;
            CalendarEventResponse eventObj = _calendarList[pos];
            string eventObjString = JsonConvert.SerializeObject(eventObj);
            Intent intent = new Intent(mActivity, typeof(AddCalendarEventActivity));
            intent.PutExtra("isAddEvent", false);
            intent.PutExtra("eventObj", eventObjString);


            mActivity.StartActivityForResult(intent, ConstantsDroid.CALENDAR_LIST_REQUEST_CODE);
            mActivity.OverridePendingTransition(Resource.Animation.animation_enter,
                        Resource.Animation.animation_leave);
        }


        private void Img_return_Click(object sender, EventArgs e)
        {
            relative_selected_layout.Visibility = ViewStates.Gone;
            linear_user_and_type.Visibility = ViewStates.Visible;
            relative_date_layout.Visibility = ViewStates.Visible;
        }


        private void Txt_calendar_type_Click(object sender, EventArgs e)
        {
            FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();

            //remove fragment from backstack if any exists
            Fragment fragmentPrev = FragmentManager.FindFragmentByTag("dialog");
            if (fragmentPrev != null)
                fragmentTransaction.Remove(fragmentPrev);

            fragmentTransaction.AddToBackStack(null);

            string notesTypeString = JsonConvert.SerializeObject(_notesTypeList);
            //create and show the dialog
            CustomDialogFrag dialogFragment = CustomDialogFrag.NewInstance(notesTypeString);
            dialogFragment.SetTargetFragment(this, CALENDAR_TYPE_DIALOG_REQUEST_CODE);
            dialogFragment.Show(fragmentTransaction, "dialog");
        }

        public void GetNotesTypeResult(List<NotesTypeResponse> _notesTypeList)
        {
            this._notesTypeList = _notesTypeList;

            List<NotesTypeResponse> selectedNotesList = _notesTypeList.Where(x => x.IsSelected == true).ToList();
            if (selectedNotesList != null && selectedNotesList.Count != 0)
            {
                txt_calendar_type.Text = "Journal (" + selectedNotesList.Count + ")";
                TextView txt_calendar_type_val = view.FindViewById<TextView>(
                    Resource.Id.txt_calendar_type_val);
                txt_calendar_type_val.Text = "Journal (" + selectedNotesList.Count + ")";
            }
        }



        private void Edt_to_date_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                try
                {
                    if (time.Date < fromDateTime.Date)
                    {
                        UtilityDroid.GetInstance().ShowAlertDialog(mActivity,
                           Resources.GetString(Resource.String.error_alert_title),
                           Resources.GetString(Resource.String.alert_message_not_less_than_from_date),
                           Resources.GetString(Resource.String.alert_cancel_btn),
                           Resources.GetString(Resource.String.alert_ok_btn));
                    }
                    else
                    {
                        toDateTime = time;
                        string toDate = Utils.Utilities.DateIntoWebserviceFormat(time);
                        txt_to_date.Text = toDate;
                    }
                }
                catch (Exception ex)
                {
                }
            }, toDateTime);
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void Edt_from_date_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                try
                {

                    fromDateTime = time;
                    string fromDate = Utils.Utilities.DateIntoWebserviceFormat(time);
                    txt_from_date.Text = fromDate;

                }
                catch (Exception ex)
                {
                }
            }, fromDateTime);
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        #endregion
    }
}


