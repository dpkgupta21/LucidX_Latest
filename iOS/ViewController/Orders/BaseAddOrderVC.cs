using System;
using Foundation;
using LucidX.ResponseModels;
using UIKit;

namespace LucidX.iOS
{
	public class BaseAddOrderVC : UIViewController
	{
		//public LedgerOrder LedgerOrderObj { get; set; }

		public AddOrderVC SuperVC;

		public BaseAddOrderVC()
		{
		}

		public BaseAddOrderVC(string nibName, NSBundle bundle) : base(NSObjectFlag.Empty) { 
		
		}

	}
}
