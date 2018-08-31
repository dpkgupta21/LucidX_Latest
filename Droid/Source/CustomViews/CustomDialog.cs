    

using Android.App;
using Android.Views;
using Android.Widget;
using System;
using LucidX.Droid.Source.Utilities;

namespace LucidX.Droid.Source.CustomViews
{
    /// <summary>
    /// Custom Dialog
    /// </summary>
    public class CustomDialog 
    {
        private const string TAG = "CustomDialog";
       /// <summary>
       /// Views
       /// </summary>
        private TextView _titleTV;
        private TextView _messageTV;
        private Button _submitBtn;
        private Button _cancelBtn;


        public Dialog CreateDialog(Activity activity,
            string title, string message, 
            string cancelBtnText, string submitBtnText,
            EventHandler cancelBtnEventHandler, System.EventHandler submitBtnEventHandler,
            bool showcancelButton, bool showSubmitButton)
        {

            try
            {

                // Begin building a new dialog.
                var builder = new AlertDialog.Builder(activity);

                //Get the layout inflater
                var inflater = activity.LayoutInflater;

                //Inflate the layout for this dialog
                var dialogView = inflater.Inflate(Resource.Layout.custom_dialog_lay, null);


                if (dialogView != null)
                {
                    //Initialize the properties
                    _titleTV = dialogView.FindViewById<TextView>(Resource.Id.titleTV);
                    _messageTV = dialogView.FindViewById<TextView>(Resource.Id.messageTV);
                    _submitBtn = dialogView.FindViewById<Button>(Resource.Id.submitBtn);
                    _cancelBtn = dialogView.FindViewById<Button>(Resource.Id.cancelBtn);

                    _titleTV.Text = title;
                    _messageTV.Text = message;
                    _submitBtn.Text = submitBtnText;
                    _cancelBtn.Text = cancelBtnText;

                    _submitBtn.Click += submitBtnEventHandler;
                    _cancelBtn.Click += cancelBtnEventHandler;

                    _submitBtn.Text = submitBtnText;
                    _cancelBtn.Text = cancelBtnText;


                    if (showSubmitButton)
                    {
                        _submitBtn.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        _submitBtn.Visibility = ViewStates.Gone;
                    }

                    if (showcancelButton)
                    {
                        _cancelBtn.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        _cancelBtn.Visibility = ViewStates.Gone;
                    }
                    builder.SetView(dialogView);
                }


                //Create the builder 
                var dialog = builder.Create();

                //Now return the constructed dialog to the calling activity
                return dialog;
            }
            catch (Exception e)
            {
                UtilityDroid.PrintLog(TAG, e.StackTrace, Global.ConstantsDroid.LogType.ERROR);
                return null;
            }
        }

    }
}