using Xamarin.Forms;

namespace Maempedia.Data
{
    public static class Settings
    {
        private const string ImageQualityKey = "ImageQuality";

        public const double ImageQualityDefaultValue = 1;

        public static double ImageQuality
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ImageQualityKey)
                                    ? (double)Application.Current.Properties[ImageQualityKey]
                                    : ImageQualityDefaultValue;
            }
            set
            {
                Application.Current.Properties[ImageQualityKey] = value;
            }
        }


        private const string AllowPushNotificationKey = "PushNotification";

        public const bool AllowPushNotificationDefaultValue = true;

        public static bool AllowPushNotification
        {
            get
            {
                return Application.Current.Properties.ContainsKey(AllowPushNotificationKey)
                                    ? (bool)Application.Current.Properties[AllowPushNotificationKey]
                                    : AllowPushNotificationDefaultValue;
            }
            set
            {
                Application.Current.Properties[AllowPushNotificationKey] = value;
            }
        }
    }
}