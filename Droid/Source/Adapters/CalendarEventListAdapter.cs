using Android.Views;
using Android.Widget;
using Android.App;
using Object = Java.Lang.Object;
using System.Collections.Generic;
using LucidX.ResponseModels;
using LucidX.Droid.Source.Utilities;

namespace LucidX.Droid.Source.Adapters
{
    public class CalendarEventListAdapter : BaseAdapter<CalendarEventResponse>
    {
        private LayoutInflater mLayoutInflater;
        private List<CalendarEventResponse> eventsList;
        private Activity mActivity;
        private List<CalendarEventResponse> filteredList;


        private class ViewHolder : Object
        {
            public TextView txt_notes_date { get; set; }
            public TextView txt_notes { get; set; }
            public TextView txt_img_lbl { get; set; }

            public TextView txt_notes_detail { get; set; }
        }

        public CalendarEventListAdapter(List<CalendarEventResponse> eventsList, Activity mActivity)
        {
            mLayoutInflater = (LayoutInflater)mActivity
                 .GetSystemService(Activity.LayoutInflaterService);
            this.eventsList = eventsList;
            this.mActivity = mActivity;
            filteredList = new List<CalendarEventResponse>();
            filteredList.AddRange(eventsList);
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

            holder.txt_notes.Text = eventsList[position].Subject;
            holder.txt_notes_detail.Text = eventsList[position].Details;
            holder.txt_img_lbl.Text = eventsList[position].Subject.Substring(0, 1);
            holder.txt_notes_date.Text = eventsList[position].DateStart.
                ToString(UtilityDroid.DISPLAY_DATE_TIME_FORMAT);
            return convertView;
        }

        public override int Count
        {
            get
            {
                return eventsList.Count;
            }
        }

        public override CalendarEventResponse this[int position]
        {
            get
            {
                return eventsList[position];
            }
        }

        public void GetFilteredList(string text)
        {
            eventsList.Clear();
            if (text.Length == 0)
            {
                eventsList.AddRange(filteredList);
            }
            else
            {
                foreach (CalendarEventResponse eventsResponseDTO in filteredList)
                {
                    if (eventsResponseDTO.Subject.ToUpper()
                            .Contains(text.ToUpper()))
                    {
                        eventsList.Add(eventsResponseDTO);
                    }
                }
            }

            NotifyDataSetChanged();
        }
    }
}

