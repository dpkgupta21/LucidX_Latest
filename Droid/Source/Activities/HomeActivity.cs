
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using LucidX.Droid.Source.Fragments;
using LucidX.Droid.Source.Adapters;
using System;
using LucidX.Webservices;
using LucidX.Droid.Source.SharedPreference;
using LucidX.Droid.Source.Global;
using Plugin.Connectivity;
using LucidX.ResponseModels;
using LucidX.Droid.Source.Utilities;
using LucidX.Droid.Source.CustomViews;
using System.Threading.Tasks;
using static Android.Widget.AdapterView;
using LucidX.Constants;
using System.Collections.Generic;
using LucidX.Droid.Source.Models;
using Android.Content;
using Android.Graphics;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Runtime;
using Fragment = Android.Support.V4.App.Fragment;

namespace LucidX.Droid.Source.Activities
{

    [Activity(Label = "HomeActivity", Theme = "@style/AppTheme", Icon = "@mipmap/icon",
        ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustResize)]
    public class HomeActivity : AppCompatActivity,
        ExpandableListView.IOnChildClickListener, ExpandableListView.IOnGroupClickListener,
        ExpandableListView.IOnGroupExpandListener
    {
        private DrawerLayout drawerLayout;


        /// <summary>
        /// The drawer toggle
        /// </summary>
        private MyActionBarDrawerToggle drawerToggle;
        /// <summary>
        /// The toolbar
        /// </summary>
        private Toolbar toolbar;

        //private MenuAdapter mAdapter;
        private SideMenuListExpandableAdapter mAdapter;

        private Activity mActivity;

        private SharedPreferencesManager mSharedPreferencesManager;

        private RelativeLayout drawerList;

        private List<GroupMenuModel> menuList;

        private ExpandableListView menuListView;

        private bool doubleBackToExitPressedOnce = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_home);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            mActivity = this;

            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);


            // Init toolbar
            toolbar = FindViewById<Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.inbox_title);


            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            SetupSideMenu();

            bool isOrderListScreen = Intent.GetBooleanExtra("addOrder", false);

            if (isOrderListScreen)
            {
                ShowScreen(0, 0);

            }
            else
            {
                Android.Support.V4.App.Fragment fragment = InboxFragment.GetInstance(WebserviceConstants.
                    INBOX_EMAIL_TYPE_ID, GetString(Resource.String.inbox_title));
                AddFrament(fragment, false);
                // mAdapter.SetSelectedPosition(0);
            }
        }



        public void SetTitle(string titleMsg)
        {
            SupportActionBar.Title = titleMsg;
            ApplyFontForToolbarTitle(toolbar);
        }


        public void ApplyFontForToolbarTitle(Toolbar toolbar)
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


        private async Task GetEmailCounts()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    EmailCountResponse emailCount = await WebServiceMethods.
                        EmailCount(mSharedPreferencesManager.GetString(
                            ConstantsDroid.USER_ID_PREFERENCE, ""));

                    if (emailCount != null)
                    {
                        mSharedPreferencesManager.PutCountResponse(emailCount);
                    }
                }
            }
            catch (Exception e)
            {
                CustomProgressDialog.HideProgressDialog();
                UtilityDroid.GetInstance().ShowAlertDialog(mActivity, Resources.GetString(Resource.String.error_alert_title),
                   Resources.GetString(Resource.String.alert_message_error),
                   Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));


            }
        }
        /// <summary>
        /// Setups the side menu.
        /// </summary>
        /// <returns>The side menu.</returns>
        private async void SetupSideMenu()
        {
            LoginResponse loginResponseObj = mSharedPreferencesManager.GetLoginResponse();

            drawerList = FindViewById<RelativeLayout>(Resource.Id.left_drawer);
            TextView txt_user_name = FindViewById<TextView>(Resource.Id.txt_user_name);
            TextView txt_user_email = FindViewById<TextView>(Resource.Id.txt_user_email);

            txt_user_name.Text = loginResponseObj.Name;
            txt_user_email.Text = loginResponseObj.UserEmail;

            menuListView = FindViewById<ExpandableListView>(Resource.Id.listview);

            menuListView.SetOnChildClickListener(this);
            menuListView.SetOnGroupClickListener(this);
            menuListView.SetOnGroupExpandListener(this);

            menuList = GetExpandableMenuItem();
            mAdapter = new SideMenuListExpandableAdapter(mActivity, menuList);

            // set up the drawer's list view with items and click listener
            menuListView.SetAdapter(mAdapter);

            drawerToggle = new MyActionBarDrawerToggle(this,
                drawerLayout, toolbar,
                Resource.String.open_drawer,
                Resource.String.close_drawer);
            drawerLayout.AddDrawerListener(drawerToggle);

            SupportActionBar.SetDisplayShowHomeEnabled(true);

            drawerToggle.SyncState();
            drawerToggle.DrawerIndicatorEnabled = true;

            // Call Email Count webservice
            await GetEmailCounts();
        }


        private List<SideMenuModel> GetSideMenuItem()
        {
            List<SideMenuModel> menuList = new List<SideMenuModel>();
            string[] mMenuNames = Resources.GetStringArray(Resource.Array.menu_array);
            string[] mMenuIcons = Resources.GetStringArray(Resource.Array.menu_icon_array);
            string[] mMenuLabels = Resources.GetStringArray(Resource.Array.menu_label_array);

            for (int i = 0; i < mMenuNames.Length; i++)
            {
                SideMenuModel menuModel = new SideMenuModel();
                menuModel.menuName = mMenuNames[i];
                menuModel.menuIcon = mMenuIcons[i];
                menuModel.menuLabel = mMenuLabels[i];
                if (string.IsNullOrEmpty(mMenuNames[i]) && string.IsNullOrEmpty(mMenuLabels[i]))
                {
                    menuModel.isMenuSeperator = true;
                }
                else
                {
                    menuModel.isMenuSeperator = false;
                }
                menuList.Add(menuModel);
            }
            return menuList;
        }

        private List<GroupMenuModel> GetExpandableMenuItem()
        {
            List<GroupMenuModel> menuList = new List<GroupMenuModel>();
            string[] groupMenuNames = Resources.GetStringArray(Resource.Array.group_menu_array);


            for (int i = 0; i < groupMenuNames.Length; i++)
            {
                GroupMenuModel groupMenuModel = new GroupMenuModel();
                groupMenuModel.menuName = groupMenuNames[i];
                List<ChildMenuModel> childMenuList = new List<ChildMenuModel>();
                string[] childMenuNames = null;
                string[] childMenuIcons = null;
                if (groupMenuNames[i].Equals(GetString(Resource.String.menu_mail_lbl)))
                {
                    childMenuNames = Resources.GetStringArray(Resource.Array.mail_child_menu_array);
                    childMenuIcons = Resources.GetStringArray(Resource.Array.mail_child_menu_icon_array);

                }
                else if (groupMenuNames[i].Equals(GetString(Resource.String.menu_calendar_lbl)))
                {
                    childMenuNames = Resources.GetStringArray(Resource.Array.calendar_child_menu_array);
                    childMenuIcons = Resources.GetStringArray(Resource.Array.calendar_child_menu_icon_array);


                }
                else if (groupMenuNames[i].Equals(GetString(Resource.String.menu_order_lbl)))
                {
                    childMenuNames = Resources.GetStringArray(Resource.Array.order_child_menu_array);
                    childMenuIcons = Resources.GetStringArray(Resource.Array.order_child_menu_icon_array);


                }
                else if (groupMenuNames[i].Equals(GetString(Resource.String.menu_notes_lbl)))
                {
                    childMenuNames = Resources.GetStringArray(Resource.Array.notes_child_menu_array);
                    childMenuIcons = Resources.GetStringArray(Resource.Array.notes_child_menu_icon_array);


                }
                else
                {
                    childMenuNames = new string[0];
                    childMenuIcons = new string[0];
                }
                if (childMenuNames != null)
                {
                    for (int j = 0; j < childMenuNames.Length; j++)
                    {
                        ChildMenuModel childMenuModel = new ChildMenuModel();
                        childMenuModel.submenuName = childMenuNames[j];
                        childMenuModel.submenuIcon = childMenuIcons[j];
                        childMenuList.Add(childMenuModel);
                    }
                    groupMenuModel.submenuList = childMenuList;
                }

                menuList.Add(groupMenuModel);
            }
            return menuList;
        }


        /// <summary>
        /// Get current visible fragment
        /// </summary>
        /// <returns>Current Fragment</returns>
        public Android.Support.V4.App.Fragment GetCurrentFragment()
        {
            return SupportFragmentManager.FindFragmentById(Resource.Id.frame_container);
        }

        /// <summary>
        /// Class MyActionBarDrawerToggle.
        /// </summary>
        internal class MyActionBarDrawerToggle : ActionBarDrawerToggle
        {
            /// <summary>
            /// The owner
            /// </summary>
            HomeActivity owner;

            /// <summary>
            /// Initializes a new instance of the <see cref="MyActionBarDrawerToggle"/> class.
            /// </summary>
            /// <param name="activity">The activity.</param>
            /// <param name="layout">The layout.</param>
            /// <param name="toolbar">The toolbar.</param>
            /// <param name="openRes">The open resource.</param>
            /// <param name="closeRes">The close resource.</param>
            public MyActionBarDrawerToggle(HomeActivity activity,
                DrawerLayout layout,
                Android.Support.V7.Widget.Toolbar toolbar,
                int openRes,
                int closeRes)
                    : base(activity, layout, toolbar, openRes, closeRes)
            {
                owner = activity;
            }

            /// <summary>
            /// Called when [drawer closed].
            /// </summary>
            /// <param name="drawerView">The drawer view.</param>
            public override void OnDrawerClosed(View drawerView)
            {
                //owner.ActionBar.Title = owner.Title;
                //owner.InvalidateOptionsMenu();
            }

            /// <summary>
            /// Called when [drawer opened].
            /// </summary>
            /// <param name="drawerView">The drawer view.</param>
            public override void OnDrawerOpened(View drawerView)
            {
                EmailCountResponse response = owner.mSharedPreferencesManager.
                    GetCountResponse();
                owner.mAdapter.emailCount = response;
                owner.mAdapter.NotifyDataSetChanged();

            }
        }


        public class MenuHandler : Handler
        {

            private const string TAG = "MenuHandler";
            public static WeakReference<HomeActivity> homeActivity;

            public MenuHandler(HomeActivity activity)
            {
                homeActivity = new WeakReference<HomeActivity>(activity);
            }


            public override void HandleMessage(Message msg)
            {
                HomeActivity activity = null;
                homeActivity.TryGetTarget(out activity);

            }

        }


        /// <summary>
        /// Adds the frament.
        /// </summary>
        /// <param name="fragment">The fragment.</param>
        /// <param name="addBackstack">if set to <c>true</c> [add backstack].</param>
        public void AddFrament(Android.Support.V4.App.Fragment fragment, bool addBackstack)
        {
            var ft = SupportFragmentManager.BeginTransaction();
            if (addBackstack)
            {
                ft.Add(Resource.Id.frame_container, fragment, fragment.Class.Name);
                ft.AddToBackStack(fragment.Class.Name);
            }
            else
            {
                ft.Replace(Resource.Id.frame_container, fragment, fragment.Class.Name);
            }
            ft.CommitAllowingStateLoss();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {

        }

        private void ShowScreen(int groupPosition, int childPosition)
        {
            Intent intent = null;
            Android.Support.V4.App.Fragment fragment = null;
            ToogleDrawer();
            switch (groupPosition)
            {
                // For Mail
                case 0:
                    switch (childPosition)
                    {
                        case 0:
                            fragment = InboxFragment.GetInstance(
                                WebserviceConstants.INBOX_EMAIL_TYPE_ID,
                                GetString(Resource.String.inbox_title));
                            AddFrament(fragment, false);
                            break;
                        case 1:
                            fragment = InboxFragment.GetInstance(
                                WebserviceConstants.DRAFT_EMAIL_TYPE_ID,
                                GetString(Resource.String.draft_title));
                            AddFrament(fragment, false);
                            break;
                        case 2:
                            fragment = InboxFragment.GetInstance(
                                WebserviceConstants.SENT_EMAIL_TYPE_ID,
                                GetString(Resource.String.sent_title));
                            AddFrament(fragment, false);
                            break;
                        case 3:
                            fragment = InboxFragment.GetInstance(
                                WebserviceConstants.TRASH_EMAIL_TYPE_ID,
                                GetString(Resource.String.trash_title));
                            AddFrament(fragment, false);
                            break;
                    }
                    break;
                // For Calendar
                case 1:
                    switch (childPosition)
                    {
                        //For Calendar Event Detail Screen     
                        case 0:
                            fragment = CalendarFragment.GetInstance();
                            AddFrament(fragment, false);
                            break;
                        //For Add Event Screen     
                        case 1:
                            intent = new Intent(mActivity, typeof(AddCalendarEventActivity));
                            intent.PutExtra("isAddEvent", true);
                            StartActivityForResult(intent, ConstantsDroid.ADD_CALENDAR_EVENT_REQUEST_CODE);
                            OverridePendingTransition(Resource.Animation.animation_enter,
                                        Resource.Animation.animation_leave);
                            break;

                    }
                    break;
                //For Order
                case 2:
                    switch (childPosition)
                    {
                        //For Order list Screen     
                        case 0:
                            fragment = OrderListFragment.GetInstance();
                            AddFrament(fragment, false);
                            break;
                        //For Order list Screen     
                        case 1:
                            fragment = OrderListFragment.GetInstance();
                            AddFrament(fragment, false);
                            break;
                        //For Add Order Screen     
                        case 2:
                            intent = new Intent(mActivity, typeof(AddOrderFirstActivity));
                            StartActivityForResult(intent, ConstantsDroid.ADD_ORDER_REQUEST_CODE);
                            OverridePendingTransition(Resource.Animation.animation_enter,
                                        Resource.Animation.animation_leave);
                            break;
                        //For Order list Screen     
                        case 3:
                            fragment = OrderListFragment.GetInstance();
                            AddFrament(fragment, false);
                            break;
                        //For Order list Screen     
                        case 4:
                            fragment = OrderListFragment.GetInstance();
                            AddFrament(fragment, false);
                            break;
                    }
                    break;
                // For Notes
                case 3:
                    switch (childPosition)
                    {
                        //For Notes list Screen     
                        case 0:
                            fragment = NotesListFragment.GetInstance();
                            AddFrament(fragment, false);
                            break;
                        //For Notes list Screen     
                        case 1:
                            fragment = NotesListFragment.GetInstance();
                            AddFrament(fragment, false);

                            break;
                        //For Add Notes Screen     
                        case 2:
                            // Show Add Notes screen
                            intent = new Intent(mActivity, typeof(AddNotesActivity));
                            intent.PutExtra("isAddNote", true);
                            StartActivityForResult(intent, ConstantsDroid.ADD_NOTES_REQUEST_CODE);
                            OverridePendingTransition(Resource.Animation.animation_enter,
                                        Resource.Animation.animation_leave);
                            break;
                        ////For Notes list Screen     
                        //case 3:
                        //    fragment = NotesListFragment.GetInstance();
                        //    AddFrament(fragment, false);

                        //    break;
                    }
                    break;

            }


            //mAdapter.SetSelectedPosition(position);

        }
        /// <summary>
        /// Toogles side menu bar.
        /// </summary>
        /// <returns>The drawer.</returns>
        private void ToogleDrawer()
        {
            if (drawerLayout.IsDrawerOpen(drawerList))
            {
                drawerLayout.CloseDrawer(drawerList);
            }

        }

        public bool OnChildClick(ExpandableListView parent, View clickedView,
            int groupPosition, int childPosition, long id)
        {
            ShowScreen(groupPosition, childPosition);
            mAdapter.selectedGroupPosition = groupPosition;
            mAdapter.selectedChildPosition = childPosition;
            mAdapter.NotifyDataSetChanged();
            return false;
        }

        public bool OnGroupClick(ExpandableListView parent, View clickedView, int groupPosition, long id)
        {
            switch (groupPosition)
            {
                case 4:
                    Finish();
                    mSharedPreferencesManager.PutCountResponse(null);
                    mSharedPreferencesManager.PutLoginResponse(null);
                    StartActivity(typeof(LoginActivity));
                    break;
            }

            return false;
        }

        public void OnGroupExpand(int groupPosition)
        {
            if (groupPosition != mAdapter.selectedGroupPosition)
                menuListView.CollapseGroup(mAdapter.selectedGroupPosition);

        }

        protected override void OnActivityResult(int requestCode,
            [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                switch (requestCode)
                {
                    case ConstantsDroid.ADD_NOTES_REQUEST_CODE:

                        ShowScreen(3, 0);
                        break;
                    case ConstantsDroid.EMAIL_DETAIL_REQUEST_CODE:

                        var frag0 = SupportFragmentManager.FindFragmentById(Resource.Id.frame_container);
                        int emailType = data.GetIntExtra("emailTypeId", -1);

                        if (emailType != -1)
                        {
                            if (frag0 is InboxFragment)
                            {
                                ((InboxFragment)frag0).CallViewInboxEmailsWebservice(emailType);
                            }
                        }
                        break;

                    case ConstantsDroid.NOTES_LIST_REQUEST_CODE:

                        var frag = SupportFragmentManager.FindFragmentById(Resource.Id.frame_container);
                        if (frag is NotesListFragment)
                        {
                            ((NotesListFragment)frag).CallViewNotesWebservice();
                        }
                        break;
                    case ConstantsDroid.ADD_CALENDAR_EVENT_REQUEST_CODE:

                        ShowScreen(1, 0);
                        break;

                    case ConstantsDroid.CALENDAR_LIST_REQUEST_CODE:

                        var frag1 = SupportFragmentManager.FindFragmentById(Resource.Id.frame_container);
                        if (frag1 is CalendarFragment)
                        {
                            ((CalendarFragment)frag1).CallCalendarEventsWebservice();
                        }
                        break;
                    case ConstantsDroid.ADD_ORDER_REQUEST_CODE:

                        ShowScreen(2, 0);
                        break;

                    case ConstantsDroid.ORDERS_LIST_REQUEST_CODE:

                        var frag2 = SupportFragmentManager.FindFragmentById(Resource.Id.frame_container);
                        if (frag2 is OrderListFragment)
                        {
                            ((OrderListFragment)frag2).CallWebserviceForOrdersList();
                        }
                        break;
                }

            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

  

        /// <summary>
        /// Presses back button
        /// </summary>
        public override void OnBackPressed()
        {
            try
            {
                Fragment fragment = GetCurrentFragment();
                if (fragment is InboxFragment) {
                    // Work for asking user press again for confirmation of exiting from application.
                    if (doubleBackToExitPressedOnce)
                    {
                        Finish();
                        Intent intent = new Intent(Intent.ActionMain);
                        intent.AddCategory(Intent.CategoryHome);
                        intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                        StartActivity(intent);
                        return;
                    }

                    this.doubleBackToExitPressedOnce = true;
                    Toast.MakeText(this, "Press again to Exit", ToastLength.Short).Show();

                    RunnableAnonymousInnerClassHelper runnable = new RunnableAnonymousInnerClassHelper(this);

                    Handler handler = new Handler();
                    handler.PostDelayed(runnable, 2000);
                }else
                {
                    ShowScreen(0, 0);
                }
            }
            catch (Exception e)
            {

            }

        }

        /// <summary>
        /// Class which handles 
        /// doubleBackToExitPressedOnce= false
        /// if user dont click the back button in 2 seconds
        /// </summary>
        private class RunnableAnonymousInnerClassHelper : Java.Lang.Object, Java.Lang.IRunnable
        {
            /// <summary>
            /// The outer instance
            /// </summary>
            private readonly HomeActivity outerInstance;

            /// <summary>
            /// Initializes a new instance of the <see cref="RunnableAnonymousInnerClassHelper"/> class.
            /// </summary>
            /// <param name="outerInstance">The outer instance.</param>
            public RunnableAnonymousInnerClassHelper(HomeActivity outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            /// <summary>
            /// Runs this instance.
            /// </summary>
            public void Run()
            {
                outerInstance.doubleBackToExitPressedOnce = false;
            }
        }
    }
}