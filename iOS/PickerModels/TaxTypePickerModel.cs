using System;
using System.Collections.Generic;
using UIKit;
using LucidX.ResponseModels;
namespace LucidX.iOS
{
	public class TaxTypePickerModel:UIPickerViewModel
	{
		public string currentTextFieldValue;
		UITextField txtField;
		public List<ShowTaxRatesResponse> lstDropDownData = new List<ShowTaxRatesResponse>();
		public ShowTaxRatesResponse selectedModel;

		public TaxTypePickerModel(List<ShowTaxRatesResponse> data, UITextField txt, ShowTaxRatesResponse currentModel)
		{
			lstDropDownData.AddRange(data);
			txtField = txt;
			selectedModel = currentModel;
			txtField.Text = selectedModel.TaxRatePercent.ToString();
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
			return model.TaxRatePercent.ToString();
		}

		public override void Selected(UIPickerView pickerView, nint row, nint component)
		{
			if (lstDropDownData == null || lstDropDownData.Count == 0)
				return;
			selectedModel = lstDropDownData[(int)row];
			txtField.Text = selectedModel.TaxRatePercent.ToString();
		}
	}
}
