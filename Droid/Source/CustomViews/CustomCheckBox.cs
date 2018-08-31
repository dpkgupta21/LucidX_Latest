
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
using Android.Util;
using Android.Content.Res;
using Android.Graphics;

namespace LucidX.Droid.Source.CustomViews
{
    /// <summary>
    /// Custom Button, provides customization of Andoird Widget Button
    /// </summary>
    public class CustomCheckBox : CheckBox
    {
        public Context context;

        public CustomCheckBox(Context context) : base(context)
        {
            this.context = context;
            Initialize();
        }

        public CustomCheckBox(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.context = context;
            Initialize();
        }
        public CustomCheckBox(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
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