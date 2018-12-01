using Foundation;
using Maempedia.Interfaces;
using Maempedia.iOS.Helpers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(StatusBarColorHelper))]
namespace Maempedia.iOS.Helpers
{
    public class StatusBarColorHelper : IStatusBarColor
    {
        public void SetStatusBarColor(Color color, bool isLight = true)
        {
            UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
            statusBar.BackgroundColor = color.ToUIColor();
        }
    }
}