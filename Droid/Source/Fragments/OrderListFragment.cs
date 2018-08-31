
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
using LucidX.Droid.Source.Models;
using Plugin.Connectivity;
using LucidX.Droid.Source.CustomViews;
using LucidX.Webservices;
using LucidX.Droid.Source.SharedPreference;
using LucidX.Droid.Source.Global;
using LucidX.ResponseModels;
using Newtonsoft.Json;

namespace LucidX.Droid.Source.Fragments
{
    /// <summary>
    /// Class OrderListFragment.
    /// </summary>
    public class OrderListFragment : Fragment
    {

        /// <summary>
        /// The tag
        /// </summary>
        private const string TAG = "OrderListFragment";
        /// <summary>
        /// The expandable view
        /// </summary>
        private ListView listView;
        /// <summary>
        /// The Orders list object
        /// </summary>
        private List<LedgerOrder> ledgerOrderList;

        /// <summary>
        /// The run list adapter
        /// </summary>
        private OrderAdapter mAdapter;
        /// <summary>
        /// The m activity
        /// </summary>
        private Activity mActivity;

        private View view;
        private TextView txt_from_date;
        private TextView txt_to_date;
        private TextView txt_no_orders;

        private DateTime fromDateTime = DateTime.Now;
        private DateTime toDateTime = DateTime.Now;

        private SharedPreferencesManager mSharedPreferencesManager;

        #region "Functions"
        public static Fragment GetInstance()
        {
            OrderListFragment fragment = new OrderListFragment();
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.fragment_order_list, container, false);
            HasOptionsMenu = true;

            mActivity = Activity;

            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);

            txt_no_orders = view.FindViewById<TextView>(
                Resource.Id.txt_no_orders);

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

            ((HomeActivity)mActivity).SetTitle(GetString(Resource.String.order_title));

            ImageView img_search = view.FindViewById<ImageView>(
             Resource.Id.img_search);
            img_search.Click += Img_search_Click;

            return view;
        }

        private void Img_search_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                CallWebserviceForOrdersList();
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

    



        /// <summary>
        /// Initializes the contents of the Activity's standard options menu
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="inflater"></param>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu_calendar, menu);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_add:
                    // Show Add event screen
                    Intent intent = new Intent(mActivity, typeof(AddOrderFirstActivity));
                    mActivity.StartActivityForResult(intent, ConstantsDroid.ORDERS_LIST_REQUEST_CODE);
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

            // call webservice for orders
            CallWebserviceForOrdersList();

        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int pos = e.Position;

            string orderObj = JsonConvert.SerializeObject(ledgerOrderList[pos]);

            Intent intent = new Intent(mActivity, typeof(AddOrderFirstActivity));
            intent.PutExtra("isEdit", true);
            intent.PutExtra("orderObj", orderObj);
            mActivity.StartActivity(intent);

            mActivity.OverridePendingTransition(Resource.Animation.animation_enter,
                        Resource.Animation.animation_leave);
        }


        /// <summary>
        /// Sets Calendar event list in listview using adapter,
        /// In case of no events "No Events" will be displayed
        /// </summary>
        /// <param name="CalendarListObj">List of run</param>
        private void InitailizeOrderListAdapter(List<LedgerOrder> ledgerOrderList)
        {
            try
            {
                if (ledgerOrderList != null && ledgerOrderList.Count > 0)
                {
                    listView.Visibility = ViewStates.Visible;
                    txt_no_orders.Visibility = ViewStates.Gone;
                    mAdapter = new OrderAdapter(ledgerOrderList, mActivity);
                    listView.Adapter = mAdapter;

                }
                else
                {
                    listView.Visibility = ViewStates.Gone;
                    txt_no_orders.Visibility = ViewStates.Visible;
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
                        txt_to_date.Text = Utils.Utilities.DateIntoWebserviceFormat(time);
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
                    txt_from_date.Text = Utils.Utilities.DateIntoWebserviceFormat(time);
                }
                catch (Exception ex)
                {

                }
            }, fromDateTime);
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }


        public async void CallWebserviceForOrdersList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    ledgerOrderList = await WebServiceMethods.GetOrders(mSharedPreferencesManager.GetString(ConstantsDroid.USER_ID_PREFERENCE, ""),
                        txt_from_date.Text, txt_to_date.Text);

                    InitailizeOrderListAdapter(ledgerOrderList);

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


        #endregion
    }
}


