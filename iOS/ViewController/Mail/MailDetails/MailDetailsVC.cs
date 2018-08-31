using System;
using Foundation;
using UIKit;
using LucidX.ResponseModels;
using LucidX.Webservices;
namespace MailDetails
{
	public partial class MailDetailsVC : UIViewController
	{
		public EmailResponse mail;
		public MailDetailsVC() : base("MailDetailsVC", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.EdgesForExtendedLayout = UIRectEdge.None;
			ConfigureView();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		#region Helping Methods

		/// <summary>
		/// Configures the view.
		/// </summary>
		void ConfigureView()
		{
			IBNameLbl.Text = mail.SenderName;
			IBSubjectLbl.Text = mail.Subject;
			IBNameIconLbl.Text = mail.SenderName[0].ToString();
			GetDetails();
		}

		/// <summary>
		/// Gets the details of the mail.
		/// </summary>
		async void GetDetails()
		{
			if (IosUtils.IosUtility.IsReachable())
			{
				IosUtils.IosUtility.showProgressHud("");
				var desc = await WebServiceMethods.EmailDetail(mail.MailId, IosUtils.Settings.UserId);
				if (desc != null)
				{
					IBContnTxt.AttributedText = GetAttributedStringFromHtml(desc);
				}
				IosUtils.IosUtility.hideProgressHud();
			}
		}

		/// <summary>
		/// Gets the attributed string from html-string.
		/// </summary>
		/// <returns>The attributed string from html.</returns>
		/// <param name="html">Html.</param>
		NSAttributedString GetAttributedStringFromHtml(string html)
		{
			NSError error = null;
			NSAttributedString attributedString = new NSAttributedString(NSData.FromString(html),
				new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML, StringEncoding = NSStringEncoding.UTF8 },
				ref error);
			return attributedString;
		}

		#endregion



		#region IBAction Methods

		partial void ForwardBtnClicked(Foundation.NSObject sender)
		{
		}

		partial void InboxBtnClicked(Foundation.NSObject sender)
		{
			this.NavigationController.PopViewController(true);
		}

		partial void MenuBtnClicked(Foundation.NSObject sender)
		{
		}

		partial void ReplyAllBtnClicked(Foundation.NSObject sender)
		{
		}

		partial void ReplyBtnClicked(Foundation.NSObject sender)
		{
		}

		#endregion

	}
}

