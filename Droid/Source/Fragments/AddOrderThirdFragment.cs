
using Android.OS;
using Android.Views;
using System;
using Android.Support.V4.App;
using Android.Widget;
using Activity = Android.App.Activity;
using Android.Content;
using LucidX.Droid.Source.Utilities;
using LucidX.Droid.Source.Activities;
using LucidX.Droid.Source.SharedPreference;
using LucidX.ResponseModels;
using LucidX.Webservices;
using Plugin.Connectivity;
using LucidX.Droid.Source.CustomViews;
using LucidX.Droid.Source.Global;

namespace LucidX.Droid.Source.Fragments
{

    public class AddOrderThirdFragment : Fragment
    {


        private Activity mActivity;

        private EditText edt_vat_val;
        private EditText edt_net_val;
        private EditText edt_gross_val;
        private View view;

        private SharedPreferencesManager mSharedPreferencesManager;


        private LedgerOrder ledgerOrderObj;

        public static Fragment GetInstance()
        {
            AddOrderThirdFragment fragment = new AddOrderThirdFragment();
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            mActivity = Activity;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.activity_add_third_order, container, false);

            mActivity = Activity;

            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);

            ledgerOrderObj = ((AddOrderFirstActivity)mActivity).LedgerOrderObj;

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            try
            {
                Init();

                ShowInEditMode();
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        private void Init()
        {
            edt_vat_val = view.FindViewById<EditText>(Resource.Id.edt_vat_val);
            edt_net_val = view.FindViewById<EditText>(Resource.Id.edt_net_val);
            edt_gross_val = view.FindViewById<EditText>(Resource.Id.edt_gross_val);

            Button btn_ok = view.FindViewById<Button>(Resource.Id.btn_ok);
            btn_ok.Click += Btn_ok_Click;

            Button btn_cancel = view.FindViewById<Button>(Resource.Id.btn_cancel);
            btn_cancel.Click += Btn_cancel_Click;

        }

        private void ShowInEditMode()
        {         
            if (ledgerOrderObj != null &&
                ledgerOrderObj.LedgerOrderItems != null &&
                ledgerOrderObj.LedgerOrderItems.Count != 0)
            {
                decimal netAmount = 0;
                decimal vatAmount = 0;
                decimal grossAmount = 0;
 
                foreach (LedgerOrderItem orderItem in ledgerOrderObj.LedgerOrderItems)
                {
                    netAmount += orderItem.BaseAmount;
                    vatAmount += orderItem.TaxAmount;

                }
                grossAmount = netAmount + vatAmount;

                edt_net_val.Text = netAmount + "";
                edt_vat_val.Text = vatAmount + "";
                edt_gross_val.Text = grossAmount + "";


            }
        }
        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            mActivity.Finish();
            Intent intent = new Intent(mActivity, typeof(HomeActivity));
            intent.PutExtra("addOrder", true);
            StartActivity(intent);
            mActivity.OverridePendingTransition(Resource.Animation.animation_enter,
                        Resource.Animation.animation_leave);
        }

        private async void Btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        CustomProgressDialog.ShowProgDialog(mActivity,
                            mActivity.Resources.GetString(Resource.String.loading));
                        BeforeCallingSaveLedgerOrders();
                        bool isLedgerSaved = await WebServiceMethods.SaveLedgerOrdersNew(ledgerOrderObj);
                        CustomProgressDialog.HideProgressDialog();

                        if (isLedgerSaved)
                        {
                            CallBackScreen();
                        }
                    }
                    else
                    {
                        UtilityDroid.GetInstance().ShowAlertDialog(mActivity, Resources.GetString(Resource.String.error_alert_title),
                       Resources.GetString(Resource.String.alert_message_no_network_connection),
                       Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));

                    }
                }
                else
                {

                    UtilityDroid.GetInstance().ShowAlertDialog(mActivity, Resources.GetString(Resource.String.error_alert_title),
                           Resources.GetString(Resource.String.alert_message_fill_all_details),
                           Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));
                }
            }
            catch (Exception ex)
            {
                CustomProgressDialog.HideProgressDialog();
            }
        }
       
        private void BeforeCallingSaveLedgerOrders()
        {
            ledgerOrderObj.PresetCode = " ";
            ledgerOrderObj.TabID = 0;
            ledgerOrderObj.BaseAmount = Convert.ToDecimal(edt_gross_val.Text.Trim());
            ledgerOrderObj.ProcessedBy = Convert.ToInt32(mSharedPreferencesManager.
                GetString(ConstantsDroid.USER_ID_PREFERENCE, "0"));
            ledgerOrderObj.LineDescription = " ";
            
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    mActivity.Finish();
                    break;

            }
            return true;
        }
        private bool ValidateForm()
        {

            if (string.IsNullOrEmpty(edt_net_val.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(edt_vat_val.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(edt_gross_val.Text))
            {
                return false;
            }
            return true;
        }
        private void CallBackScreen()
        {
            Intent returnIntent = new Intent();        
            mActivity.SetResult(Android.App.Result.Ok, returnIntent);
            mActivity.Finish();
        }
    }

}

