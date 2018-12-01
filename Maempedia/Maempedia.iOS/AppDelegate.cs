using FFImageLoading.Forms.Touch;
using Foundation;
using Maempedia.Common;
using UIKit;

namespace Maempedia.iOS
{
    [Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsGoogleMaps.Init(Constant.GOOGLE_MAPS_IOS_API_KEY);
            CachedImageRenderer.Init();

            App.ScreenWidth = UIScreen.MainScreen.Bounds.Width;
            App.ScreenHeight = UIScreen.MainScreen.Bounds.Height;
            App.Scale = UIScreen.MainScreen.Scale;

            this.LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
