
using Android.OS;
using Android.Views;
using LucidX.Droid.Source.SharedPreference;
using LucidX.Droid.Source.Utilities;
using System;
using Android.Support.V7.App;
using Android.Widget;
using Android.App;
using Android.Content;
using System.Collections.Generic;
using Android.Graphics;
using LucidX.ResponseModels;
using Newtonsoft.Json;
using LucidX.Droid.Source.Fragments;
using LucidX.Droid.Source.Listeners;
using Plugin.Connectivity;
using LucidX.Droid.Source.CustomViews;
using LucidX.Webservices;

namespace LucidX.Droid.Source.Activities
{
    [Activity(Label = "LucidX", Icon = "@mipmap/icon", Theme = "@style/AppTheme",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AddOrderFirstActivity : AppCompatActivity, OrderResponseListener
    {

        /// <summary>
        /// The toolbar
        /// </summary>
        private Android.Support.V7.Widget.Toolbar toolbar;
        private Activity mActivity;
        private SharedPreferencesManager mSharedPreferencesManager;
        public LedgerOrder LedgerOrderObj { get; set; }
        // public OrdersResponse orderObj { get; set; }
        public bool isEdit { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Window.RequestFeature(WindowFeatures.NoTitle);

                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.activity_add_first_order);

                mActivity = this;

                /// Shared Preference manager
                mSharedPreferencesManager = UtilityDroid.GetInstance().
                           GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);

                LedgerOrderObj = new LedgerOrder();

                isEdit = Intent.GetBooleanExtra("isEdit", false);
                string orderObjString = Intent.GetStringExtra("orderObj");

                if (orderObjString != null)
                {
                    LedgerOrderObj = JsonConvert.DeserializeObject<LedgerOrder>(orderObjString);
                    if (isEdit)
                    {
                        GetLedgerItems(LedgerOrderObj.CompCode, LedgerOrderObj.JournalNo);
                    }
                }



                Init();

                DisplayFragment(isEdit);
            }
            catch (Exception e)
            {

            }
        }

        private async void GetLedgerItems(int compCode, int journalNo)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    List<LedgerOrderItem> ledgerOrderItem = await WebServiceMethods.GetLedgerOrderItems(compCode, journalNo);

                    LedgerOrderObj.LedgerOrderItems = ledgerOrderItem;

                    CustomProgressDialog.HideProgressDialog();

                }

            }
            catch (Exception e)
            {
                CustomProgressDialog.HideProgressDialog();
            }

        }

        private void DisplayFragment(bool isEdit)
        {
            Android.Support.V4.App.Fragment fragment = AddOrderFirstFragment.GetInstance(isEdit);
            Android.Support.V4.App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.frame_container, fragment, fragment.Class.Name);
            ft.Commit();
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        private void Init()
        {
            // Init toolbar
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.add_order_title);
            ApplyFontForToolbarTitle();
            // SupportActionBar.SetDisplayHomeAsUpEnabled(true);


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

        public void GetOrderResponse(string orderObj)
        {
            throw new NotImplementedException();
        }



        public override void OnBackPressed()
        {
            FragmentManager fm = FragmentManager;
            if (fm.BackStackEntryCount > 0)
            {
                fm.PopBackStack();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        private void CallBackScreen()
        {
            Intent returnIntent = new Intent();
            SetResult(Android.App.Result.Ok, returnIntent);
            Finish();
        }
    }

}

