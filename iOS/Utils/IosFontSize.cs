//  ***********************************************************************
//  Assembly			: MyHero.iOS
//  Author				: Akash Deep Kaushik
//  CreatedAt			: 10-08-2016
//
//  ***********************************************************************
//  <copyright file="IosFontSize.cs" company="Pratham Software">
//      Pratham Software Pvt. Ltd. All rights reserved.
//  </copyright>
//  <summary>Height and Font Size Constants</summary>
//  ***********************************************************************

using System;
using UIKit;

namespace IosUtils
{
	public class IosFontSize
	{
		public static nfloat KNavButtonFontSize = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 40.0f : 25.0f;
		public static nfloat KFontSizeOne = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 2.0f : 1.0f;
		public static nfloat KFontSizeTwo = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 4.0f : 2.0f;
		public static nfloat KSizeFour = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 8.0f : 4.0f;
		public static nfloat KSizeFive= (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 10.0f : 5.0f;
		public static nfloat KFontSizeTen = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 20.0f : 10.0f;
		public static nfloat KFontSizeEleven = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 22.0f : 11.0f;
		public static nfloat KFontSizeTwele = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 24.0f : 12.0f;
		public static nfloat KFontSizeThirteen = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 26.0f : 13.0f;
		public static nfloat KFontSizeFourteen = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 28.0f : 14.0f;
		public static nfloat KFontSizeFifteen = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 30.0f : 15.0f;
		public static nfloat KFontSizeSixteen = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 32.0f : 16.0f;
		public static nfloat KFontSizeSeventeen = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 34.0f : 17.0f;
		public static nfloat KFontSizeEighteen = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 36.0f : 18.0f;
		public static nfloat KFontSizeNinteen = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 38.0f : 19.0f;
		public static nfloat KFontSizeTwenty = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 40.0f : 20.0f;
		public static nfloat KFontSizeTwentyTwo = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 44.0f : 22.0f;
		public static nfloat KFontSizeTwentyThree = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 46.0f : 23.0f;
		public static nfloat KFontSizeTwentyFive = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 50.0f : 25.0f;
		public static nfloat KFontSizeTwentySix = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 52.0f : 26.0f;
		public static nfloat KFontSizeTwentySeven = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 54.0f : 27.0f;
		public static nfloat KFontSizeThirty = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 60.0f : 30.0f;
		public static nfloat KFontSizeThirtyThree = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 66.0f : 33.0f;
		public static nfloat KFontSizeThirtyFour = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 68.0f : 34.0f;
		public static nfloat KFontSizeThirtyFive = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 70.0f : 35.0f;
		public static nfloat KFontSizeFifty = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 100.0f : 50.0f;
		public static nfloat KFontSizeSixty = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 120.0f : 60.0f;
	}

	public class HeightConstants
	{
		public static nint CellHeight25 = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 50 : 25;
		public static nint CellHeight35 = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 50 : 35;
		public static nint CellHeight50 = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 70 : 50;
		public static nint CellHeight60 = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 80 : 60;
		public static nint CellHeight70 = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 100 : 70;
		
		public static nint CellHeight200 = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 400 : 200;
	}
}

