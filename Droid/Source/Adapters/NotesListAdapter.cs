using Android.Views;
using Android.Widget;
using Android.App;
using Object = Java.Lang.Object;
using System.Collections.Generic;
using LucidX.ResponseModels;
using LucidX.Droid.Source.Utilities;

namespace LucidX.Droid.Source.Adapters
{
    public class NotesListAdapter : BaseAdapter<CrmNotesResponse>
    {
        private LayoutInflater mLayoutInflater;
        private List<CrmNotesResponse> notesList;
        private Activity mActivity;
        private List<CrmNotesResponse> filteredList;


        private class ViewHolder : Object
        {
            public TextView txt_notes_date { get; set; }
            public TextView txt_notes { get; set; }
            public TextView txt_img_lbl { get; set; }
            
            public TextView txt_notes_detail { get; set; }
        }

        public NotesListAdapter(List<CrmNotesResponse> notesList, Activity mActivity)
        {
            mLayoutInflater = (LayoutInflater)mActivity
                 .GetSystemService(Activity.LayoutInflaterService);
            this.notesList = notesList;
            this.mActivity = mActivity;
            filteredList = new List<CrmNotesResponse>();
            filteredList.AddRange(notesList);
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

                convertView = mLayoutInflater.Inflate(Resource.Layout.individual_item_notes,
                    parent, false);
                holder.txt_notes_date = convertView.FindViewById<TextView>
                    (Resource.Id.txt_notes_date);
                holder.txt_notes = convertView.FindViewById<TextView>
                    (Resource.Id.txt_notes);
                holder.txt_notes_detail = convertView.FindViewById<TextView>
                   (Resource.Id.txt_notes_detail);
                holder.txt_img_lbl = convertView.FindViewById<TextView>
                 (Resource.Id.txt_img_lbl);
                convertView.Tag = holder;
            }
            else
            {
                holder = (ViewHolder)convertView.Tag;
            }

            holder.txt_notes.Text = notesList[position].NotesSubject;
            string date= notesList[position].CreatedDate.ToString(UtilityDroid.DISPLAY_DATE_FORMAT);
            holder.txt_notes_date.Text = date;
            holder.txt_notes_detail.Text = notesList[position].NotesDetail;
            holder.txt_img_lbl.Text= notesList[position].NotesSubject.Substring(0, 1);

            return convertView;
        }

        public override int Count
        {
            get
            {
                return notesList.Count;
            }
        }

        public override CrmNotesResponse this[int position]
        {
            get
            {
                return notesList[position];
            }
        }

        public void GetFilteredList(string text)
        {
            notesList.Clear();
            if (text.Length == 0)
            {
                notesList.AddRange(filteredList);
            }
            else
            {
                foreach (CrmNotesResponse notesResponseDTO in filteredList)
                {
                    if (notesResponseDTO.NotesSubject.ToUpper()
                            .Contains(text.ToUpper()))
                    {
                        notesList.Add(notesResponseDTO);
                    }
                }
            }

            NotifyDataSetChanged();
        }
    }
}

