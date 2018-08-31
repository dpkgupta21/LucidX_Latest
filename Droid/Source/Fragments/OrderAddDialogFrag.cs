using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using LucidX.Droid.Source.CustomSpinner.Adapter;
using LucidX.Droid.Source.CustomSpinner.Model;
using LucidX.Droid.Source.CustomViews;
using LucidX.Droid.Source.Fragments;
using LucidX.Droid.Source.Global;
using LucidX.Droid.Source.SharedPreference;
using LucidX.Droid.Source.Utilities;
using LucidX.ResponseModels;
using LucidX.Webservices;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using Activity = Android.App.Activity;
namespace LucidX.Droid.Source.CustomDialogFragment
{

    public class OrderAddDialogFrag : DialogFragment
    {
        private View mView;
        private Activity mActivity;
        private AddOrderSecondFragment secondFragment;
        private LedgerOrderItem ledgerOrderItemObj;
        private EditText edt_item_desc_val;
        private EditText edt_amount_val;
        private EditText edt_vat_val;
        private Button btn_save;
        private Button btn_cancel;
        private ImageView img_delete;

        private SharedPreferencesManager mSharedPreferencesManager;


        private Spinner spin_tax_rates_val;
        private SpinnerItemModel _selectedTaxRatesItem;
        private SpinnerAdapter _taxRatesSpinnerAdapter;
        private List<SpinnerItemModel> _taxRatesSpinnerItemModelList;
        //private int _selectedTaxRatesItemPosition;


        private Spinner spin_revenue_account_val;
        private SpinnerItemModel _selectedRevenueAccountItem;
        private SpinnerAdapter _revenueAccountSpinnerAdapter;
        private List<SpinnerItemModel> _revenueAccountSpinnerItemModelList;
        private int _selectedRevenueAccountItemPosition;

        private AccountOrdersResponse revAccOrderResponseObj = null;
        private List<AccountOrdersResponse> revenueAccOrderResponseList = null;

        private ShowTaxRatesResponse showTaxRatesResponseObj = null;
        private List<ShowTaxRatesResponse> showTaxRatesResponseList = null;

        private bool isEditOrderItem;
        private LedgerOrder ledgerObj;
        private int itemPos;

        private OrderAddDialogFrag(AddOrderSecondFragment secondFragment)
        {
            this.secondFragment = secondFragment;
        }
        public static OrderAddDialogFrag NewInstance(AddOrderSecondFragment secondFragment,
            string ledgerOrderLineItemString,
            bool isEditOrderItem,
            int itemPos, bool inEditMode)
        {
            var fragment = new OrderAddDialogFrag(secondFragment);
            Bundle b = new Bundle();
            b.PutString("ledgerOrderLineItem", ledgerOrderLineItemString);
            b.PutBoolean("isEditOrderItem", isEditOrderItem);
            b.PutInt("itemPos", itemPos);
            b.PutBoolean("inEditMode", inEditMode);
            fragment.Arguments = b;
            return fragment;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            mView = inflater.Inflate(Resource.Layout.dialog_add_order_fragment, container, false);

            mActivity = Activity;
            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);

            Dialog.SetTitle(Resource.String.ledger_order_item_title);
            Dialog.SetCancelable(false); //dismiss window on touch outside

            return mView;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            edt_item_desc_val = mView.FindViewById<EditText>(Resource.Id.edt_item_desc_val);
            edt_amount_val = mView.FindViewById<EditText>(Resource.Id.edt_amount_val);
            edt_vat_val = mView.FindViewById<EditText>(Resource.Id.edt_vat_val);
            btn_save = mView.FindViewById<Button>(Resource.Id.btn_save);
            btn_cancel = mView.FindViewById<Button>(Resource.Id.btn_cancel);
            spin_revenue_account_val = mView.FindViewById<Spinner>(Resource.Id.spin_revenue_account_val);
            spin_tax_rates_val = mView.FindViewById<Spinner>(Resource.Id.spin_tax_rates_val);
            img_delete = mView.FindViewById<ImageView>(Resource.Id.img_delete);

            btn_save.Click += Btn_save_Click;
            btn_cancel.Click += Btn_cancel_Click;
            img_delete.Click += Img_delete_Click;
            edt_amount_val.TextChanged += Edt_amount_val_TextChanged;

            itemPos = Arguments.GetInt("itemPos", -1);
            isEditOrderItem = Arguments.GetBoolean("isEditOrderItem", false);
            string ledgerOrderLineItemString = Arguments.GetString("ledgerOrderLineItem", null);

            if (isEditOrderItem && !string.IsNullOrEmpty(ledgerOrderLineItemString))
            {
                ledgerObj = secondFragment.GetLedgerOrderObj();
                ledgerOrderItemObj = JsonConvert.DeserializeObject<LedgerOrderItem>(ledgerOrderLineItemString);
                if (ledgerOrderItemObj != null)
                {
                    edt_item_desc_val.Text = ledgerOrderItemObj.LineDescription;
                    edt_amount_val.Text = string.Format("{0:F2}", ledgerOrderItemObj.BaseAmount);
                    edt_vat_val.Text = string.Format("{0:F2}", ledgerOrderItemObj.TaxAmount);
                    InitRevenueAccountSpinnerValues(ledgerOrderItemObj.CompCode);
                }
            }
            else
            {
                ledgerObj = secondFragment.GetLedgerOrderObj();
                InitRevenueAccountSpinnerValues(ledgerObj.CompCode);
            }

            // Set View Enability
            bool inEditMode = Arguments.GetBoolean("inEditMode", false);
            ShowViewEnablibity(inEditMode);
        }

        private void Edt_amount_val_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            CalculateVat();
        }

        private void Img_delete_Click(object sender, EventArgs e)
        {
           
        }

        private void ShowViewEnablibity(bool isEditMode)
        {

            edt_item_desc_val.Enabled = isEditMode;
            edt_amount_val.Enabled = isEditMode;
          

            spin_tax_rates_val.Enabled = isEditMode;
            spin_revenue_account_val.Enabled = isEditMode;

            btn_save.Visibility = isEditMode ? ViewStates.Visible : ViewStates.Gone;
            btn_cancel.Visibility = isEditMode ? ViewStates.Visible : ViewStates.Gone;


        }
        private void CalculateVat()
        {
            try
            {
                decimal taxPercent = showTaxRatesResponseObj.TaxRatePercent;
                if (!string.IsNullOrEmpty(edt_amount_val.Text))
                {
                    decimal amount = Convert.ToDecimal(edt_amount_val.Text);

                    decimal vat = (amount * taxPercent) / 100;

                    edt_vat_val.Text = vat + "";
                }


            }
            catch (Exception e) { }
        }

        /// <summary>
        /// Init values for Revenue Account Spinner
        /// </summary>
        private async void InitRevenueAccountSpinnerValues(int compCode)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    revenueAccOrderResponseList = await WebServiceMethods.GetRevenueOrders(compCode);

                    CustomProgressDialog.HideProgressDialog();
                }
                _revenueAccountSpinnerItemModelList = new List<SpinnerItemModel>();

                for (int i = 0; i < revenueAccOrderResponseList.Count; i++)
                {
                    SpinnerItemModel item = new SpinnerItemModel
                    {
                        Id = (i + 1) + "",
                        TEXT = revenueAccOrderResponseList[i].AccountName,
                        STATE = false,
                        EXTRA_TEXT = revenueAccOrderResponseList[i].CountryCode
                    };

                    if (ledgerOrderItemObj != null)
                    {
                        if (revenueAccOrderResponseList[i].AccountId == ledgerOrderItemObj.AccountId)
                        {
                            _selectedRevenueAccountItemPosition = i;
                        }
                    }
                    _revenueAccountSpinnerItemModelList.Add(item);
                }
                SetRevenueAccountSpinnerAdapter();
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
        private void SetRevenueAccountSpinnerAdapter()
        {
            _revenueAccountSpinnerAdapter = new SpinnerAdapter(mActivity, Resource.Layout.spinner_row_item_lay,
                   _revenueAccountSpinnerItemModelList);
            spin_revenue_account_val.Adapter = _revenueAccountSpinnerAdapter;
            spin_revenue_account_val.SetSelection(_selectedRevenueAccountItemPosition);

            // Initialize listener for spinner
            InitializeListeners();
        }

        private void InitializeListeners()
        {
            try
            {
                // Revenue Account Spinner
                spin_revenue_account_val.ItemSelected += (sender, args) =>
                {
                    _selectedRevenueAccountItem = _revenueAccountSpinnerItemModelList[args.Position];

                    revAccOrderResponseObj = revenueAccOrderResponseList[args.Position];

                    _revenueAccountSpinnerItemModelList[args.Position].STATE = true;
                    // update spinner item list state
                    for (int i = 0; i < _revenueAccountSpinnerItemModelList.Count; i++)
                    {
                        if (i == args.Position)
                        {
                            _revenueAccountSpinnerItemModelList[i].STATE = true;
                        }
                        else
                        {
                            _revenueAccountSpinnerItemModelList[i].STATE = false;
                        }
                    }
                    _revenueAccountSpinnerAdapter.NotifyDataSetChanged();

                    InitTaxRatesSpinnerValues();
                };

                // Show tax rates  Spinner
                spin_tax_rates_val.ItemSelected += (sender, args) =>
                {
                    _selectedTaxRatesItem = _taxRatesSpinnerItemModelList[args.Position];

                    showTaxRatesResponseObj = showTaxRatesResponseList[args.Position];

                    _taxRatesSpinnerItemModelList[args.Position].STATE = true;
                    // update spinner item list state
                    for (int i = 0; i < _taxRatesSpinnerItemModelList.Count; i++)
                    {
                        if (i == args.Position)
                        {
                            _taxRatesSpinnerItemModelList[i].STATE = true;
                        }
                        else
                        {
                            _taxRatesSpinnerItemModelList[i].STATE = false;
                        }
                    }
                    _taxRatesSpinnerAdapter.NotifyDataSetChanged();

                    CalculateVat();
                };
            }
            catch (Exception e)
            {
                UtilityDroid.PrintLog(Tag, e.StackTrace.ToString(), ConstantsDroid.LogType.ERROR);
            }
        }


        /// <summary>
        /// Init values for Revenue Account Spinner
        /// </summary>
        private async void InitTaxRatesSpinnerValues()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    string countryCode = string.IsNullOrEmpty(revAccOrderResponseObj.CountryCode) ?
                        ledgerObj.CountryCode : revAccOrderResponseObj.CountryCode;
                    showTaxRatesResponseList = await WebServiceMethods.ShowTaxRates(countryCode);

                    CustomProgressDialog.HideProgressDialog();
                }
                _taxRatesSpinnerItemModelList = new List<SpinnerItemModel>();

                for (int i = 0; i < showTaxRatesResponseList.Count; i++)
                {
                    SpinnerItemModel item = new SpinnerItemModel
                    {
                        Id = (i + 1) + "",
                        TEXT = showTaxRatesResponseList[i].TaxRatePercent + "",
                        STATE = false,
                        EXTRA_TEXT = showTaxRatesResponseList[i].TaxID + ""
                    };

                    _taxRatesSpinnerItemModelList.Add(item);
                }
                SetTaxRatesSpinnerAdapter();
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
        private void SetTaxRatesSpinnerAdapter()
        {
            _taxRatesSpinnerAdapter = new SpinnerAdapter(mActivity, Resource.Layout.spinner_row_item_lay,
                   _taxRatesSpinnerItemModelList);
            spin_tax_rates_val.Adapter = _taxRatesSpinnerAdapter;
            // spin_tax_rates_val.SetSelection(_selectedTaxRatesItemPosition);

        }



        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            try
            {
                Dismiss();
            }
            catch (Exception ex)
            {

            }
        }

        private void Btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                LedgerOrderItem model = new LedgerOrderItem();
                model.LineDescription = edt_item_desc_val.Text;
                model.BaseAmount = Convert.ToDecimal(edt_amount_val.Text);
                model.TaxAmount = Convert.ToDecimal(edt_vat_val.Text);
                model.CompCode = revAccOrderResponseObj.CompCode;
                model.AccountCode = revAccOrderResponseObj.AccountCode;
                model.AccountId = revAccOrderResponseObj.AccountId;
                model.AccountName = revAccOrderResponseObj.AccountName;

                // Add extra field
                model.TaxCode = " ";
                model.ProcessedBy = Convert.ToInt32(mSharedPreferencesManager.
                    GetString(ConstantsDroid.USER_ID_PREFERENCE, "0"));
                model.TransactionReference = " ";
                secondFragment.AddLedgerOrderItem(model, itemPos);
                Dismiss();


            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
        }
    }
}