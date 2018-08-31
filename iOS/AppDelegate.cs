using Foundation;
using LucidX.iOS;
using UIKit;
using Xamarin.SWRevealViewController;

namespace LucidX.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var navdesign = UINavigationBar.Appearance;
            navdesign.BackgroundColor = IosUtils.IosColorConstant.ThemeNavBlue;
            navdesign.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            navdesign.ShadowImage = new UIImage();
            navdesign.TintColor = UIColor.White;
            var textattributes = UINavigationBar.Appearance.GetTitleTextAttributes();
            textattributes.TextColor = UIColor.White;
            UINavigationBar.Appearance.SetTitleTextAttributes(textattributes);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
            if (IosUtils.Settings.IsLogedin)
            {
                showHomeScreen();
            }
            else
            {
                showLoginScreen();
            }
            return true;
        }

        /// <summary>
        /// Gets AppDelegate shared instance.
        /// </summary>
        /// <returns>The shared instance.</returns>
        public static AppDelegate GetSharedInstance()
        {
            return (AppDelegate)(UIApplication.SharedApplication.Delegate);
        }

        /// <summary>
        /// Gets AppDelegate shared instance.
        /// </summary>
        /// <returns>The shared instance.</returns>
        public static UIWindow GetMainWindow()
        {
            return UIApplication.SharedApplication.Delegate.GetWindow();
        }

        /// <summary>
        /// Shows the login screen.
        /// </summary>
        public void showLoginScreen()
        {
            var loginvc = new LoginVC();
            var win = UIApplication.SharedApplication.Delegate.GetWindow();
            win.RootViewController = loginvc;
            win.MakeKeyAndVisible();
        }

        /// <summary>
        /// Shows the home screen.
        /// </summary>
        public void showHomeScreen()
        {
            var inboxVC = new Inbox.InboxVC();
            var navVc = new UINavigationController(inboxVC);
            var fixitVw = new UIView(new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 20));
            fixitVw.BackgroundColor = IosUtils.IosColorConstant.ThemeNavBlue;
            navVc.View.AddSubview(fixitVw);

            var settingVC = new SettingsVc();
            settingVC.inboxVc = inboxVC;
            var NavDrawer = new SWRevealViewController();
            NavDrawer.RearViewRevealWidth = UIScreen.MainScreen.Bounds.Width * 0.75f;
            NavDrawer.FrontViewController = navVc;
            NavDrawer.RearViewController = settingVC;
            NavDrawer.RearViewRevealOverdraw = 0.0f;
            inboxVC.revealVC = NavDrawer;
            settingVC.revealVC = NavDrawer;
            inboxVC.MailTypeId = 1;
            Window.RootViewController = NavDrawer;
            Window.MakeKeyAndVisible();
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}

