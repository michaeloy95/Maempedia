using AudioToolbox;
using Foundation;
using Maempedia.Interfaces;
using Maempedia.iOS.Utilities;
using System;
using System.Reflection;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceUtility))]
namespace Maempedia.iOS.Utilities
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
                return UIDevice.CurrentDevice.Name;
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
                return NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
            }
        }

        public string BuildVersion
        {
            get
            {
                return NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
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
                return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
        }

        public double StatusBarHeight
        {
            get
            {
                return UIApplication.SharedApplication.StatusBarFrame.Height;
            }
        }

        public bool CanVibrate => true;

        public void Vibration(TimeSpan? vibrateSpan = null)
        {
            SystemSound.Vibrate.PlaySystemSound();
        }
    }
}