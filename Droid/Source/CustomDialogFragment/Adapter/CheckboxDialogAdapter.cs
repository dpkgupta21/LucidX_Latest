using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using LucidX.Droid.Source.Models;
using Android.Widget;
using Object = Java.Lang.Object;
using LucidX.ResponseModels;

namespace LucidX.Droid.Source.CustomDialogFragment.Adapter
{
    public class CheckboxDialogAdapter : BaseAdapter<NotesTypeResponse>
    {
        private LayoutInflater mLayoutInflater;
        public List<NotesTypeResponse> notesTypeList { get; set; }

        private Activity mActivity;

        public CheckboxDialogAdapter(List<NotesTypeResponse> notesTypeList, Activity mActivity)
        {
            mLayoutInflater = (LayoutInflater)mActivity
                 .GetSystemService(Activity.LayoutInflaterService);
            this.notesTypeList = notesTypeList;
            this.mActivity = mActivity;
        }

        private class ViewHolder : Object
        {
            public CheckBox chk_user { get; set; }
        }

    
        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            ViewHolder holder = null;
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = mLayoutInflater.Inflate(Resource.Layout.dialog_list_individual_item,
                        parent, false);
                holder.chk_user = convertView.FindViewById<CheckBox>
                  (Resource.Id.chk_user);
              
                convertView.Tag = holder;
            }
            else
            {
                holder = (ViewHolder)convertView.Tag;
            }

            holder.chk_user.Text = notesTypeList[position].NotesTypeName;
            holder.chk_user.Tag = position;
            holder.chk_user.CheckedChange += Chk_user_CheckedChange;
            holder.chk_user.Checked = notesTypeList[position].IsSelected;
            return convertView;
        }

        private void Chk_user_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            int pos = Convert.ToInt32(chkbox.Tag);
            notesTypeList[pos].IsSelected = chkbox.Checked;
            NotifyDataSetChanged();
        }

        public override long GetItemId(int position)
        {
            return position;
        }


        public override int Count
        {
            get
            {
                return notesTypeList.Count;
            }
        }

        public override NotesTypeResponse this[int position]
        {
            get
            {
                return notesTypeList[position];
            }
        }
    }
}