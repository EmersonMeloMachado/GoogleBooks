using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleBooks.Droid
{
    [Activity(Label = "GoogleBooks",
       Theme = "@style/Theme.Splash",
           Icon = "@mipmap/icon",
           RoundIcon = "@mipmap/icon",
           MainLauncher = true,
           NoHistory = true,
           UiOptions = UiOptions.SplitActionBarWhenNarrow)]
    public class SplashActivity : Activity
    {
        public static Exception LogError { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;

            try
            {
                RequestWindowFeature(WindowFeatures.ActionBarOverlay);
            }
            catch (Exception e)
            {
                LogError = e;
            }

            StartActivity(typeof(MainActivity));
        }
    }
}