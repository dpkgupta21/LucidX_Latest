

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LucidX.Droid.Source.Global
{
    /// <summary>
    /// Constants Class, Contains constants used at android application level
    /// </summary>
    public class ConstantsDroid
    {

        /// <summary>
        /// Shared Preference Name
        /// </summary>
        public static string SHARED_PREF_NAME = "LucidX";

        /// <summary>
        /// Shared Preference Secure Key
        /// </summary>
        public static string SHARED_PREFERENCE_SECURE_KEY = "adasfdafsdfafafzaafafafafasf";


        /// <summary>
        ///  Log Type emums
        /// </summary>
        public enum LogType
        {
            VERBOSE,
            DEBUG,
            INFO,
            WARN,
            ERROR
        }

        public static string LANG_ENGLISH_CODE = "English";

        public static string LANG_FRENCH_CODE = "French";

        /// <summary>
        /// Shared Preference keys
        /// </summary>
        public static string APP_LANG_PREFERENCE = "AppLang";
        public static string ENCRYPT_TOKEN_PREFERENCE = "EncryptToken";
        public static string USERNAME_PREFERENCE = "Username";
        public static string PASSWORD_PREFERENCE = "Password";
        public static string DATABASE_PREFERENCE = "Database";
        public static string USER_ID_PREFERENCE = "UserId";
        public static string NAME_PREFEREENCE = "Name";
        public static string EMAIL_ID_PREFERENCE = "EmailId";
        public static string EMAIL_COUNT_OBJECT_PREFERENCE = "EmailCountObject";
        public static string LOGIN_RESPONSE_OBJECT_PREFERENCE = "LoginResponseObject";



        public const int ADD_NOTES_REQUEST_CODE = 1001;
        public const int NOTES_LIST_REQUEST_CODE = 1002;
        public const int CALENDAR_LIST_REQUEST_CODE = 1003;
        public const int ADD_CALENDAR_EVENT_REQUEST_CODE = 1004;
        public const int ORDERS_LIST_REQUEST_CODE = 1005;
        public const int ADD_ORDER_REQUEST_CODE = 1006;
        public const int EMAIL_DETAIL_REQUEST_CODE = 10007;



    }
}