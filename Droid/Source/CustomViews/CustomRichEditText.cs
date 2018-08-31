

using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Graphics;
using Android.Text.Style;
using StyleSpan = Android.Text.Style.StyleSpan;

namespace LucidX.Droid.Source.CustomViews
{
    public class CustomRichEditText : EditText
    {

        //public EditText edt;
        public Context context;
        public CustomRichEditText(Context context) : base(context)
        {
            this.context = context;
            Initialize();
        }

        public CustomRichEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.context = context;
            Initialize();
        }
        public CustomRichEditText(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            this.context = context;
            Initialize();
        }


        private void Initialize()
        {
            //edt = this;
            CustomSelectionActionModeCallback = new StyleCallback(this);
            Typeface tf = tf = Typeface.CreateFromAsset(Context.Assets, "Fonts/century-gothic.ttf");
            SetTypeface(tf, 0);

        }

        public class StyleCallback : Java.Lang.Object, ActionMode.ICallback
        {
            public CustomRichEditText edt;
            public StyleCallback(CustomRichEditText edt)
            {
                this.edt = edt;
            }

            public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
            {
                CharacterStyle cs;
                int start = edt.SelectionStart;
                int end = edt.SelectionEnd;
                SpannableStringBuilder ssb = new SpannableStringBuilder(edt.Text);

                switch (item.ItemId)
                {

                    case Resource.Id.bold:
                        cs = new StyleSpan(TypefaceStyle.Bold);
                        ssb.SetSpan(cs, start, end, SpanTypes.ExclusiveExclusive);
                        edt.Text = ssb.ToString();
                        return true;

                    case Resource.Id.italic:
                        cs = new StyleSpan(TypefaceStyle.Italic);
                        ssb.SetSpan(cs, start, end, SpanTypes.ExclusiveExclusive);
                        edt.Text = ssb.ToString();
                        return true;

                    case Resource.Id.underline:
                        cs = new UnderlineSpan();
                        ssb.SetSpan(cs, start, end, SpanTypes.ExclusiveExclusive);
                        edt.Text = ssb.ToString();
                        return true;

                    case Resource.Id.strikethrough:
                        cs = new StrikethroughSpan();
                        ssb.SetSpan(cs, start, end, SpanTypes.ExclusiveExclusive);
                        edt.Text = ssb.ToString();
                        return true;
                }
                return false;
            }

            public bool OnCreateActionMode(ActionMode mode, IMenu menu)
            {
                MenuInflater inflater = mode.MenuInflater;
                inflater.Inflate(Resource.Menu.menu_style, menu);
                return true;
            }

            public void OnDestroyActionMode(ActionMode mode)
            {
               
            }

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
            {
                return false;
            }
        }
    }
}