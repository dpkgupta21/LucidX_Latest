using System;
using System.Collections.Generic;
using IosUtils;
using LucidX.ResponseModels;
using UIKit;
using System.Linq;

namespace LucidX.iOS
{
    public partial class AddOrderItemController : UIViewController
    {
        public int compCode;
        List<AccountOrdersResponse> revenueAccountResponseList;
        AccountOrderPickerModel PickerModel;
        public AccountOrdersResponse SelectedAccount;

        List<ShowTaxRatesResponse> TaxTypeResponseList;
        TaxTypePickerModel TaxPickerModel;
        public ShowTaxRatesResponse SelectedTax;

        public LedgerOrderItem ledgerItem;

        public EventHandler ItemAdded;

        public string ConutryCode;
        public int AccountId;
        public int TaxId;
        public bool Enable;
        bool isTaxEdit = true;

        public AddOrderItemController() : base("AddOrderItemController", null)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();
        }

        void ConfigureView()
        {
            if (ledgerItem == null)
            {
                ledgerItem = new LedgerOrderItem();
            }
            else
            {
                TxtDescription.Text = ledgerItem.LineDescription;
                TxtAmount.Text = ledgerItem.BaseAmount.ToString();
            }

            IosUtils.IosUtility.setcornerRadius(BtnOk);
            IosUtils.IosUtility.setcornerRadius(BtnCancel);
            TxtRevenue.InputView = RevenuePicker;
            TxtRevenue.InputAccessoryView = RevenueDoneBar;

            TxtTaxType.InputView = TaxtTypePicker;
            TxtTaxType.InputAccessoryView = TaxTypeDoneBar;

            TxtAmount.InputAccessoryView = AmountDoneBar;
            ShouldEdit();
            GetRevenueAccount();

        }

        void ShouldEdit()
        {
            TxtVat.Enabled = Enable;
            TxtAmount.Enabled = Enable;
            TxtTaxType.Enabled = Enable;
            TxtRevenue.Enabled = Enable;
            TxtDescription.Enabled = Enable;
            BtnOk.Hidden = !Enable;
            BtnCancel.Hidden = !Enable;
        }

        async void GetRevenueAccount()
        {
            try
            {
                if (IosUtils.IosUtility.IsReachable())
                {
                    IosUtils.IosUtility.showProgressHud("");
                    revenueAccountResponseList = await Webservices.WebServiceMethods.GetRevenueOrders(compCode);
                    IosUtility.hideProgressHud();
                    if (revenueAccountResponseList != null && revenueAccountResponseList.Count > 0)
                    {
                        var temp = revenueAccountResponseList.Where(a => a.AccountId == AccountId).FirstOrDefault();
                        SelectedAccount = temp;
                        if (SelectedAccount == null)
                        {
                            SelectedAccount = revenueAccountResponseList[0];
                        }
                        PickerModel = new AccountOrderPickerModel(revenueAccountResponseList, TxtRevenue, SelectedAccount);
                        RevenuePicker.Model = PickerModel;
                        isTaxEdit = false;
                        GetTaxTypes();
                    }
                    else
                    {
                        IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                              IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
                    }

                }

            }
            catch (Exception e)
            {
                IosUtility.hideProgressHud();
                IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                              IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
            }

        }

        async void GetTaxTypes()
        {
            try
            {
                if (IosUtils.IosUtility.IsReachable() && PickerModel != null)
                {
                    IosUtils.IosUtility.showProgressHud("");
                    TaxTypeResponseList = await Webservices.WebServiceMethods.
                                                           ShowTaxRates(
                                                               string.IsNullOrEmpty(PickerModel.selectedModel.CountryCode) ?
                                                               ConutryCode :
                                                               PickerModel.selectedModel.CountryCode);
                    IosUtility.hideProgressHud();
                    if (TaxTypeResponseList != null && TaxTypeResponseList.Count > 0)
                    {
                        //var temp = TaxTypeResponseList.Where(a => a.TaxID == TaxId).FirstOrDefault();
                        //SelectedTax = temp;
                        //if (SelectedTax == null && isTaxEdit)
                        //{
                        //	SelectedTax = TaxTypeResponseList[0];
                        //}
                        SelectedTax = TaxTypeResponseList[0];
                        TaxPickerModel = new TaxTypePickerModel(TaxTypeResponseList, TxtTaxType, SelectedTax);
                        TaxtTypePicker.Model = TaxPickerModel;
                        CalculateVat();
                    }
                    else
                    {
                        IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                              IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
                    }

                }

            }
            catch (Exception e)
            {
                IosUtility.hideProgressHud();
                IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                              IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
            }

        }

        partial void BtnCloseClicked(Foundation.NSObject sender)
        {
            this.DismissViewController(true, null);
        }

        partial void CancelClicked(Foundation.NSObject sender)
        {
            this.DismissViewController(true, null);
        }

        partial void AmountDoneClicked(Foundation.NSObject sender)
        {
            TxtAmount.EndEditing(true);
        }

        partial void BtnDeleteClicked(Foundation.NSObject sender)
        {

        }

        partial void BtnOkClicked(Foundation.NSObject sender)
        {
            try
            {
                ledgerItem = new LedgerOrderItem();
                ledgerItem.LineDescription = TxtDescription.Text;
                ledgerItem.BaseAmount = Convert.ToDecimal(TxtAmount.Text);
                ledgerItem.TaxAmount = Convert.ToDecimal(TxtVat.Text);
                ledgerItem.CompCode = SelectedAccount.CompCode;
                ledgerItem.AccountCode = SelectedAccount.AccountCode;
                ledgerItem.AccountId = SelectedAccount.AccountId;
                ledgerItem.AccountName = SelectedAccount.AccountName;

                // Add extra field
                ledgerItem.TaxCode = " ";
                ledgerItem.ProcessedBy = Convert.ToInt32(Settings.UserId);
                ledgerItem.TransactionReference = " ";
                if (ItemAdded != null)
                {
                    ItemAdded(ledgerItem, null);
                }
                this.DismissViewController(true, null);
            }
            catch (Exception e)
            {
                IosUtility.hideProgressHud();
                IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                              IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
            }
        }

        partial void RevenueDoneClicked(Foundation.NSObject sender)
        {
            TxtRevenue.EndEditing(true);
        }

        partial void TaxDoneClicked(Foundation.NSObject sender)
        {
            TxtTaxType.EndEditing(true);
        }

        partial void RevenueEditingEnded(Foundation.NSObject sender)
        {
            if (PickerModel != null)
            {
                SelectedAccount = PickerModel.selectedModel;
                isTaxEdit = true;
                GetTaxTypes();
            }
        }

        partial void AmountEditingEnded(Foundation.NSObject sender)
        {
            CalculateVat();
        }

        partial void TaxTypeEditingEnded(Foundation.NSObject sender)
        {
            if (TaxPickerModel != null)
            {
                SelectedTax = TaxPickerModel.selectedModel;
                CalculateVat();
            }
        }

        private void CalculateVat()
        {
            try
            {
                decimal taxPercent = SelectedTax.TaxRatePercent;
                decimal amount = Convert.ToDecimal(TxtAmount.Text);

                decimal vat = (amount * taxPercent) / 100;

                TxtVat.Text = vat + "";
            }
            catch
            {

            }
        }

    }
}

