

using Android.App;
using System;

namespace LucidX.Droid.Source.CustomViews
{
    /// <summary>
    /// Custom Progress Dialog
    /// </summary>
    public class CustomProgressDialog
    {
        /// <summary>
        /// Tag Name
        /// </summary>
        private const string tag = "CustomProgressDialog";

        private static ProgressDialog MprogressDialog = null;

        /// <summary>
        /// Show Progress Dialog
        /// </summary>
        /// <param name="mActivity">Activity instance</param>
        /// <param name="message">Message to show</param>
        public static void ShowProgDialog(Activity mActivity, string message)
        {
            try
            {
                if (MprogressDialog == null && mActivity != null)
                {
                    MprogressDialog = new ProgressDialog(mActivity);
                    MprogressDialog.SetCanceledOnTouchOutside(false);
                    MprogressDialog.SetCancelable(false);
                    if (message == null)
                        MprogressDialog.SetMessage("Loading....");
                    else
                        MprogressDialog.SetMessage(message);

                    MprogressDialog.Show();
                }
            }
            catch(Exception e)
            {
              
            }
           
        }

        /// <summary>
        /// Hide Progress dialog
        /// </summary>
        public static void HideProgressDialog()
        {
            try
            {
                if (MprogressDialog != null)
                {
                    MprogressDialog.Dismiss();
                    MprogressDialog = null;
                }
            }
            catch (Exception e)
            {
              
            }
        }
    }
}