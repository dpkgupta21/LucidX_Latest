
using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using LucidX.Droid.Source.Adapters;
using LucidX.Droid.Source.Activities;
using Android.Support.V4.App;
using Activity = Android.App.Activity;
using LucidX.Droid.Source.Picker;
using LucidX.Droid.Source.Utilities;
using LucidX.Droid.Source.CustomSpinner.Model;
using LucidX.Droid.Source.CustomSpinner.Adapter;
using LucidX.ResponseModels;
using Plugin.Connectivity;
using LucidX.Droid.Source.CustomViews;
using LucidX.Webservices;
using LucidX.Droid.Source.SharedPreference;
using Newtonsoft.Json;
using Android.Runtime;
using LucidX.Droid.Source.Global;

namespace LucidX.Droid.Source.Fragments
{
    /// <summary>
    /// Class NotesListFragment.
    /// </summary>
    public class NotesListFragment : Fragment
    {

        /// <summary>
        /// The tag
        /// </summary>
        private const string TAG = "NotesListFragment";
        /// <summary>
        /// The expandable view
        /// </summary>
        private ListView listView;
        /// <summary>
        /// The run list adapter
        /// </summary>
        private NotesListAdapter mAdapter;
        /// <summary>
        /// The m activity
        /// </summary>
        private Activity mActivity;

        private View view;
        private TextView txt_from_date;
        private TextView txt_to_date;
        private TextView txt_no_notes;

        private Spinner spin_account_code;
        private SpinnerItemModel _selectedAccountCodeItem;
        private SpinnerAdapter _accountCodeSpinnerAdapter;
        private List<SpinnerItemModel> _accountCodeSpinnerItemModelList;

        private Spinner spin_current_entity;
        private SpinnerItemModel _selectedCurrentEntitysItem;
        private SpinnerAdapter _entitySpinnerAdapter;
        private List<SpinnerItemModel> _entitySpinnerItemModelList;


        private DateTime fromDateTime = DateTime.Now;
        private DateTime toDateTime = DateTime.Now;

        private SharedPreferencesManager mSharedPreferencesManager;
        private LoginResponse objLoginResponse;

        private List<CrmNotesResponse> responseList = null;
        #region "Functions"
        public static Fragment GetInstance()
        {
            NotesListFragment fragment = new NotesListFragment();
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.fragment_notes_list, container, false);
            HasOptionsMenu = true;

            mActivity = Activity;

            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);
            objLoginResponse = mSharedPreferencesManager.GetLoginResponse();

            spin_current_entity = view.FindViewById<Spinner>(
              Resource.Id.spin_current_entity);
            spin_account_code = view.FindViewById<Spinner>(
             Resource.Id.spin_account_code);

            txt_no_notes = view.FindViewById<TextView>(
             Resource.Id.txt_no_notes);

            txt_from_date = view.FindViewById<TextView>(
                Resource.Id.txt_from_date);
            txt_from_date.Click += Edt_from_date_Click;

            txt_to_date = view.FindViewById<TextView>(
                Resource.Id.txt_to_date);
            txt_to_date.Click += Edt_to_date_Click;

            DateTime now = DateTime.Now;

            DateTime startDate = new DateTime(now.Year, now.Month, 1);
            string startDateString = startDate.ToString(UtilityDroid.DATE_FORMAT);
            txt_from_date.Text = startDateString.Replace("-", "/");

            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            string endDateString = endDate.ToString(UtilityDroid.DATE_FORMAT);
            txt_to_date.Text = endDateString.Replace("-", "/");

            ImageView img_search = view.FindViewById<ImageView>(Resource.Id.img_search);
            img_search.Click += Img_search_Click;

            ((HomeActivity)mActivity).SetTitle(GetString(Resource.String.notes_title));

            // Set Current Entity in Spinner
            InitEntitySpinnerValues();

            // Initialize listener for spinner
            InitializeListeners();

            return view;
        }

        private void Img_search_Click(object sender, EventArgs e)
        {
            try
            {
                CallViewNotesWebservice();

            }
            catch (Exception ex)
            {

            }
        }

        public void CallViewNotesWebservice()
        {
            try
            {
                if (ValidateForm())
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        GetCrmNotesList();
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

            }
        }
        private bool ValidateForm()
        {

            if (string.IsNullOrEmpty(txt_from_date.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(txt_to_date.Text))
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Init values for Current Entity spinner
        /// </summary>
        private async void InitEntitySpinnerValues()
        {
            try
            {
                //List<string> entityItems = new List<string> { "Select Current Entity", "Entity1",
                //    "Entity2", "Entity3"};
                List<EntityCodesResponse> responseList = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    responseList = await WebServiceMethods.GetEntityCode(Convert.ToInt32(objLoginResponse.UserId), objLoginResponse.UserCompCode);

                    CustomProgressDialog.HideProgressDialog();
                }

                _entitySpinnerItemModelList = new List<SpinnerItemModel>();

                for (int i = 0; i < responseList.Count; i++)
                {
                    SpinnerItemModel item = new SpinnerItemModel
                    {
                        Id = (i + 1) + "",
                        TEXT = responseList[i].CompName + "",
                        EXTRA_TEXT = responseList[i].CompCode + "",
                        STATE = false
                    };

                    _entitySpinnerItemModelList.Add(item);
                }

                SetEntitySpinnerAdapter();
            }
            catch (Exception e)
            {
                CustomProgressDialog.HideProgressDialog();
            }
        }




        /// <summary>
        /// Set Current Entity spinner adapter
        /// </summary>
        private void SetEntitySpinnerAdapter()
        {
            _entitySpinnerAdapter = new SpinnerAdapter(mActivity, Resource.Layout.spinner_row_item_lay,
                   _entitySpinnerItemModelList);
            spin_current_entity.Adapter = _entitySpinnerAdapter;
        }



        /// <summary>
        /// Init values for Account Code spinner
        /// </summary>
        private async void InitAccountCodeSpinnerValues()
        {
            try
            {
                //List<string> accountCodeItems = new List<string> { "Select Account Code", "Account Code1",
                //    "Account Code2", "Account Code2" };
                List<AccountCodesResponse> responseList = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    responseList = await WebServiceMethods.GetAccountCodes(_selectedCurrentEntitysItem.EXTRA_TEXT);

                    CustomProgressDialog.HideProgressDialog();
                }

                _accountCodeSpinnerItemModelList = new List<SpinnerItemModel>();

                for (int i = 0; i < responseList.Count; i++)
                {
                    SpinnerItemModel item = new SpinnerItemModel
                    {
                        Id = (i + 1) + "",
                        TEXT = responseList[i].AccountName,
                        EXTRA_TEXT = responseList[i].AccountId ,
                        SECOND_EXTRA_TEXT= responseList[i].AccountCode,
                        STATE = false
                    };
                    _accountCodeSpinnerItemModelList.Add(item);
                }

                SetAccountCodeSpinnerAdapter();
            }
            catch (Exception e)
            {
                CustomProgressDialog.HideProgressDialog();
            }
        }

        /// <summary>
        /// Set Account Code spinner adapter
        /// </summary>
        private void SetAccountCodeSpinnerAdapter()
        {
            _accountCodeSpinnerAdapter = new SpinnerAdapter(mActivity, Resource.Layout.spinner_row_item_lay,
                   _accountCodeSpinnerItemModelList);
            spin_account_code.Adapter = _accountCodeSpinnerAdapter;
        }



        private void InitializeListeners()
        {
            // Current Entity Spinner
            spin_current_entity.ItemSelected += (sender, args) =>
            {
                _selectedCurrentEntitysItem = _entitySpinnerItemModelList[args.Position];

                _entitySpinnerItemModelList[args.Position].STATE = true;
                // update spinner item list state
                for (int i = 0; i < _entitySpinnerItemModelList.Count; i++)
                {
                    if (i == args.Position)
                    {
                        _entitySpinnerItemModelList[i].STATE = true;
                    }
                    else
                    {
                        _entitySpinnerItemModelList[i].STATE = false;
                    }
                }
                _entitySpinnerAdapter.NotifyDataSetChanged();

                // Set Account Code in Spinner
                InitAccountCodeSpinnerValues();


            };


            // Account Code Spinner
            spin_account_code.ItemSelected += (sender, args) =>
            {
                _selectedAccountCodeItem = _accountCodeSpinnerItemModelList[args.Position];
                _accountCodeSpinnerItemModelList[args.Position].STATE = true;
                // update spinner item list state
                for (int i = 0; i < _accountCodeSpinnerItemModelList.Count; i++)
                {
                    if (i == args.Position)
                    {
                        _accountCodeSpinnerItemModelList[i].STATE = true;
                    }
                    else
                    {
                        _accountCodeSpinnerItemModelList[i].STATE = false;
                    }
                }
                _accountCodeSpinnerAdapter.NotifyDataSetChanged();

                CallViewNotesWebservice();
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
                var searchItem = menu.FindItem(Resource.Id.menu_search);

                var searvView = Android.Support.V4.View.MenuItemCompat.GetActionView(searchItem);
                Android.Support.V7.Widget.SearchView searchView = searvView.
                    JavaCast<Android.Support.V7.Widget.SearchView>();

                searchView.QueryTextChange += (sender, args) =>
                {
                    string search = args.NewText;
                    mAdapter.GetFilteredList(search);
                    //if (string.IsNullOrEmpty(search))
                    //{
                    //    adapter.ResetSearch();
                    //}
                    //else
                    //{
                    //    adapter.filter.InvokeFilter(search);
                    //}
                };
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
                    // Show Add Notes screen
                    Intent intent = new Intent(mActivity, typeof(AddNotesActivity));
                    intent.PutExtra("isAddNote", true);
                    mActivity.StartActivityForResult(intent, ConstantsDroid.NOTES_LIST_REQUEST_CODE);
                    mActivity.OverridePendingTransition(Resource.Animation.animation_enter,
                                Resource.Animation.animation_leave);
                    break;
            }
            return true;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            listView = view.FindViewById<ListView>(Resource.Id.listview_order);
            listView.ItemClick += ListView_ItemClick;

        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int pos = e.Position;
            CrmNotesResponse noteObj = responseList[pos];
            string noteObjString = JsonConvert.SerializeObject(noteObj);
            Intent intent = new Intent(mActivity, typeof(AddNotesActivity));
            intent.PutExtra("isAddNote", false);
            intent.PutExtra("noteObj", noteObjString);
            intent.PutExtra("entityCode", _selectedCurrentEntitysItem.EXTRA_TEXT);
            intent.PutExtra("accountId", _selectedAccountCodeItem.EXTRA_TEXT);
            intent.PutExtra("accountCode", _selectedAccountCodeItem.SECOND_EXTRA_TEXT);

            mActivity.StartActivityForResult(intent, ConstantsDroid.NOTES_LIST_REQUEST_CODE);
            mActivity.OverridePendingTransition(Resource.Animation.animation_enter,
                        Resource.Animation.animation_leave);
        }



        private async void GetCrmNotesList()
        {
            try
            {

                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    responseList = await WebServiceMethods.ShowNotes(_selectedCurrentEntitysItem.EXTRA_TEXT,
                        _selectedAccountCodeItem.SECOND_EXTRA_TEXT, txt_from_date.Text, txt_to_date.Text);

                    InitailizeNotesListAdapter(responseList);

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
        private void InitailizeNotesListAdapter(List<CrmNotesResponse> notesList)
        {
            try
            {
                if (notesList != null && notesList.Count > 0)
                {
                    listView.Visibility = ViewStates.Visible;
                    txt_no_notes.Visibility = ViewStates.Gone;
                    mAdapter = new NotesListAdapter(notesList, mActivity);
                    listView.Adapter = mAdapter;
                }
                else
                {
                    txt_no_notes.Visibility = ViewStates.Visible;
                    listView.Visibility = ViewStates.Gone;
                }
            }
            catch (Exception e)
            {
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
                        string toDate = time.ToString(UtilityDroid.DATE_FORMAT);
                        txt_to_date.Text = toDate.Replace("-", "/");
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
                    string fromDate = time.ToString(UtilityDroid.DATE_FORMAT);
                    txt_from_date.Text = fromDate.Replace("-", "/");
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


