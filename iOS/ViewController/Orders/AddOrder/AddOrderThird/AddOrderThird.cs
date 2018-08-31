using System;
using IosUtils;
using LucidX.ResponseModels;
using LucidX.Webservices;
using UIKit;

namespace LucidX.iOS
{
    public partial class AddOrderThird : BaseAddOrderVC
    {
        decimal netAmount = 0;
        decimal vatAmount = 0;
        decimal grossAmount = 0;

        public AddOrderThird() : base("AddOrderThird", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            SetValues();
        }

        void ConfigureView()
        {
            IosUtils.IosUtility.setcornerRadius(BtnSave);
            IosUtils.IosUtility.setcornerRadius(BtnCancel);
        }

        private void SetValues()
        {
            if (SuperVC.LedgerOrderObj != null &&
                SuperVC.LedgerOrderObj.LedgerOrderItems != null &&
                SuperVC.LedgerOrderObj.LedgerOrderItems.Count != 0)
            {
                foreach (LedgerOrderItem orderItem in SuperVC.LedgerOrderObj.LedgerOrderItems)
                {
                    netAmount += orderItem.BaseAmount;
                    vatAmount += orderItem.TaxAmount;

                }
                grossAmount = netAmount + vatAmount;

                TxtNet.Text = netAmount + "";
                txtVat.Text = vatAmount + "";
                TxtGross.Text = grossAmount + "";


            }
        }

        partial void BtnCancelClicked(Foundation.NSObject sender)
        {
            SuperVC.NavigationController.PopViewController(true);
        }

        partial void BtnSaveClicked(Foundation.NSObject sender)
        {
            SuperVC.LedgerOrderObj.PresetCode = " ";
            SuperVC.LedgerOrderObj.TabID = 0;
            SuperVC.LedgerOrderObj.BaseAmount = grossAmount;
            SuperVC.LedgerOrderObj.ProcessedBy = Convert.ToInt32(Settings.UserId);
            SuperVC.LedgerOrderObj.LineDescription = " ";
            SaveRecord();
        }


        async void SaveRecord()
        {
            try
            {
                if (IosUtils.IosUtility.IsReachable())
                {
                    IosUtils.IosUtility.showProgressHud("");
                    bool isLedgerSaved = await WebServiceMethods.SaveLedgerOrdersNew(SuperVC.LedgerOrderObj);
                    IosUtility.hideProgressHud();
                    if (!isLedgerSaved)
                    {
                        IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                              IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
                    }
                    else
                    {
                        SuperVC.NavigationController.PopViewController(true);
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

    }
}

