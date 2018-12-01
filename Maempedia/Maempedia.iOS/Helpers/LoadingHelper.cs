using BigTed;
using Maempedia.Interfaces;
using Maempedia.iOS.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoadingHelper))]
namespace Maempedia.iOS.Helpers
{
    public class LoadingHelper : ILoadingHelper
    {
        public LoadingHelper()
        {
            BTProgressHUD.ForceiOS6LookAndFeel = true;
        }

        public void Show(string message = "")
        {
            BTProgressHUD.Show(maskType: ProgressHUD.MaskType.Gradient);

        }

        public void Hide()
        {
            BTProgressHUD.Dismiss();
        }
    }
}