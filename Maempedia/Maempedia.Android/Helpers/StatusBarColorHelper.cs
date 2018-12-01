using Android.App;
using Maempedia.Droid.Helpers;
using Maempedia.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(StatusBarColorHelper))]
namespace Maempedia.Droid.Helpers
{
    public class StatusBarColorHelper : IStatusBarColor
    {
        public void SetStatusBarColor(Color color, bool isLight = true)
        {
            ((Activity)Android.App.Application.Context).Window.SetStatusBarColor(color.ToAndroid());
        }
    }
}