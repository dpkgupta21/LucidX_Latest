
using Android.OS;
using Android.Views;
using LucidX.Droid.Source.SharedPreference;
using LucidX.Droid.Source.Utilities;
using System;
using Android.Support.V7.App;
using Android.App;
using Android.Webkit;
using Java.IO;
using Android.Widget;
using Android.Graphics;
using Android.Content;
using Android.Runtime;

namespace LucidX.Droid.Source.Activities
{
    [Activity(Label = "LucidX", Icon = "@mipmap/icon", Theme = "@style/AppTheme",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ComposeEmailActivity : AppCompatActivity
    {

        /// <summary>
        /// The toolbar
        /// </summary>
        private Android.Support.V7.Widget.Toolbar toolbar;
        private Activity mActivity;
        private SharedPreferencesManager mSharedPreferencesManager;

        private const int ATTACHMENT_REQUEST_CODE = 1001;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_compose_email);

            mActivity = this;

            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);

            Init();
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_compose_email, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    break;
                case Resource.Id.menu_attachment:
                    Intent intent = new Intent(Intent.ActionGetContent);
                    intent.SetType("*/*");
                    StartActivityForResult(intent, ATTACHMENT_REQUEST_CODE);
                    break;
                case Resource.Id.menu_send:
                    break;
            }
            return true;
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        private void Init()
        {
            // Init toolbar
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.compose_email_title);
            ApplyFontForToolbarTitle();
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            //WebView mwebview = FindViewById<WebView>(Resource.Id.LocalWebView);
            //mwebview.Settings.JavaScriptEnabled = true;        
            //mwebview.LoadUrl("file:///android_asset/jQuery-TE_v.1.4.0"+File.Separator+ 
            //"demo" + File.Separator + "demo.html"); //new.html is html file 
        }


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                switch (requestCode)
                {
                    case ATTACHMENT_REQUEST_CODE:
                        string pathHolder = data.Data.Path;
                        Toast.MakeText(mActivity, pathHolder, ToastLength.Short).Show();
                        break;
                }
            }
            else
            {
                Toast.MakeText(mActivity, "Not attached", ToastLength.Short).Show();
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

    }

}

