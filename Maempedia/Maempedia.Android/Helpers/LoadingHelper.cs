using AndroidHUD;
using Maempedia.Droid.Helpers;
using Maempedia.Interfaces;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoadingHelper))]
namespace Maempedia.Droid.Helpers
{
    public class LoadingHelper : ILoadingHelper
    {
        public void Show(string message = "Memuat...")
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AndHUD.Shared.Show(CrossCurrentActivity.Current.Activity, maskType: MaskType.Black);
            });
        }

        public void Hide()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AndHUD.Shared.Dismiss(CrossCurrentActivity.Current.Activity);
            });
        }
    } 
}