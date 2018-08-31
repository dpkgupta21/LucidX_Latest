using System;
using Android.Views;
using Android.Widget;
using Android.App;
using Object = Java.Lang.Object;
using LucidX.Droid.Source.Models;
using System.Collections.Generic;
using LucidX.ResponseModels;
using Android.Content.Res;

namespace LucidX.Droid.Source.Adapters
{
    public class SideMenuListExpandableAdapter : BaseExpandableListAdapter
    {
        public List<GroupMenuModel> menuList { get; set; }
        private Activity mActivity;
        public EmailCountResponse emailCount { get; set; }

        public int selectedGroupPosition { get; set; }
        public int selectedChildPosition { get; set; }

        public SideMenuListExpandableAdapter(Activity mActivity, List<GroupMenuModel> menuList)
        {
            this.mActivity = mActivity;
            this.menuList = menuList;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded,
            View convertView, ViewGroup parent)
        {
            View mView = convertView;
            ViewHolderGroup holder = null;
            if (mView == null)
            {
                holder = new ViewHolderGroup();
                mView = mActivity.LayoutInflater.Inflate(Resource.Layout.group_menu_view, null);

                holder.txt_menu_name = mView.FindViewById<TextView>(Resource.Id.txt_menu_name);
                holder.img_down_btn = mView.FindViewById<ImageView>(Resource.Id.img_down_btn);
                holder.horizontal_seperator = mView.FindViewById<View>(Resource.Id.horizontal_seperator);

                mView.Tag = holder;
            }
            else
            {
                holder = (ViewHolderGroup)mView.Tag;
            }

            try
            {

                holder.txt_menu_name.Text = menuList[groupPosition].menuName;

                if (groupPosition == 4)
                {
                    holder.img_down_btn.Visibility = ViewStates.Gone;
                }
                else
                {
                    holder.img_down_btn.Visibility = ViewStates.Visible;
                }
            }

            catch (Exception e)
            {

            }

            return mView;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View mView = convertView;
            ViewHolderChild holder = null;

            if (mView == null)
            {
                holder = new ViewHolderChild();
                mView = mActivity.LayoutInflater.Inflate(Resource.Layout.child_submenu_view, null);

                holder.relative = mView.FindViewById<RelativeLayout>(Resource.Id.relative);
                holder.txt_submenu_name = mView.FindViewById<TextView>(Resource.Id.txt_submenu_name);
                holder.txt_submenu_count = mView.FindViewById<TextView>(Resource.Id.txt_submenu_count);
                holder.img_submenu_icon = mView.FindViewById<ImageView>(Resource.Id.img_submenu_icon);

                mView.Tag = holder;
            }
            else
            {
                holder = (ViewHolderChild)mView.Tag;
            }

            try
            {
                List<ChildMenuModel> submenuList = menuList[groupPosition].submenuList;
                ChildMenuModel submenu = submenuList[childPosition];
                holder.txt_submenu_name.Tag = groupPosition + ":" + childPosition;
                holder.txt_submenu_name.Text = submenu.submenuName;

                Resources resources = mActivity.Resources;
                int resourceId = resources.GetIdentifier(submenu.submenuIcon, "drawable",
                   mActivity.PackageName);

                holder.img_submenu_icon.SetImageResource(resourceId);

                if (groupPosition == selectedGroupPosition
                    && childPosition== selectedChildPosition)
                {
                    holder.relative.SetBackgroundColor(mActivity.Resources.
                        GetColor(Resource.Color.ColorPrimary));
                }
                else
                {
                    holder.relative.SetBackgroundColor(mActivity.Resources.
                        GetColor(Resource.Color.transparent));
                }

                switch (groupPosition)
                {
                    case 0:
                        switch (childPosition)
                        {
                            case 0:
                                holder.txt_submenu_count.Text = emailCount.inboxCount != 0 ?
                                emailCount.inboxCount + "" : "";
                                break;
                            case 1:
                                holder.txt_submenu_count.Text = emailCount.draftCount != 0 ?
                                    emailCount.draftCount + "" : "";
                                break;
                            case 2:
                                holder.txt_submenu_count.Text = emailCount.sentItemCount != 0 ?
                                    emailCount.sentItemCount + "" : "";
                                break;
                            case 3:
                                holder.txt_submenu_count.Text = emailCount.trashCount != 0 ?
                                    emailCount.trashCount + "" : "";
                                break;
                        }
                        break;

                    default:
                        holder.txt_submenu_count.Visibility = ViewStates.Gone;

                        break;
                }

            }

            catch (Exception e)
            {
            }

            return mView;
        }


        public override int GroupCount
        {
            get
            {
                return menuList.Count;
            }
        }

        public override bool HasStableIds
        {
            get
            {
                return true;
            }
        }

        public override Object GetChild(int groupPosition, int childPosition)
        {
            return menuList[groupPosition].submenuList[childPosition].submenuName;
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return menuList[groupPosition].submenuList.Count;
        }


        public override Object GetGroup(int groupPosition)
        {
            return null;
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }


        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            Console.WriteLine("groupPosition :" + groupPosition + "\nchildPosition :" + childPosition);
            return true;
        }

        public class ViewHolderGroup : Java.Lang.Object
        {
            public TextView txt_menu_name { get; set; }
            public ImageView img_down_btn { get; set; }
            public View horizontal_seperator { get; set; }
        }

        public class ViewHolderChild : Java.Lang.Object
        {
            public TextView txt_submenu_name { get; set; }
            public TextView txt_submenu_count { get; set; }
            public ImageView img_submenu_icon { get; set; }
            public RelativeLayout relative { get; set; }


        }
    }
}

