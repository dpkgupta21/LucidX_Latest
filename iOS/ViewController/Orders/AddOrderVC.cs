using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using IosUtils;
using LucidX.ResponseModels;
using LucidX.Webservices;
using UIKit;

namespace LucidX.iOS
{
    public partial class AddOrderVC : UIViewController
    {
        public LedgerOrder LedgerOrderObj;

        AddOrderFirstVC addFirstVC;
        AddOrderSecond addTwoVC;
        AddOrderThird addThridVC;
        UIPageViewController pageVC;
        List<BaseAddOrderVC> ViewControllers = new List<BaseAddOrderVC>();
        public int index = 0;

        public bool isEdit;

        public AddOrderVC() : base("AddOrderVC", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();
        }

        async void ConfigureView()
        {
            this.EdgesForExtendedLayout = UIRectEdge.None;
            if (LedgerOrderObj == null)
            {
                LedgerOrderObj = new LedgerOrder();
            }
            else
            {
                await CallWebserviceForOrdersList();
            }

            pageVC = new UIPageViewController(UIPageViewControllerTransitionStyle.Scroll,
                                              UIPageViewControllerNavigationOrientation.Horizontal,
                                              UIPageViewControllerSpineLocation.Mid);

            addFirstVC = new AddOrderFirstVC();
            addTwoVC = new AddOrderSecond();
            addThridVC = new AddOrderThird();

            addFirstVC.SuperVC = this;
            addTwoVC.SuperVC = this;
            addThridVC.SuperVC = this;

            ViewControllers.Add(addFirstVC);
            ViewControllers.Add(addTwoVC);
            ViewControllers.Add(addThridVC);

            pageVC.View.Frame = new CoreGraphics.CGRect(0, 0,
                                                        ContentVw.Frame.Width,
                                                        ContentVw.Frame.Height);

            this.ContentVw.AddSubview(pageVC.View);
            this.AddChildViewController(pageVC);
            ChangePage();
        }

        public async Task CallWebserviceForOrdersList()
        {
            try
            {
                if (IosUtility.IsReachable())
                {
                    IosUtility.showProgressHud("");

                    var ledgerOrderItemList = await WebServiceMethods.GetLedgerOrderItems(LedgerOrderObj.CompCode, LedgerOrderObj.JournalNo);

                    LedgerOrderObj.LedgerOrderItems = ledgerOrderItemList;
                    IosUtility.hideProgressHud();
                }
            }
            catch (Exception ex)
            {
                IosUtility.hideProgressHud();
                IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "LSErrorTitle"),
                                                              IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUnknownError", "LSErrorTitle"));
            }
        }

        public void ChangePage()
        {
            //if (index == 0)
            //{
            //	BtnPre.Hidden = true;
            //}
            //else {
            //	BtnPre.Hidden = false;
            //}
            LblPageCount.Text = (index + 1) + "/" + ViewControllers.Count;
            pageVC.SetViewControllers(new UIViewController[] { ViewControllers[index] },
                                          UIPageViewControllerNavigationDirection.Forward,
                                          true, null);
        }

        partial void BtnPreClicked(Foundation.NSObject sender)
        {
            if (index > 0)
            {
                index--;
                ChangePage();
            }
        }

    }
}

