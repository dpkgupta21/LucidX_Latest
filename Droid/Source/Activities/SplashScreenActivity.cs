using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using LucidX.Droid.Source.SharedPreference;
using LucidX.Droid.Source.Utilities;
using LucidX.ResponseModels;
using System;

namespace LucidX.Droid.Source.Activities
{
    [Activity(Label = "LucidX", MainLauncher = true, Icon = "@drawable/logo", Theme = "@style/AppTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashScreenActivity : Activity
    {
        private Activity mActivity;

        private SharedPreferencesManager mSharedPreferencesManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_splash);

            mActivity = this;

            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);

        }

        protected override void OnResume()
        {
            base.OnResume();

            var handler = new Handler();
            Action myAction = () =>
            {
                LoginResponse loginResponse = mSharedPreferencesManager.GetLoginResponse();
                if (loginResponse != null)
                {
                    StartActivity(new Intent(mActivity, typeof(HomeActivity)));
                    OverridePendingTransition(Resource.Animation.animation_enter,
                                Resource.Animation.animation_leave);
                }
                else
                {
                    StartActivity(new Intent(mActivity, typeof(LoginActivity)));
                    OverridePendingTransition(Resource.Animation.animation_enter,
                                Resource.Animation.animation_leave);
                }
                Finish();
            };
            handler.PostDelayed(myAction, 2000);
        }



    }

}

