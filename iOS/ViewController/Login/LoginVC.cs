using System;
using System.Collections.Generic;
using Foundation;
using IosUtils;
using LucidX.Constants;
using LucidX.RequestModels;
using LucidX.Webservices;
using UIKit;

namespace LucidX.iOS
{
    public partial class LoginVC : UIViewController, IUITextFieldDelegate
    {

        List<string> languages = new List<string>() { "English", "French" };

        string SelectedDataBase;

        #region Life Cycle methods

        public LoginVC() : base("LoginVC", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
        #endregion

        #region Helper Methods

        void ConfigureView()
        {
            SetupLanguageString();
            var model = new PickerModel(languages, IBLanguageTxt);
            IBLanguagePicker.Model = model;
            IBLanguageTxt.InputView = IBLanguagePicker;
            IBLanguageTxt.InputAccessoryView = IBDoneBar;
            IBLanguageTxt.Text = IosUtils.Settings.CurrentLanguage == "fr" ? "French" : "English";
            if (IosUtils.Settings.IsRememberMeSelected)
            {
                IBUsernameTxt.Text = IosUtils.Settings.UserName;
                IBPasswordTxt.Text = IosUtils.Settings.Password;
                IBRemeberMeBtn.Selected = IosUtils.Settings.IsRememberMeSelected;
            }
            IBHqBtn.Selected = true;
            IBVersinonLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSVersionInfo", "") + "(" + Settings.hq_val + ")";
            SelectedDataBase = Settings.hq_connection;
            WebserviceConstants.CONNECTION_NAME = Settings.hq_connection;
        }

        void SetupLanguageString()
        {
            IBUsernameLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSUsername", "");
            IBPasswordLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSPassword", "");
            IBLanguageLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSLanguage", "");
            IBRemeberMeBtn.SetTitle(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSRememberMe", ""), UIControlState.Normal);
            IBSignInBtn.SetTitle(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSLogin", ""), UIControlState.Normal);
            IBCopyRightLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSCopyRightTxt", "");
            IBTitleLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSCompanyTitle", "");
            IBVersinonLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSVersionInfo", "");
            IBDoneBtn.Title = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSDone", "");

        }

        async void Login()
        {
            if (IBRemeberMeBtn.Selected)
            {
                IosUtils.Settings.IsRememberMeSelected = true;
                IosUtils.Settings.UserName = IBUsernameTxt.Text;
                IosUtils.Settings.Password = IBPasswordTxt.Text;
            }
            else
            {
                IosUtils.Settings.RemoveLoginInfo();
            }
            if (IosUtils.IosUtility.IsReachable())
            {
                IosUtils.IosUtility.showProgressHud("");
                var loginResponse = await WebServiceMethods.Login(IBUsernameTxt.Text, IBPasswordTxt.Text);
                IosUtils.IosUtility.hideProgressHud();
                if (loginResponse != null)
                {
                    if (loginResponse.IsAuthenticate)
                    {
                        IosUtils.Settings.UserId = loginResponse.UserId;
                        IosUtils.Settings.IsLogedin = true;
                        IosUtils.Settings.Name = loginResponse.Name;
                        IosUtils.Settings.UserMail = loginResponse.UserEmail;
                        IosUtils.Settings.UserCompCode = loginResponse.UserCompCode;
                        AppDelegate.GetSharedInstance().showHomeScreen();
                    }
                    else
                    {
                        IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", ""),
                                                          IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSInvalidCredentialError", ""));

                    }
                }
                else
                {
                    //AppDelegate.GetSharedInstance().showHomeScreen();

                    IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", ""),
                                                          IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSInvalidCredentialError", ""));

                }
            }
        }

        void Validate()
        {

            bool result = true;
            string msg = "";
            if (string.IsNullOrEmpty(IBUsernameTxt.Text))
            {
                result = false;
                msg = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSBlankUsername", "");
            }
            else if (string.IsNullOrEmpty(IBPasswordTxt.Text))
            {

                result = false;
                msg = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSBlankPassword", "");
            }
            else if (string.IsNullOrEmpty(SelectedDataBase))
            {
                result = false;
                msg = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSBlankDataBase", "");
            }
            if (result)
            {
                Login();
            }
            else
            {
                IosUtils.IosUtility.showAlertWithInfo(IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSErrorTitle", "")
                                                   , msg);
            }
        }

        #endregion

        [Export("textFieldShouldBeginEditing:")]
        public bool ShouldBeginEditing(UITextField textField)
        {

            IosUtils.IosUtility.scrollViewToCenterOfScreen(IBScrollVw, this.View);
            return true;
        }

        [Export("textFieldShouldReturn:")]
        public bool ShouldReturn(UITextField textField)
        {
            IosUtils.IosUtility.scrollViewToZero(IBScrollVw);
            textField.EndEditing(true);
            return true;
        }

        #region IBAction methods

        partial void SignInClicked(Foundation.NSObject sender)
        {
            Validate();
        }


        partial void IBRemembermeClicked(Foundation.NSObject sender)
        {
            IBRemeberMeBtn.Selected = !IBRemeberMeBtn.Selected;
        }

        partial void IBDoneClicked(Foundation.NSObject sender)
        {
            if (IBLanguageTxt.Text == "French")
            {
                IosUtils.Settings.CurrentLanguage = "fr";
            }
            else
            {
                IosUtils.Settings.CurrentLanguage = "en";

            }
            SetupLanguageString();
            IosUtils.IosUtility.scrollViewToZero(IBScrollVw);
            IBLanguageTxt.EndEditing(true);
        }

        partial void DatabaseSelectionChanged(Foundation.NSObject sender)
        {
            var btn = sender as UIButton;

            IBSAASBtn.Selected = false;
            IBDemoBtn.Selected = false;
            IBLucidBtn.Selected = false;
            IBHqBtn.Selected = false;

            btn.Selected = true;
            //SelectedDataBase = WebserviceConstants.CONNECTION_NAME;
            if (sender == IBSAASBtn)
            {
                WebserviceConstants.CONNECTION_NAME = Settings.saas_connection;
                SelectedDataBase = Settings.saas_connection;
                IBVersinonLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSVersionInfo", "") + "(" + Settings.saas_val + ")";
            }
            else if (sender == IBDemoBtn)
            {
                WebserviceConstants.CONNECTION_NAME = Settings.demo_connection;
                SelectedDataBase = Settings.demo_connection;
                IBVersinonLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSVersionInfo", "") + "(" + Settings.demo_val + ")";
            }
            else if (sender == IBLucidBtn)
            {
                WebserviceConstants.CONNECTION_NAME = Settings.lucid_connection;
                SelectedDataBase = Settings.lucid_connection;
                IBVersinonLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSVersionInfo", "") + "(" + Settings.lucid_val + ")";
            }
            else if (sender == IBHqBtn)
            {
                WebserviceConstants.CONNECTION_NAME = Settings.hq_connection;
                SelectedDataBase = Settings.hq_connection;
                IBVersinonLbl.Text = IosUtils.LocalizedString.sharedInstance.GetLocalizedString("LSVersionInfo", "") + "(" + Settings.hq_val + ")";
            }

        }

        partial void BtnShowPasswordClicked(Foundation.NSObject sender)
        {
            BtnShowPassword.Selected = !BtnShowPassword.Selected;
            IBPasswordTxt.SecureTextEntry = !IBPasswordTxt.SecureTextEntry;

        }

        #endregion
    }
}

