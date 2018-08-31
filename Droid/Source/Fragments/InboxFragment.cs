
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using LucidX.Droid.Source.SharedPreference;
using Plugin.Connectivity;
using System;
using LucidX.Droid.Source.Adapters;
using LucidX.ResponseModels;
using System.Collections.Generic;
using LucidX.Droid.Source.CustomViews;
using LucidX.Webservices;
using LucidX.Droid.Source.Utilities;
using LucidX.Droid.Source.Global;
using Android.Support.V4.View;
using Android.Runtime;
using Android.Content;
using LucidX.Droid.Source.Activities;
using Newtonsoft.Json;

namespace LucidX.Droid.Source.Fragments
{
    public class InboxFragment : Fragment, View.IOnClickListener
    {
        #region "Constructor"
        public InboxFragment()
        {
        }
        #endregion

        private View view;
        private RecyclerView rvInbox;
        private SwipeRefreshLayout refresher;
        private TextView tvPullRefresh;
        private LinearLayoutManager layoutManager;
        private InboxAdapter mAdapter;
        private Android.App.Activity mActivity;
        private SharedPreferencesManager mSharedPreferencesManager;

        // It is for inbox, Draft, Sent items and Trash
        private int emailTypeId;


        #region "Functions"
        public static Fragment GetInstance(int emailTypeId, string toolbarTitle)
        {
            InboxFragment fragment = new InboxFragment();

            Bundle b = new Bundle();
            b.PutInt("emailTypeId", emailTypeId);
            b.PutString("title", toolbarTitle);
            fragment.Arguments = b;
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        /// <summary>
        /// Returns fragment alert screen view
        /// </summary>
        /// <param name="inflater"></param>-
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.fragment_inbox, container, false);
            HasOptionsMenu = true;
            mActivity = Activity;


            string title = Arguments.GetString("title");
            ((HomeActivity)mActivity).SetTitle(title);

            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);

            Init();
            return view;
        }

        /// <summary>
        /// Initializes the contents of the Activity's standard options menu
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="inflater"></param>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search, menu);
            try
            {
                var searchItem = menu.FindItem(Resource.Id.menu_search);

                var searvView = MenuItemCompat.GetActionView(searchItem);
                Android.Support.V7.Widget.SearchView searchView = searvView.JavaCast<Android.Support.V7.Widget.SearchView>();

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
              
                case Resource.Id.menu_new_mail:
                    Intent intent = new Intent(mActivity, typeof(ComposeEmailActivity));
                    mActivity.StartActivity(intent);
                    break;
            }
            return true;
        }

        /// <summary>
        /// Method use to initialize resources.
        /// </summary>
        private void Init()
        {
            rvInbox = (RecyclerView)view.FindViewById(Resource.Id.rvCompaign);

            // improve performance by indicating the list if fixed size.
            rvInbox.HasFixedSize = true;

            layoutManager = new LinearLayoutManager(Activity);
            rvInbox.SetLayoutManager(layoutManager);


            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

            tvPullRefresh = (TextView)view.FindViewById(Resource.Id.tvRefresh);

            refresher.Refresh += Refresher_Refresh;

            InfiniteScrollListener infiniteScrollListener = new InfiniteScrollListener(layoutManager, LoadMore);
            rvInbox.AddOnScrollListener(infiniteScrollListener);

        }
        /// <summary>
        /// Initalize alert send screen view
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            emailTypeId = Arguments.GetInt("emailTypeId");
            GetInboxList(emailTypeId);
        }

        private async void GetInboxList(int emailTypeId)
        {
            try
            {
                List<EmailResponse> responseList = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity, mActivity.Resources.GetString(Resource.String.loading));

                    responseList = await WebServiceMethods.InboxEmails(
                        mSharedPreferencesManager.GetString(ConstantsDroid.USER_ID_PREFERENCE, ""), emailTypeId);

                    SetInboxList(responseList);

                    CustomProgressDialog.HideProgressDialog();
                }
                else
                {
                    UtilityDroid.GetInstance().ShowAlertDialog(mActivity, Resources.GetString(Resource.String.error_alert_title),
                        Resources.GetString(Resource.String.alert_message_no_network_connection),
                        Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));
                }

                refresher.Refreshing = false;
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
        /// Method use for load more when user scroll to end of list.
        /// </summary>
        private void LoadMore()
        {
            //GetCampaignList(RecordType.Prev);
        }

        async private void Refresher_Refresh(object sender, System.EventArgs e)
        {
            // LOADING YOUR DATA.
            GetInboxList(emailTypeId);

        }

        /// Method use to set campaign list data into recycler view.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="recordType"></param>
        public void SetInboxList(List<EmailResponse> data)
        {
            if (mAdapter == null)
            {
                mAdapter = new InboxAdapter(Activity);
                mAdapter.ItemClick += MAdapter_ItemClick;
                mAdapter.SetData(data);
                rvInbox.SetAdapter(mAdapter);
            }
            else
            {

                mAdapter.NotifyDataSetChanged();
            }

            if (mAdapter.GetData() != null && mAdapter.GetData().Count > 0)
            {
                rvInbox.Visibility = ViewStates.Visible;
                tvPullRefresh.Visibility = ViewStates.Gone;
            }
            else
            {
                rvInbox.Visibility = ViewStates.Gone;
                tvPullRefresh.Visibility = ViewStates.Visible;
            }
        }

        public void MAdapter_ItemClick(object sender, int position)
        {
            try
            {
                List<EmailResponse> emailListResponse = mAdapter.GetData();
                EmailResponse emailResponseObj = emailListResponse[position];
                if (emailResponseObj.Unread)
                {
                    emailResponseObj.Unread = false;
                    emailListResponse[position] = emailResponseObj;
                    mAdapter.emailList = emailListResponse;
                    mAdapter.NotifyItemChanged(position);

                    // Call webservice for update read flag
                    WebServiceMethods.MarkReadEmail(emailResponseObj.MailId);
                }

                string emailResponseString = JsonConvert.SerializeObject(emailResponseObj);

                Intent intent = new Intent(mActivity, typeof(EmailDetailActivity));
                intent.PutExtra("emailResponseString", emailResponseString);
                intent.PutExtra("emailTypeId", emailTypeId);
                StartActivityForResult(intent, ConstantsDroid.EMAIL_DETAIL_REQUEST_CODE);
            }
            catch (Exception)
            {

            }
        }

        public void CallViewInboxEmailsWebservice(int emailTypeId)
        {
            try
            {

                if (CrossConnectivity.Current.IsConnected)
                {
                    GetInboxList(emailTypeId);
                }

                else
                {
                    UtilityDroid.GetInstance().ShowAlertDialog(mActivity,
                        Resources.GetString(Resource.String.error_alert_title),
                        Resources.GetString(Resource.String.alert_message_no_network_connection),
                        Resources.GetString(Resource.String.alert_cancel_btn),
                        Resources.GetString(Resource.String.alert_ok_btn));
                }

            }
            catch (Exception ex)
            {

            }
        }
        public void OnClick(View v)
        {

        }


        #endregion
    }
}