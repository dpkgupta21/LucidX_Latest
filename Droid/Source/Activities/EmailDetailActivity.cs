using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using LucidX.Droid.Source.SharedPreference;
using LucidX.Droid.Source.Utilities;
using LucidX.Constants;
using System;
using LucidX.Droid.Source.Global;
using LucidX.Webservices;
using Android.Webkit;
using Plugin.Connectivity;
using Android.Widget;
using Newtonsoft.Json;
using LucidX.ResponseModels;
using Android.Text;
using LucidX.Droid.Source.CustomViews;
using Android.Support.V7.App;
using Android.Graphics;
using System.Threading.Tasks;

namespace LucidX.Droid.Source.Activities
{
    [Activity(Label = "LucidX", Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class EmailDetailActivity : AppCompatActivity
    {

        /// <summary>
        /// The toolbar
        /// </summary>
        private Android.Support.V7.Widget.Toolbar toolbar;

        private Activity mActivity;

        private SharedPreferencesManager mSharedPreferencesManager;

        private EmailResponse emailResponseObj;

        private int emailTypeId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_email_detail);

            mActivity = this;

            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);

            try
            {
              
                Init();

                emailTypeId = Intent.GetIntExtra("emailTypeId", -1);
                string emailResonseString = Intent.GetStringExtra("emailResponseString");
                emailResponseObj = JsonConvert.DeserializeObject<EmailResponse>(emailResonseString);

                TextView txt_subject_val = FindViewById<TextView>(Resource.Id.txt_subject_val);
                TextView txt_img_lbl = FindViewById<TextView>(Resource.Id.txt_img_lbl);
                TextView txt_sender_name = FindViewById<TextView>(Resource.Id.txt_sender_name);

                txt_subject_val.Text = emailResponseObj.Subject;
                txt_sender_name.Text = emailResponseObj.SenderName;

                string lbl = emailResponseObj.SenderName.Substring(0, 1);
                txt_img_lbl.Text = lbl;



                GetEmailDetails(emailResponseObj.MailId,
                    mSharedPreferencesManager.GetString(ConstantsDroid.USER_ID_PREFERENCE, ""));
            }
            catch (Exception)
            {

            }

        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.delete, menu);

            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
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

        /// <summary>
        /// Init this instance.
        /// </summary>
        private void Init()
        {
            // Init toolbar
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.email_detail_title);
            ApplyFontForToolbarTitle();
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);


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


   
        private async void GetEmailDetails(string mailId, string userId)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity, null);

                    

                    string emailDetail = await WebServiceMethods.EmailDetail(mailId, userId);

                    if (emailDetail != null)
                    {
                        //WebView webview = FindViewById<WebView>(Resource.Id.webview);
                        // webview.Settings.JavaScriptEnabled = true;
                        // webview.LoadData(emailDetail, "text/html; charset=utf-8", "UTF-8");

                        TextView textview = FindViewById<TextView>(Resource.Id.webview);
                        textview.SetText(Html.FromHtml(emailDetail), TextView.BufferType.Spannable);

                        CustomProgressDialog.HideProgressDialog();
                    }
                    else
                    {
                        CustomProgressDialog.HideProgressDialog();
                        UtilityDroid.GetInstance().ShowAlertDialog(mActivity, Resources.GetString(Resource.String.error_alert_title),
                               Resources.GetString(Resource.String.alert_message_error),
                               Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));

                    }
                }
                else
                {
                    UtilityDroid.GetInstance().ShowAlertDialog(mActivity, Resources.GetString(Resource.String.error_alert_title),
                        Resources.GetString(Resource.String.alert_message_no_network_connection),
                        Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));

                }
            }
            catch (Exception)
            {
                CustomProgressDialog.HideProgressDialog();
                UtilityDroid.GetInstance().ShowAlertDialog(mActivity, Resources.GetString(Resource.String.error_alert_title),
                   Resources.GetString(Resource.String.alert_message_invalid_credentials),
                   Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));
            }
        }


        private async Task<bool> DeleteEmails()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                           GetString(Resource.String.processing_message));
                    bool isDelete = await WebServiceMethods.DeleteEmail(emailResponseObj.MailId,
                    mSharedPreferencesManager.GetString(ConstantsDroid.USER_ID_PREFERENCE, ""));
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
                                       bool isDeleted = await DeleteEmails();
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


        private void CallBackScreen()
        {
            Intent returnIntent = new Intent();
            returnIntent.PutExtra("emailTypeId", emailTypeId);
            SetResult(Result.Ok, returnIntent);
            Finish();
        }
    }

}

