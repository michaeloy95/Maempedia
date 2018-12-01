using Android.App;
using Android.Support.V7.App;

namespace Maempedia.Droid
{
    [Activity(
        Label = "Maempedia",
        Icon = "@drawable/icon",
        Theme = "@style/splashscreen",
        MainLauncher = true,
        NoHistory = true)]
    public class SplashScreen : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}