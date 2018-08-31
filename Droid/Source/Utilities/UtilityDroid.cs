

using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.Util;
using LucidX.Droid.Source.SharedPreference;
using LucidX.Droid.Source.CustomViews;

namespace LucidX.Droid.Source.Utilities
{
    /// <summary>
    /// Utility Class contains utility functions
    /// </summary>
    public class UtilityDroid
    {

        /// <summary>
        /// Tag Name
        /// </summary>
        private const string tag = "UtilityDroid";

        public const string DATE_FORMAT = "MM/dd/yyyy";
        public const string DISPLAY_DATE_FORMAT = "d-MMM";
        public const string CALENDAR_DATE_FORMAT = "yyyy-MM-dd";
        public const string DISPLAY_TIME_FORMAT = "HH:mm";
        public const string DISPLAY_DATE_TIME_FORMAT = "d-MMM HH:mm";

        public static UtilityDroid utility;

        public static UtilityDroid GetInstance()
        {
            if (utility == null)
            {
                utility = new UtilityDroid();
            }

            return utility;
        }

        /// <summary>
        /// Print Logs
        /// </summary>
        /// <param name="logTag">Log Tag</param>
        /// <param name="logMsg">Log Message</param>
        /// <param name="logType">Log Type</param>
        public static void PrintLog(string logTag, string logMsg, Global.ConstantsDroid.LogType logType)
        {

            if (logType == Global.ConstantsDroid.LogType.VERBOSE)
            {
                Log.Verbose(logTag, logMsg);
            }
            else if (logType == Global.ConstantsDroid.LogType.INFO)
            {
                Log.Info(logTag, logMsg);
            }
            else if (logType == Global.ConstantsDroid.LogType.DEBUG)
            {
                Log.Debug(logTag, logMsg);
            }
            else if (logType == Global.ConstantsDroid.LogType.ERROR)
            {
                Log.Error(logTag, logMsg);
            }
            else if (logType == Global.ConstantsDroid.LogType.WARN)
            {
                Log.Warn(logTag, logMsg);
            }
            else
            {
                Log.Verbose(logTag, logMsg);
            }

        }

        /// <summary>
        /// Showing Toast messages
        /// </summary>
        /// <param name="context">Context of activity or Application</param>
        /// <param name="toastMsg">Toast message</param>
        /// <param name="toastLength">Toast Length</param>
        public void ShowToast(Context context, string toastMsg, ToastLength toastLength)
        {
            if (context != null)
            {
                Toast.MakeText(context, toastMsg, toastLength).Show();
            }
        }


        /// <summary>
        /// Show Alert Dialog
        /// </summary>
        /// <param name="activity">Activity instance</param>
        /// <param name="title">title of alert dialog</param>
        /// <param name="messsage">message to show alert dialog</param>
        /// <returns>Dialog</returns>
        public Dialog ShowAlertDialog(Activity activity, string title, string messsage, 
            string positivebtn, string negativeBtn)
        {
            try
            {

                Dialog dialog = null;
                dialog = new CustomDialog().CreateDialog(activity,
                                title,
                                messsage,
                                negativeBtn, positivebtn,
                                   new EventHandler(delegate (Object o, EventArgs a)
                                   {
                                       if (dialog != null)
                                       {
                                           dialog.Dismiss();
                                           dialog = null;
                                       }
                                   }),
                                 new EventHandler(delegate (Object o, EventArgs a)
                                 {
                                     if (dialog != null)
                                     {
                                         dialog.Dismiss();
                                         dialog = null;
                                     }
                                 }),
                              true, false);

                dialog.SetCancelable(true);
                dialog.Show();
                return dialog;


            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        ///  Get shared preference Manager with Encryption on for Key and value
        /// </summary>
        /// <param name="context"></param>
        /// <returns>SharedPreference Manager Object</returns>
        public SharedPreferencesManager GetSharedPreferenceManagerWithEncriptionEnabled(Context context)
        {
            SharedPreferencesManager sharedPreferencesManager = new SharedPreferencesManager(context,
                    Global.ConstantsDroid.SHARED_PREF_NAME, FileCreationMode.Private,
                    Global.ConstantsDroid.SHARED_PREFERENCE_SECURE_KEY, true, true);
            return sharedPreferencesManager;
        }



        /// <summary>
        /// Convert String to int
        /// </summary>
        /// <param name="value">String number</param>
        /// <returns>int value</returns>
        public static int ConvertStringToInt(string value)
        {
            try
            {
                return Int32.Parse(value);
            }
            catch (FormatException e)
            {
                return 0;
            }
        }



    }
}