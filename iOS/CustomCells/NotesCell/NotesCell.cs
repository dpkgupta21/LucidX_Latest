using System;

using Foundation;
using LucidX.ResponseModels;
using UIKit;

namespace LucidX.iOS.CustomCells
{
	public partial class NotesCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("NotesCell");
		public static readonly UINib Nib = UINib.FromName("NotesCell", NSBundle.MainBundle);

		CrmNotesResponse Data;

		CalendarEventResponse CalendarData;

		protected NotesCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureCell(CrmNotesResponse model)
		{
			Data = model;
			IBDateTimeLbl.Text = Data.CreatedDate.ToString("dd-MMM");
			IBNotesTitleLbl.Text = Data.NotesSubject;
			IBTitleLbl.Text = Data.NotesSubject.ToCharArray()[0].ToString();
			IBDescLbl.Text = Data.NotesDetail;
			this.SelectionStyle = UITableViewCellSelectionStyle.None;
		}

		public void ConfigureCell(CalendarEventResponse model)
		{
			CalendarData = model;
			IBDateTimeLbl.Text = CalendarData.DateStart.ToString("dd-MMM HH:mm");
			IBNotesTitleLbl.Text = CalendarData.Subject;
			IBTitleLbl.Text = CalendarData.Subject.ToCharArray()[0].ToString();
			IBDescLbl.Text = CalendarData.Details;
			this.SelectionStyle = UITableViewCellSelectionStyle.None;
		}

	}
}
