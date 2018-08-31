
using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using LucidX.ResponseModels;
using System;
using System.Collections.Generic;

namespace LucidX.Droid.Source.Adapters
{
    public class InboxAdapter : RecyclerView.Adapter
    {
        public List<EmailResponse> emailList { get; set; }
        public event EventHandler<int> ItemClick;
        private Context context;
        private List<EmailResponse> filteredList;

        public class InboxViewHolder : RecyclerView.ViewHolder
        {
            public View viewMain;
            public RelativeLayout relative_item;
            public TextView txt_email_address;
            public TextView txt_email_detail;
            public TextView txt_email_time;
            public TextView txt_img_lbl;
            public ImageView img_attachment_icon;

            public InboxViewHolder(View view, Action<int> itemClickListener) : base(view)
            {
                viewMain = view;
                txt_email_address = (TextView)view.FindViewById(Resource.Id.txt_email_address);
                txt_email_detail = (TextView)view.FindViewById(Resource.Id.txt_email_detail);
                txt_email_time = (TextView)view.FindViewById(Resource.Id.txt_email_time);
                txt_img_lbl = (TextView)view.FindViewById(Resource.Id.txt_img_lbl);
                img_attachment_icon = (ImageView)view.FindViewById(Resource.Id.img_attachment_icon);
                relative_item = (RelativeLayout)view.FindViewById(Resource.Id.relative_item);
                relative_item.Click += (sender, e) => itemClickListener(base.Position);
            }
        }

        public InboxAdapter(Context context)
        {
            this.context = context;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var vi = LayoutInflater.From(parent.Context);
            var view = vi.Inflate(Resource.Layout.Individual_Item_Email, parent, false);

            return new InboxViewHolder(view, OnItemClickListener);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holderRaw, int position)
        {
            try
            {
                var holder = (InboxViewHolder)holderRaw;
                holder.viewMain.Tag = position;
                DateTime dateTime = new DateTime();

                EmailResponse dto = emailList[position];

                holder.txt_email_address.Text = dto.SenderEmail;
                holder.txt_email_detail.Text = dto.Subject;
                holder.txt_email_time.Text = Convert.ToDateTime(dto.Received).ToString("dd MMM") + "\n" + Convert.ToDateTime(dto.Received).ToString("hh:mm tt");
                if (dto.Attachment > 0)
                {
                    holder.img_attachment_icon.Visibility = ViewStates.Visible;
                }
                else
                {
                    holder.img_attachment_icon.Visibility = ViewStates.Gone;
                }

                if (dto.Unread)
                {
                    holder.txt_email_address.SetTextColor(Color.Black);
                    holder.txt_email_detail.SetTextColor(Color.Black);
                    holder.txt_email_address.Typeface = Typeface.DefaultBold;
                    holder.txt_email_detail.Typeface = Typeface.DefaultBold;
                }
                else
                {
                    holder.txt_email_address.SetTextColor(Color.Gray);
                    holder.txt_email_detail.SetTextColor(Color.Gray);
                    holder.txt_email_address.Typeface = Typeface.Default;
                    holder.txt_email_detail.Typeface = Typeface.Default;
                }

                holder.txt_img_lbl.Text = string.IsNullOrEmpty(dto.SenderName) ? "" : dto.SenderName.Substring(0, 1);


            }
            catch(Exception e)
            {

            }
        }

        public void SetData(List<EmailResponse> data)
        {
            emailList = data;
            filteredList = new List<EmailResponse>();
            filteredList.AddRange(emailList);

        }



        public override int ItemCount
        {
            get
            {
                return emailList.Count;
            }
        }

        public List<EmailResponse> GetData()
        {
            return emailList;
        }

        public void OnItemClickListener(int position)
        {
            if (ItemClick != null)
            {
                ItemClick(this, position);
            }
        }


        public void GetFilteredList(string text)
        {
            emailList.Clear();
            if (text.Length == 0)
            {
                emailList.AddRange(filteredList);
            }
            else
            {
                foreach (EmailResponse emailResponseDTO in filteredList)
                {
                    if (emailResponseDTO.SenderName.ToUpper()
                            .Contains(text.ToUpper()) ||
                            emailResponseDTO.SenderEmail.ToUpper()
                            .Contains(text.ToUpper()) ||
                            emailResponseDTO.Subject.ToUpper()
                            .Contains(text.ToUpper()))
                    {
                        emailList.Add(emailResponseDTO);
                    }
                }
            }

            NotifyDataSetChanged();
        }
    }
}