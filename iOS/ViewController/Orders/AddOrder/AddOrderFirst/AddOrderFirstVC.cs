using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using IosUtils;
using LucidX.ResponseModels;
using LucidX.Webservices;
using Plugin.Connectivity;
using UIKit;

namespace LucidX.iOS
{
	public partial class AddOrderFirstVC : BaseAddOrderVC
	{

		private List<AccountOrdersResponse> accountOrderResponseList = null;

		AccountOrderPickerModel PickerModel;

		AccountOrdersResponse SelectedAccount;

		public AddOrderFirstVC() : base("AddOrderFirstVC", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigureView();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}


		void ConfigureView()
		{
			IosUtils.IosUtility.setcornerRadius(BtnNext);
			IosUtils.IosUtility.setBorderColor(TxtAddress);
			TxtOrderDate.InputView = IBDatePicker;
			IBDatePicker.Date = NSDate.Now;
			TxtOrderDate.InputAccessoryView = IBDateDoneBar;

			TxtSelectAcoountTilte.InputView = IBAccountPicker;
			TxtSelectAcoountTilte.InputAccessoryView = IBAccountDoneBar;
			//Scrollvw.UserInteractionEnabled = false;

			if (SuperVC.LedgerOrderObj != null)
			{
				//TxtAddress.Text = SuperVC.LedgerOrderObj.a
				//TxtCurrency.Text = SuperVC.LedgerOrderObAccountId
				TxtOrderName.Text = SuperVC.LedgerOrderObj.TransactionReference;
				TxtOrderDate.Text = SuperVC.LedgerOrderObj.TransDate;
			}
			else
			{
				TxtOrderDate.Text = DateTime.Now.ToString(Utils.Utilities.CALENDAR_DATE_FORMAT);
			}
			GetAccountCodes();
		}



		async void GetAccountCodes()
		{

			try
			{
				if (IosUtils.IosUtility.IsReachable())
				{
					IosUtils.IosUtility.showProgressHud("");
					accountOrderResponseList = await Webservices.WebServiceMethods.GetAccountForOrders();
					IosUtility.hideProgressHud();
					if (accountOrderResponseList != null && accountOrderResponseList.Count > 0)
					{
						var temp = accountOrderResponseList.Where(a => a.AccountId == SuperVC.LedgerOrderObj.AccountId).FirstOrDefault();
						SelectedAccount = temp;

						if (SelectedAccount == null)
						{
							SelectedAccount = accountOrderResponseList[0];
						}
						PickerModel = new AccountOrderPickerModel(accountOrderResponseList, TxtSelectAcoountTilte, SelectedAccount);
						IBAccountPicker.Model = PickerModel;
						ShowAccountAddress();
						ShowUserCurrency();
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

		private void ShowAccountAddress()
		{
			InvokeOnMainThread(() =>
			{
				if (PickerModel != null && !string.IsNullOrEmpty(PickerModel.selectedModel.City))
				{
					TxtAddress.Text = PickerModel.selectedModel.ContactPerson + "\n" +
						PickerModel.selectedModel.Telephone
						+ "\n" + PickerModel.selectedModel.City;
				}
			});
		}

		private async void ShowUserCurrency()
		{
			try
			{
				string countryCode = PickerModel.selectedModel.CountryCode;
				ShowUserCurrencyResponse userCurrencyResponseObj = null;
				if (IosUtils.IosUtility.IsReachable())
				{
					IosUtility.showProgressHud("");

					userCurrencyResponseObj = await WebServiceMethods.GetUserCurrencyFromCountryCode(countryCode);
					InvokeOnMainThread(() =>
					{
						if (userCurrencyResponseObj != null && !string.IsNullOrEmpty(userCurrencyResponseObj.CurrencyName))
						{
							TxtCurrency.Text = userCurrencyResponseObj.CurrencyName;
						}
					});
					IosUtility.hideProgressHud();

				}
			}
			catch (Exception e)
			{
				IosUtility.hideProgressHud();
				IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
															  IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
			}
		}

		public bool ValidateForm()
		{
			if (PickerModel == null)
			{ return false; }
			if (string.IsNullOrEmpty(TxtOrderDate.Text))
			{ return false; }
			else if (string.IsNullOrEmpty(TxtOrderName.Text))
			{ return false; }

			return true;
		}


		partial void DatePickerValueChanged(Foundation.NSObject sender)
		{
			TxtOrderDate.Text = IosUtils.IosUtility.ConvertToDateTime(IBDatePicker.Date).
											ToString(Utils.Utilities.CALENDAR_DATE_FORMAT);
		}

		partial void IBDateDoneClicked(Foundation.NSObject sender)
		{
			TxtOrderDate.EndEditing(true);
		}

		partial void IBDoneClicked(Foundation.NSObject sender)
		{
			TxtSelectAcoountTilte.EndEditing(true);
		}

		partial void TxtAcountEditingEnded(Foundation.NSObject sender)
		{
			ShowAccountAddress();
			ShowUserCurrency();
		}

		partial void BtnNextClicked(Foundation.NSObject sender)
		{
			if (!ValidateForm())
			{
				IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
													  IosUtils.LocalizedString.sharedInstance.GetLocalizedString("Please enter all details", "LSErrorTitle"));
				return;
			}
			SelectedAccount = PickerModel.selectedModel;
			SuperVC.LedgerOrderObj.AccountCode = SelectedAccount.AccountCode;
			SuperVC.LedgerOrderObj.AccountId = SelectedAccount.AccountId;
			//SuperVC.LedgerOrderObj.AccountName = SelectedAccount.AccountName;
			SuperVC.LedgerOrderObj.CountryCode = SelectedAccount.CountryCode;
			SuperVC.LedgerOrderObj.TransactionReference = TxtOrderName.Text;
			SuperVC.LedgerOrderObj.TransDate = TxtOrderDate.Text;
			SuperVC.LedgerOrderObj.CompCode = SelectedAccount.CompCode;

			SuperVC.index++;
			SuperVC.ChangePage();
		}

	}
}
