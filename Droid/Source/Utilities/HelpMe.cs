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
using LucidX.Droid.Source.Global;
using Java.Util;
using Android.Content.Res;

namespace LucidX.Droid.Source.Utilities
{
    public class HelpMe
    {
        public static HelpMe helpMe;

        public static HelpMe GetInstance()
        {
            if (helpMe == null)
            {
                helpMe = new HelpMe();
            }

            return helpMe;
        }

        // Check for valid mobile number of 10 digits
        public static void SetLocale(String languageCode, Context mContext)
        {
            String code = null;
            if (languageCode.Equals(ConstantsDroid.LANG_ENGLISH_CODE))
            {
                code = "en";
            }
            else
            {
                code = "fr";
            }
            Locale locale = new Locale(code);
            Locale.Default = locale;
            Configuration config = new Configuration();
            config.Locale = locale;
            mContext.Resources.UpdateConfiguration(config,
                    mContext.Resources.DisplayMetrics);
        }

    }
}