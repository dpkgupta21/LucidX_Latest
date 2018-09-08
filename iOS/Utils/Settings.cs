
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace IosUtils
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        public static string saas_connection = "SAASConneection";
        public static string demo_connection = "DEMOConneection";
        public static string hq_connection = "HQConneection";
        public static string lucid_connection = "LUCIDConneection";

        public static string saas_val = "SAAS";
        public static string demo_val = "HELP";
        public static string hq_val = "HQ";
        public static string lucid_val = "LUCID";

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string RemeberMeKey = "rememberme_key";
        private const string UserNameKey = "username_key";
        private const string PasswordKey = "password_key";
        private const string LanguageKey = "language_key";
        private const string IsLogedinKey = "login_key";
        private const string UserIdKey = "userid_key";
        private const string UserMailKey = "usermail_key";
        private const string NameKey = "name_key";
        private const string UserCompCodeKey = "usercompcode_key";

        #endregion

        public static bool IsLogedin
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsLogedinKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsLogedinKey, value);
            }
        }

        public static string UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserIdKey, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserIdKey, value);
            }
        }

        public static string UserMail
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserMailKey, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserMailKey, value);
            }
        }

        public static string Name
        {
            get
            {
                return AppSettings.GetValueOrDefault(NameKey, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(NameKey, value);
            }
        }

        public static bool IsRememberMeSelected
        {
            get
            {
                return AppSettings.GetValueOrDefault(RemeberMeKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RemeberMeKey, value);
            }
        }

        public static string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserNameKey, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserNameKey, value);
            }
        }

        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault(PasswordKey, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PasswordKey, value);
            }
        }

        public static string CurrentLanguage
        {
            get
            {
                return AppSettings.GetValueOrDefault(LanguageKey, "en");
            }
            set
            {
                AppSettings.AddOrUpdateValue(LanguageKey, value);
            }
        }
        public static string UserCompCode
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserCompCodeKey, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserCompCodeKey, value);
            }
        }

        public static void RemoveLoginInfo()
        {
            AppSettings.Remove(UserNameKey);
            AppSettings.Remove(PasswordKey);
            AppSettings.Remove(RemeberMeKey);
        }

    }
}