using System;
using System.Collections.Generic;
using UIKit;
using LucidX.ResponseModels;
namespace LucidX.iOS
{
	public class AccountOrderPickerModel: UIPickerViewModel
	{
		public string currentTextFieldValue;
		UITextField txtField;
		public List<AccountOrdersResponse> lstDropDownData = new List<AccountOrdersResponse>();
		public AccountOrdersResponse selectedModel;

		public AccountOrderPickerModel(List<AccountOrdersResponse> data, UITextField txt, AccountOrdersResponse currentModel)
		{
			lstDropDownData.AddRange(data);
			txtField = txt;
			selectedModel = currentModel;
			txtField.Text = selectedModel.AccountName;
		}

		public override nint GetComponentCount(UIPickerView pickerView)
		{
			return 1;
		}

		public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
		{
			if (lstDropDownData == null)
				return 0;

			return lstDropDownData.Count;
		}

		public override string GetTitle(UIPickerView pickerView, nint row, nint component)
		{
			var model = lstDropDownData[(int)row];
			return model.AccountName;
		}

		public override void Selected(UIPickerView pickerView, nint row, nint component)
		{
			if (lstDropDownData == null || lstDropDownData.Count == 0)
				return;
			selectedModel = lstDropDownData[(int)row];
			txtField.Text = selectedModel.AccountName;
		}
	}
}
