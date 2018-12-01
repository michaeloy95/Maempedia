using Android.App;
using Android.Widget;
using Maempedia.Droid.Helpers;
using Maempedia.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(MessageHelper))]
namespace Maempedia.Droid.Helpers
{
    public class MessageHelper : IMessageHelper
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}