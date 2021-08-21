using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppMusicPlayLists.Droid
{
    [Activity(Theme = "@style/Theme.Splash", Icon = "@mipmap/ic_launcher", MainLauncher = true, NoHistory = true, Label = "Favorite Songs")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            StartActivity(typeof(MainActivity));
        }
    }
}