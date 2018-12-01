using Android.Content;
using Android.OS;
using Maempedia.Droid.Utilities;
using Maempedia.Interfaces;
using Plugin.CurrentActivity;
using System;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceUtility))]
namespace Maempedia.Droid.Utilities
{
    public class DeviceUtility : IDeviceUtility
    {
        public DeviceUtility()
        {
        }

        public string DeviceName
        {
            get
            {
                return Android.Bluetooth.BluetoothAdapter.DefaultAdapter.Name;
            }
        }

        public string AppName
        {
            get
            {
                var attrs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                return attrs.Length == 0
                    ? string.Empty
                        : ((AssemblyProductAttribute)attrs[0]).Product;
            }
        }

        public string AppVersion
        {
            get
            {
                var context = CrossCurrentActivity.Current.Activity;
                return context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            }
        }

        public string BuildVersion
        {
            get
            {
                var context = CrossCurrentActivity.Current.Activity;
                return context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode.ToString(CultureInfo.InvariantCulture);
            }
        }

        public string Copyright
        {
            get
            {
                var attrs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return attrs.Length == 0
                    ? string.Empty
                        : ((AssemblyCopyrightAttribute)attrs[0]).Copyright;
            }
        }

        public string AppPath
        {
            get
            {
                return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            }
        }

        private double DisplayScale
        {
            get
            {
                return CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.ScaledDensity;
            }
        }

        public double StatusBarHeight
        {
            get
            {
                var context = CrossCurrentActivity.Current.Activity;
                var resourceId = context.Resources.GetIdentifier("status_bar_height", "dimen", "android");
                if (resourceId > 0)
                {
                    return context.Resources.GetDimensionPixelSize(resourceId) / this.DisplayScale;
                }

                return 20.0;
            }
        }

        public bool CanVibrate
        {
            get
            {
                if ((int)Android.OS.Build.VERSION.SdkInt >= 11)
                {
                    using (var vibrator = (Android.OS.Vibrator)CrossCurrentActivity.Current.Activity.GetSystemService(Context.VibratorService))
                    {
                        return vibrator.HasVibrator;
                    }
                }

                return true;
            }
        }

        public void Vibration(TimeSpan? vibrateSpan = null)
        {
            using (var vibrator = (Android.OS.Vibrator)CrossCurrentActivity.Current.Activity.GetSystemService(Context.VibratorService))
            {
                if ((int)Android.OS.Build.VERSION.SdkInt >= 11)
                {
                    if (!vibrator.HasVibrator)
                    {
                        Console.WriteLine("Android device does not have vibrator.");
                        return;
                    }
                }

                var milliseconds = vibrateSpan.HasValue ? vibrateSpan.Value.TotalMilliseconds : 500;
                if (milliseconds < 0)
                {
                    milliseconds = 0;
                }

                try
                {
                    vibrator.Vibrate(VibrationEffect.CreateOneShot((long)milliseconds, VibrationEffect.DefaultAmplitude));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to vibrate Android device, ensure VIBRATE permission is set.\r\n{ex.Message}");
                }
            }
        }
    }
}