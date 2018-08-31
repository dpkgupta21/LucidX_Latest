using System;
using Android.Views;
using Android.Widget;
using Android.App;
using Object = Java.Lang.Object;
using LucidX.ResponseModels;
using Android.Content.Res;
using LucidX.Droid.Source.Models;
using System.Collections.Generic;

namespace LucidX.Droid.Source.Adapters
{
    public class MenuAdapter : BaseAdapter<SideMenuModel>
    {
        private LayoutInflater mLayoutInflater;
        private List<SideMenuModel> menuList;
        private Activity mActivity;
        private int selectedPosition = -1;

        private const int TYPE_ITEM = 0;
        private const int TYPE_SEPARATOR = 1;
        private const int TYPE_LABEL = 2;
        private const int TYPE_MAX_COUNT = TYPE_LABEL + 1;



        public EmailCountResponse emailCount { get; set; }


        private class ViewHolder : Object
        {
            public LinearLayout linear { get; set; }
            public ImageView img_icon { get; set; }
            public TextView txt_menu_name { get; set; }
            public TextView txt_menu_counter { get; set; }

            public TextView txt_menu_label_header { get; set; }

        }

        public MenuAdapter(List<SideMenuModel> menuList, Activity mActivity)
        {
            mLayoutInflater = (LayoutInflater)mActivity
                 .GetSystemService(Activity.LayoutInflaterService);
            this.menuList = menuList;
            this.mActivity = mActivity;
        }

        public override int GetItemViewType(int position)
        {
            if (menuList[position].isMenuSeperator)
                return TYPE_SEPARATOR;
            else if (!string.IsNullOrEmpty(menuList[position].menuLabel))
                return TYPE_LABEL;
            else if (!string.IsNullOrEmpty(menuList[position].menuName))
                return TYPE_ITEM;
            else
                return TYPE_ITEM;
        }

        public override int ViewTypeCount
        {
            get
            {
                return TYPE_MAX_COUNT;
            }
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
                switch (type)
                {
                    case TYPE_ITEM:
                        convertView = mLayoutInflater.Inflate(Resource.Layout.item_menu_text,
                            parent, false);
                        holder.linear = convertView.FindViewById<LinearLayout>
                          (Resource.Id.linear);
                        holder.img_icon = convertView.FindViewById<ImageView>
                            (Resource.Id.img_icon);
                        holder.txt_menu_name = convertView.FindViewById<TextView>
                            (Resource.Id.txt_menu_name);
                        holder.txt_menu_counter = convertView.FindViewById<TextView>
                            (Resource.Id.txt_menu_counter);

                        break;
                    case TYPE_SEPARATOR:
                        convertView = mLayoutInflater.Inflate(Resource.Layout.item_menu_seperator,
                           parent, false);

                        break;
                    case TYPE_LABEL:
                        convertView = mLayoutInflater.Inflate(Resource.Layout.item_menu_label,
                           parent, false);
                        holder.txt_menu_label_header = convertView.FindViewById<TextView>
                            (Resource.Id.txt_menu_label_header);
                        break;
                }
                convertView.Tag = holder;
            }
            else
            {
                holder = (ViewHolder)convertView.Tag;
            }

            switch (type)
            {
                case TYPE_ITEM:
                    int id = mActivity.Resources.GetIdentifier(menuList[position].menuIcon,
                "drawable", mActivity.PackageName);
                    holder.img_icon.SetImageResource(id);
                    holder.txt_menu_name.Text = menuList[position].menuName;
                    if (emailCount != null)
                    {
                        switch (position)
                        {
                            case 1:
                                holder.txt_menu_counter.Text = emailCount.inboxCount != 0 ?
                                emailCount.inboxCount + "" : "";
                                break;
                            case 2:
                                holder.txt_menu_counter.Text = emailCount.draftCount != 0 ? 
                                    emailCount.draftCount + "" : "";
                                break;
                            case 3:
                                holder.txt_menu_counter.Text = emailCount.sentItemCount != 0 ? 
                                    emailCount.sentItemCount + "" : "";
                                break;
                            case 4:
                                holder.txt_menu_counter.Text = emailCount.trashCount != 0 ? emailCount.trashCount + "" : "";
                                break;
                        }
                    }

                    if (position == selectedPosition)
                    {
                        holder.linear.SetBackgroundColor(mActivity.Resources.
                            GetColor(Resource.Color.ColorPrimary));
                    }
                    else
                    {
                        holder.linear.SetBackgroundColor(mActivity.Resources.
                            GetColor(Resource.Color.transparent));
                    }
                    break;

                case TYPE_LABEL:
                    holder.txt_menu_label_header.Text = menuList[position].menuLabel;
                    break;
            }

            return convertView;
        }



        public override int Count
        {
            get
            {
                return menuList.Count;
            }
        }

        public override SideMenuModel this[int position]
        {
            get
            {
                return menuList[position];
            }
        }

        public void SetSelectedPosition(int selectedPosition)
        {
            this.selectedPosition = selectedPosition;
            NotifyDataSetChanged();
        }
    }
}

