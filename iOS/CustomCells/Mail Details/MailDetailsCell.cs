using System;
using LucidX.ResponseModels;
using Foundation;
using UIKit;

namespace CustomCells
{
    public partial class MailDetailsCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MailDetailsCell");
        public static readonly UINib Nib = UINib.FromName("MailDetailsCell", NSBundle.MainBundle);

        protected MailDetailsCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void ConfigureView(EmailResponse data)
        {
            this.SelectionStyle = UITableViewCellSelectionStyle.None;
            IBMailAddressLbl.Text = data.SenderEmail;
            IBDescLbl.Text = data.Subject;
            IBDateTimeLbl.Text = data.Received;
            IBTitleLbl.Text = data.SenderName[0].ToString();
            if (data.Unread)
            {
                setUnRead();
            }
            else
            {
                setRead();
            }
        }

        void setRead()
        {
            IBTitleLbl.BackgroundColor = UIColor.LightGray;
            IBDescLbl.TextColor = UIColor.LightGray;
            IBMailAddressLbl.TextColor = UIColor.LightGray;
            IBDescLbl.TextColor = UIColor.LightGray;
            IBDateTimeLbl.TextColor = UIColor.LightGray;
        }

        void setUnRead()
        {
            IBTitleLbl.BackgroundColor = IosUtils.IosColorConstant.ThemeProfileTextColor;
            IBDescLbl.TextColor = UIColor.Black;
            IBMailAddressLbl.TextColor = UIColor.Black;
            IBDescLbl.TextColor = UIColor.Black;
            IBDateTimeLbl.TextColor = UIColor.Black;
        }

        partial void IBDeleteBtnClicked(Foundation.NSObject sender)
        {


        }

    }
}
