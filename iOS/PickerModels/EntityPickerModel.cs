using System;
using UIKit;
using System.Collections.Generic;
using LucidX.ResponseModels;

namespace LucidX.iOS.PickerModels
{
    public class EntityPickerModel : UIPickerViewModel
    {
        public string currentTextFieldValue;
        UITextField txtField;
        public List<EntityCodesResponse> lstDropDownData = new List<EntityCodesResponse>();

        public EntityCodesResponse selectedModel;

        public EntityPickerModel(List<EntityCodesResponse> data, UITextField txt, EntityCodesResponse current)
        {
            lstDropDownData.AddRange(data);
            selectedModel = current;
            txtField = txt;
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
            return model.CompName;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            if (lstDropDownData == null || lstDropDownData.Count == 0)
                return;
            selectedModel = lstDropDownData[(int)row];
            txtField.Text = selectedModel.CompName;
        }
    }
}
