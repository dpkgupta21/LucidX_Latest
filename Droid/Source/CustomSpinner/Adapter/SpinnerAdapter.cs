

using Java.Lang;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using LucidX.Droid.Source.CustomSpinner.Model;

namespace LucidX.Droid.Source.CustomSpinner.Adapter
{
    public class SpinnerAdapter : ArrayAdapter<SpinnerItemModel>
    {
        /// <summary>
        /// Tag
        /// </summary>
        private string TAG = "SpinnerAdaptor";
        /// <summary>
        /// Activity object
        /// </summary>
        readonly Activity mActivity;
        /// <summary>
        /// List of Spinner Item model list
        /// </summary>
        public List<SpinnerItemModel> SpinnerItemModelListObj { get; set; }

        public SpinnerAdapter(Activity mActivity, int txtViewResourceId, List<SpinnerItemModel> spinnerItemModelList)
            : base(mActivity, txtViewResourceId, spinnerItemModelList)

        {
            this.mActivity = mActivity;
            this.SpinnerItemModelListObj = spinnerItemModelList;

        }

        /// <summary>
        /// Sets dropdown view
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parentView"></param>
        /// <returns></returns>
        public override View GetDropDownView(int position, View convertView, ViewGroup parentView)
        {
            View view = mActivity.LayoutInflater.Inflate(Resource.Layout.custom_spinner_item, 
                parentView, false);
            TextView itemNameTV = view.FindViewById<TextView>(Resource.Id.txt_name);
            itemNameTV.Text = SpinnerItemModelListObj[position].TEXT;

            if (SpinnerItemModelListObj[position].STATE)
            {
                view.SetBackgroundResource(Resource.Color.blue_pop_up_header);
                itemNameTV.SetTextColor(Context.Resources.GetColor(Resource.Color.white));
            }
            else
            {
                view.SetBackgroundResource(Resource.Color.white);
                itemNameTV.SetTextColor(Context.Resources.GetColor(Resource.Color.blue_pop_up_header));
            }

            return view;
        }
        /// <summary>
        /// Returns view
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parentView"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parentView)
        {
            View view = mActivity.LayoutInflater.Inflate(Resource.Layout.custom_spinner_item, parentView, false);
            TextView itemNameTV = view.FindViewById<TextView>(Resource.Id.txt_name);


            try
            {
                itemNameTV.Text = SpinnerItemModelListObj[position].TEXT;
            }
            catch (Exception e1)
            {

            }



            return view;
        }

    }
}