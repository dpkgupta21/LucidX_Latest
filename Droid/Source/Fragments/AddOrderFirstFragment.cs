
using Android.OS;
using Android.Views;
using LucidX.Droid.Source.SharedPreference;
using LucidX.Droid.Source.Utilities;
using System;
using LucidX.Droid.Source.Picker;
using Android.Widget;
using LucidX.Droid.Source.CustomSpinner.Model;
using System.Collections.Generic;
using LucidX.Droid.Source.CustomSpinner.Adapter;
using Activity = Android.App.Activity;
using Android.Support.V4.App;
using LucidX.ResponseModels;
using LucidX.Droid.Source.Activities;
using Plugin.Connectivity;
using LucidX.Droid.Source.CustomViews;
using LucidX.Webservices;

namespace LucidX.Droid.Source.Fragments
{

    public class AddOrderFirstFragment : Fragment
    {

        /// <summary>
        /// The toolbar
        /// </summary>
        private Android.Support.V7.Widget.Toolbar toolbar;
        private Activity mActivity;
        private SharedPreferencesManager mSharedPreferencesManager;

        private View view;
        private TextView txt_order_date_val;
        private TextView txt_order_name_val;
        private EditText edt_currency;
        private EditText edt_account_address;
        private DateTime orderDateTime = DateTime.Now;

        private Spinner spin_account_code;
        private SpinnerItemModel _selectedAccountCodeItem;
        private SpinnerAdapter _accountCodeSpinnerAdapter;
        private List<SpinnerItemModel> _accountCodeSpinnerItemModelList;
        private int _selectedAccountCodeItemPosition;


        private LedgerOrder ledgerOrderObj;

        private List<AccountOrdersResponse> accountOrderResponseList = null;
        private AccountOrdersResponse accountOrderResponseObj = null;

        private bool isEdit;

        public static Fragment GetInstance(bool isEdit)
        {
            AddOrderFirstFragment fragment = new AddOrderFirstFragment();
            Bundle b = new Bundle();
            b.PutBoolean("isEdit", isEdit);
            fragment.Arguments = b;
            return fragment;
        }


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.fragment_add_first_order, container, false);

            mActivity = Activity;
            ledgerOrderObj = ((AddOrderFirstActivity)mActivity).LedgerOrderObj;
            isEdit = Arguments.GetBoolean("isEdit", false);

            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            Init();

            ShowInEditMode();

        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        private void Init()
        {   
            txt_order_name_val = view.FindViewById<TextView>(Resource.Id.txt_order_name_val);
            txt_order_date_val = view.FindViewById<TextView>(Resource.Id.txt_order_date_val);
            txt_order_date_val.Click += Txt_order_date_val_Click;

            Button btn_next = view.FindViewById<Button>(Resource.Id.btn_next);
            btn_next.Click += Btn_next_Click;

            spin_account_code = view.FindViewById<Spinner>(Resource.Id.spin_customer);

            edt_currency = view.FindViewById<EditText>(Resource.Id.edt_currency_val);
            edt_account_address = view.FindViewById<EditText>(Resource.Id.edt_account_address);
        }

        private void ShowInEditMode()
        {
            if (ledgerOrderObj != null)
            {
                txt_order_date_val.Text = Utils.Utilities.ShowDateInFormat(ledgerOrderObj.TransDate);
                txt_order_name_val.Text = !string.IsNullOrEmpty(ledgerOrderObj.TransactionReference) ?
                    ledgerOrderObj.TransactionReference : "";

                // Set selected Account order in spinner;
                InitAccountOrderSpinnerValues(ledgerOrderObj.AccountCode);
            }
            else
            {
                txt_order_date_val.Text = Utils.Utilities.ShowCurrentDateInFormat();

                // Set Account order in spinner;
                InitAccountOrderSpinnerValues(null);
            }

        }

        /// <summary>
        /// Init values for Customerspinner
        /// </summary>
        private async void InitAccountOrderSpinnerValues(string accountCode)
        {
            try
            {
                //string[] items = Resources.GetStringArray(Resource.Array.customer_entries);
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    accountOrderResponseList = await WebServiceMethods.GetAccountForOrders();

                    CustomProgressDialog.HideProgressDialog();
                }
                _accountCodeSpinnerItemModelList = new List<SpinnerItemModel>();

                for (int i = 0; i < accountOrderResponseList.Count; i++)
                {
                    SpinnerItemModel item = new SpinnerItemModel
                    {
                        Id = (i + 1) + "",
                        TEXT = accountOrderResponseList[i].AccountName,
                        STATE = false,
                        EXTRA_TEXT = accountOrderResponseList[i].AccountId + ""
                    };

                    if (!string.IsNullOrEmpty(accountCode))
                    {
                        if (accountOrderResponseList[i].AccountCode == accountCode)
                        {
                            _selectedAccountCodeItemPosition = i;
                        }
                    }
                    _accountCodeSpinnerItemModelList.Add(item);
                }
                SetCustomerSpinnerAdapter();
            }
            catch (Exception e)
            {
                CustomProgressDialog.HideProgressDialog();
                UtilityDroid.PrintLog(Tag, e.StackTrace.ToString(), Global.ConstantsDroid.LogType.ERROR);
            }
        }




        /// <summary>
        /// Set Customer spinner adapter
        /// </summary>
        private void SetCustomerSpinnerAdapter()
        {
            _accountCodeSpinnerAdapter = new SpinnerAdapter(mActivity, Resource.Layout.spinner_row_item_lay,
                   _accountCodeSpinnerItemModelList);
            spin_account_code.Adapter = _accountCodeSpinnerAdapter;
            spin_account_code.SetSelection(_selectedAccountCodeItemPosition);

            // Initialize listener for spinner
            InitializeListeners();
        }



        private void InitializeListeners()
        {
            // Customer Spinner
            spin_account_code.ItemSelected += (sender, args) =>
            {
                _selectedAccountCodeItem = _accountCodeSpinnerItemModelList[args.Position];

                accountOrderResponseObj = accountOrderResponseList[args.Position];

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

                // Show Account address 
                ShowAccountAddress();

                // Call User Currency from here
                ShowUserCurrency(accountOrderResponseObj.CountryCode);
            };
        }

        private void ShowAccountAddress()
        {
            if (accountOrderResponseObj != null && !string.IsNullOrEmpty(accountOrderResponseObj.City))
            {
                edt_account_address.Text = accountOrderResponseObj.ContactPerson + "\n" +
                    accountOrderResponseObj.Telephone
                    + "\n" + accountOrderResponseObj.City;
            }
        }


        private async void ShowUserCurrency(string countryCode)
        {
            try
            {
                ShowUserCurrencyResponse userCurrencyResponseObj = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    userCurrencyResponseObj = await WebServiceMethods.GetUserCurrencyFromCountryCode(countryCode);

                    if (userCurrencyResponseObj != null && !string.IsNullOrEmpty(userCurrencyResponseObj.CurrencyName))
                    {
                        edt_currency.Text = userCurrencyResponseObj.CurrencyName;
                    }
                    CustomProgressDialog.HideProgressDialog();

                }
            }
            catch (Exception e)
            {
                CustomProgressDialog.HideProgressDialog();
                UtilityDroid.PrintLog(Tag, e.StackTrace.ToString(), Global.ConstantsDroid.LogType.ERROR);
            }
        }



        private void Btn_next_Click(object sender, EventArgs e)
        {
            try {
                if (ValidateForm())
                {
                    ledgerOrderObj.TransDate = Utils.Utilities.GetDateForWebserviceTransDate(txt_order_date_val.Text.Trim());
                    ledgerOrderObj.TransactionReference = txt_order_name_val.Text.Trim();
                    ledgerOrderObj.AccountId = accountOrderResponseObj.AccountId;
                    ledgerOrderObj.AccountCode = accountOrderResponseObj.AccountCode;
                    ledgerOrderObj.CompCode = accountOrderResponseObj.CompCode;
                    ledgerOrderObj.CountryCode = accountOrderResponseObj.CountryCode;
                    ((AddOrderFirstActivity)mActivity).LedgerOrderObj = ledgerOrderObj;

                    DisplayFragment();
                }
                else
                {

                    UtilityDroid.GetInstance().ShowAlertDialog(mActivity, Resources.GetString(Resource.String.error_alert_title),
                           Resources.GetString(Resource.String.alert_message_fill_all_details),
                           Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));
                }

            }catch(Exception ex)
            {

            }
        }

        private void DisplayFragment()
        {
            Fragment fragment = AddOrderSecondFragment.GetInstance();
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.frame_container, fragment, fragment.Class.Name);
            ft.AddToBackStack(null);
            ft.Commit();
        }

        /// <summary>
        /// Shows Date picker for start Date time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_order_date_val_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                try
                {
                    //if (date.Date < DateTime.Now.Date)
                    //{
                    //    UtilityDroid.GetInstance().ShowAlertDialog(mActivity,
                    //        Resources.GetString(Resource.String.error_alert_title),
                    //        Resources.GetString(Resource.String.alert_message_not_less_than_current_date),
                    //        Resources.GetString(Resource.String.alert_cancel_btn),
                    //        Resources.GetString(Resource.String.alert_ok_btn));
                    //}
                    //else
                    //{
                    orderDateTime = date;
                    txt_order_date_val.Text = Utils.Utilities.DateIntoWebserviceFormat(date);
                    //  }
                }
                catch (Exception ex)
                {
                }
            }, orderDateTime);
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }


        private bool ValidateForm()
        {

            if (string.IsNullOrEmpty(txt_order_date_val.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(txt_order_name_val.Text))
            {
                return false;
            }

            return true;
        }

    }

}

