using Android.Views;
using Android.Widget;
using Android.App;
using Object = Java.Lang.Object;
using LucidX.Droid.Source.Models;
using System.Collections.Generic;
using LucidX.ResponseModels;

namespace LucidX.Droid.Source.Adapters
{
    public class OrderAdapter : BaseAdapter<LedgerOrder>
    {
        private LayoutInflater mLayoutInflater;
        private List<LedgerOrder> ledgerOrderList;
        private Activity mActivity;


        private class ViewHolder : Object
        {
            public TextView txt_order_date { get; set; }
            public TextView txt_order_name { get; set; }
            public TextView txt_cost { get; set; }
            public TextView txt_account_name { get; set; }

        }

        public OrderAdapter(List<LedgerOrder> ledgerOrderList, Activity mActivity)
        {
            mLayoutInflater = (LayoutInflater)mActivity
                 .GetSystemService(Activity.LayoutInflaterService);
            this.ledgerOrderList = ledgerOrderList;
            this.mActivity = mActivity;
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            ViewHolder holder = null;
            int type = GetItemViewType(position);
            if (convertView == null)
            {
                holder = new ViewHolder();

                convertView = mLayoutInflater.Inflate(Resource.Layout.individual_item_order,
                    parent, false);
                holder.txt_order_date = convertView.FindViewById<TextView>
                    (Resource.Id.txt_order_date);
                holder.txt_order_name = convertView.FindViewById<TextView>
                    (Resource.Id.txt_order_name);
                holder.txt_cost = convertView.FindViewById<TextView>
                   (Resource.Id.txt_cost);
                holder.txt_account_name = convertView.FindViewById<TextView>
                   (Resource.Id.txt_account_name);

                convertView.Tag = holder;
            }
            else
            {
                holder = (ViewHolder)convertView.Tag;
            }

            holder.txt_order_name.Text = ledgerOrderList[position].TransactionReference;
            holder.txt_cost.Text = string.Format("{0:F2}", ledgerOrderList[position].CompleteTotal) + "";
            holder.txt_order_date.Text=  Utils.Utilities.ShowDateInFormat(ledgerOrderList[position].TransDate);
            holder.txt_account_name.Text = ledgerOrderList[position].AccountName;

            return convertView;
        }

        public override int Count
        {
            get
            {
                return ledgerOrderList.Count;
            }
        }

        public override LedgerOrder this[int position]
        {
            get
            {
                return ledgerOrderList[position];
            }
        }

    }
}

