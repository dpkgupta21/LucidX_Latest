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
using Android.Graphics;
using Android.Content.Res;
using Android.Util;

namespace LucidX.Droid.Source.CustomViews
{
    /// <summary>
    ///  Custom Text View, provides customization of Android Widget Text View
    /// </summary>
    public class CustomTextView : TextView
    {
        public Context context;

        public CustomTextView(Context context) : base(context)
        {
            this.context = context;
            Initialize();
        }

        public CustomTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.context = context;
            Initialize();
        }
        public CustomTextView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
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