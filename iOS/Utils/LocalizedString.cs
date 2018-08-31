//  ***********************************************************************
//  Assembly			: MyHero.iOS
//  Author				: Akash Deep Kaushik
//  CreatedAt			: 04-09-2016
//
//  ***********************************************************************
//  <copyright file="LocalizedString.cs" company="Pratham Software">
//      Pratham Software Pvt. Ltd. All rights reserved.
//  </copyright>
//  <summary></summary>
//  ***********************************************************************

using System;
using Foundation;

namespace IosUtils
{
	public class LocalizedString
	{
		private static LocalizedString instance = null;
		private LocalizedString ()
		{
		}

		public static LocalizedString sharedInstance {
			get {
				if (instance == null) {
					instance = new LocalizedString ();
				}
				return instance;
			}
		}

		/// <summary>
		/// Gets the localized string according to language selcted.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="comments">Comments.</param>
		public string GetLocalizedString (string key,string comments) {

			// Get the language code.
			NSString languageCode = new NSString(Settings.CurrentLanguage);

			// Get the relevant language bundle.
			var bundlePath = NSBundle.MainBundle.PathForResource( languageCode ,"lproj");
			NSBundle languageBundle;
			if (bundlePath != null) {
				languageBundle = NSBundle.FromPath (bundlePath);
			} else {
				languageBundle = NSBundle.MainBundle;
			}
			// Get the translated string using the language bundle.
			var translatedString = languageBundle.LocalizedString (key,comments);

			if (translatedString.Length < 1) {
				// There is no localizable strings file for the selected language.
				translatedString = NSBundle.MainBundle.LocalizedString (key,comments);
			}

			return translatedString;
		}
	}
}

