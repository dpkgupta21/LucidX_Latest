//  ***********************************************************************
//  Assembly			: MyHero.iOS
//  Author				: Akash Deep Kaushik
//  CreatedAt			: 02-08-2016
//
//  ***********************************************************************
//  <copyright file="PickerModel.cs" company="Pratham Software">
//      Pratham Software Pvt. Ltd. All rights reserved.
//  </copyright>
//  <summary></summary>
//  ***********************************************************************

using System;
using UIKit;
using System.Collections.Generic;

namespace LucidX.iOS
{
	public class PickerModel : UIPickerViewModel
	{
		public string currentTextFieldValue;
		UITextField txtField;
		public List<string> lstDropDownData = new List<string>();

		public PickerModel (List<string> data,UITextField txt)
		{
			lstDropDownData.AddRange (data);
			txtField = txt;
		}

		public override nint GetComponentCount (UIPickerView pickerView)
		{
			return 1;
		}

		public override nint GetRowsInComponent (UIPickerView pickerView, nint component)
		{
			if (lstDropDownData == null)
				return 0;
			
			return lstDropDownData.Count;
		}

		public override string GetTitle (UIPickerView pickerView, nint row, nint component)
		{
			var model = lstDropDownData[(int)row];
			return model;
		}

		public override void Selected (UIPickerView pickerView, nint row, nint component)
		{
			if (lstDropDownData == null || lstDropDownData.Count == 0)
				return;
			var model = lstDropDownData [(int)row];
			txtField.Text = model;
		}


	}
}

