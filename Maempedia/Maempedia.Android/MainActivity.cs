using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using FFImageLoading.Forms.Platform;
using Plugin.Permissions;

namespace Maempedia.Droid
{
    [Activity(
        Label = "Maempedia",
        Theme = "@style/MyTheme",
        MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsGoogleMaps.Init(this, bundle);
            CachedImageRenderer.Init(true);

            this.OverridePendingTransition(Resource.Animation.design_snackbar_in, Resource.Animation.design_snackbar_out);

            var dm = new DisplayMetrics();
            this.WindowManager.DefaultDisplay.GetMetrics(dm);
            App.ScreenWidth = dm.WidthPixels / dm.ScaledDensity;
            App.ScreenHeight = dm.HeightPixels / dm.ScaledDensity;
            App.Scale = dm.ScaledDensity;

            // get the action bar height
            var value = new TypedValue();
            if (this.Theme.ResolveAttribute(Android.Resource.Attribute.ActionBarSize, value, true))
            {
                var height = TypedValue.ComplexToDimensionPixelSize(value.Data, this.Resources.DisplayMetrics);
                App.NavigationBarHeight = height / App.Scale;
            }

            this.LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}