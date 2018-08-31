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
using Android.Content.Res;
using Android.Util;
using Android.Graphics;

namespace LucidX.Droid.Source.CustomViews
{
    /// <summary>
    /// Custom Edit Text, provides customization of Andoird Widget Edit Text
    /// </summary>
    public class CustomEditText :EditText
    {

        public Context context;

        public CustomEditText(Context context) : base(context)
        {
            this.context = context;
            Initialize();
        }

        public CustomEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.context = context;
            Initialize();
        }
        public CustomEditText(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            this.context = context;
            Initialize();
        }
        void Initialize()
        {
            Typeface tf = tf = Typeface.CreateFromAsset(Context.Assets, "Fonts/century-gothic.ttf");
            SetTypeface(tf, 0);
        }
    }
   
}