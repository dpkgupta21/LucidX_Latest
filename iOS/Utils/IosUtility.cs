//  ***********************************************************************
//  Assembly			: MyHero.iOS
//  Author				: Akash Deep Kaushik
//  CreatedAt			: 28-09-2016
//
//  ***********************************************************************
//  <copyright file="Utility.cs" company="Pratham Software">
//      Pratham Software Pvt. Ltd. All rights reserved.
//  </copyright>
//  <summary></summary>
//  ***********************************************************************

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using CoreGraphics;
using Foundation;
using AVFoundation;
using MBProgressHUD;
using LucidX.iOS;
using Plugin.Connectivity;

namespace IosUtils
{
	public class IosUtility
	{
		public static Double keyBoardHeight = 270;
		public static nfloat cornerRadious = 5.0f;

		private static MTMBProgressHUD hud;// = ();

		public IosUtility()
		{
		}

		/// <summary>
		/// Set content possition of UIScrollView according to input textField
		/// </summary>
		/// <param name="scrollView">Scroll view.</param>
		/// <param name="theView">The view.</param>
		public static void scrollViewToCenterOfScreen(UIScrollView scrollView, UIView theView)
		{
			var applicationFrame = UIScreen.MainScreen.ApplicationFrame;

			var avaliableHeight = applicationFrame.Size.Height - IosUtility.keyBoardHeight;

			scrollView.SetContentOffset(new CGPoint(scrollView.ContentOffset.X, avaliableHeight), animated: true);
		}

		/// <summary>
		/// Set content Offset of UIScrollView to zero
		/// </summary>
		/// <param name="scrollView">Scroll view.</param>
		public static void scrollViewToZero(UIScrollView scrollView)
		{
			scrollView.SetContentOffset(new CGPoint(0, 0), true);
			UIApplication.SharedApplication.KeyWindow.EndEditing(true);
		}

		/// <summary>
		/// Set left view of text field 
		/// </summary>
		/// <param name="textField">Text field.</param>
		public static void setLeftPadding(UITextField textField)
		{
			var paddingView = new UIView(new CGRect(0, 0, 5, textField.Frame.Size.Height));
			textField.LeftView = paddingView;
			textField.LeftViewMode = UITextFieldViewMode.Always;
		}

		/// <summary>
		/// Set right view of text field 
		/// </summary>
		/// <param name="textField">Text field.</param>
		/// <param name="imageName">Image name.</param>
		public static void setRightPaddingImage(UITextField textField, String imageName)
		{
			var paddingView = new UIImageView(new CGRect(0, 0, textField.Frame.Size.Height, textField.Frame.Size.Height));
			paddingView.Image = UIImage.FromBundle(imageName);
			textField.RightView = paddingView;
			textField.RightViewMode = UITextFieldViewMode.Always;
		}

		/// <summary>
		/// Sets the color of the border of textfield.
		/// </summary>
		/// <param name="textField">Text field.</param>
		public static void setBorderColor(UIView textField)
		{
			textField.Layer.CornerRadius = IosUtility.cornerRadious;
			textField.Layer.BorderWidth = 2.0f;
			textField.Layer.BorderColor = IosColorConstant.TextBorderColor.CGColor;
			textField.Layer.MasksToBounds = true;
		}

		public static void setcornerRadius(UIView vw)
		{
			vw.Layer.CornerRadius = IosUtility.cornerRadious;
		}

		/// <summary>
		/// Add the shadow to view.
		/// </summary>
		/// <param name="button">Button.</param>
		/// <param name="justShadow">Just shadow.</param>
		public static void addShadow(UIView button, bool justShadow = false)
		{
			if (!justShadow)
			{
				button.Layer.CornerRadius = IosUtility.cornerRadious;
			}
			button.Layer.ShadowColor = UIColor.Black.CGColor;
			button.Layer.ShadowOffset = new CGSize(5, 5);
			button.Layer.MasksToBounds = false;
			button.Layer.ShadowOpacity = 0.5f;
		}


		/// <summary>
		/// This function show progress view over window.
		/// </summary>
		/// <param name="text">Text.</param>
		public static void showProgressHud(String text)
		{
			if (hud == null)
			{
				hud = new MTMBProgressHUD(AppDelegate.GetSharedInstance().Window);
				AppDelegate.GetMainWindow().AddSubview(hud);
				hud.Show(true);
				hud.LabelText = text;
			}
		}


		/// <summary>
		/// This function hide progress view over window.
		/// </summary>
		public static void hideProgressHud()
		{
			if (hud != null)
			{
				hud.Hide(true);
				hud.RemoveFromSuperview();
				hud.Delegate = null;

				hud = null;
			}
		}

		/// <summary>
		/// Shows the toast message.
		/// </summary>
		/// <param name="msg">Message to display.</param>
		public static void showToast(string msg)
		{
			//show message for activation/deactivation hero mode
			MTMBProgressHUD toast = new MTMBProgressHUD(AppDelegate.GetSharedInstance().Window);
			AppDelegate.GetMainWindow().AddSubview(toast);
			toast.Mode = MBProgressHUDMode.Text;
			toast.Show(true);
			toast.DetailsLabelText = msg;
			toast.RemoveFromSuperViewOnHide = true;
			toast.Hide(true, 1);
		}


		/// <summary>
		/// Shows the alert with info.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		public static void showAlertWithInfo(string title, string message)
		{

			var alertView = new UIAlertView(title,
											message,
											null,
											LocalizedString.sharedInstance.GetLocalizedString("LSOkKey", ""));

			alertView.Show();
		}

		/// <summary>
		/// Check Internet is available or not.
		/// </summary>
		public static bool IsReachable()
		{
			if (Reachability.InternetConnectionStatus() == NetworkStatus.NotReachable)
			{
				IosUtility.showAlertWithInfo(LocalizedString.sharedInstance.GetLocalizedString("LSConnectionTitle", ""),
											 LocalizedString.sharedInstance.GetLocalizedString("LSNoInternet", ""));

				return false;
			}

			return true;
		}

		/// <summary>
		/// Converts NSDate to DateTime.
		/// </summary>
		/// <param name="date">The date.</param>
		public static DateTime ConvertToDateTime(NSDate date)
		{
			DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0);
			DateTime Date = reference.AddSeconds(date.SecondsSinceReferenceDate);
			return Date;
		}

		/// <summary>
		/// Converts to NSDate.
		/// </summary>
		/// <returns>The to NSDate.</returns>
		/// <param name="date">Date.</param>
		public static NSDate ConvertToNSDate(DateTime date)
		{
			if (date.Kind == DateTimeKind.Unspecified)
				date = DateTime.SpecifyKind(date, DateTimeKind.Local);
			return (NSDate)date;
		}

		/// <summary>
		/// Convert hex to UIColor
		/// </summary>
		/// <returns>The UI Color.</returns>
		/// <param name="hexString">Hex string.</param>
		public static UIColor ToUIColor(string hexString)
		{
			hexString = hexString.Replace("#", "");

			if (hexString.Length == 3)
				hexString = hexString + hexString;

			if (hexString.Length != 6)
				throw new Exception("Invalid hex string");

			int red = Int32.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
			int green = Int32.Parse(hexString.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
			int blue = Int32.Parse(hexString.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

			return UIColor.FromRGB(red, green, blue);
		}

	}

}

