using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Maempedia.Common
{
    public static class Utils
    {
        public static async Task<bool> CheckPermissions(Permission permission)
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            bool request = false;
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    string title, question, positive, negative;

                    switch (permission)
                    {
                        case Permission.Location:
                            title = $"Izin Akses Lokasi";
                            question = $"Maempedia membutuhkan izin akses Lokasi untuk menjelajahi menu kuliner di sekitar anda. Pergi ke jendela Settings untuk mengaktifkan akses.";
                            positive = "Settings";
                            negative = "Mungkin nanti";
                            break;
                        case Permission.MediaLibrary:
                            title = $"Izin Akses Media";
                            question = $"Maempedia membutuhkan izin akses Media untuk mengunggah foto melalui perangkat anda. Pergi ke jendela Settings untuk mengaktifkan akses.";
                            positive = "Settings";
                            negative = "Mungkin nanti";
                            break;
                        default:
                            title = $"Izin {permission}";
                            question = $"Maempedia membutuhkan izin akses {permission}. Pergi ke jendela Settings untuk mengaktifkan akses.";
                            positive = "Settings";
                            negative = "Mungkin nanti";
                            break;
                    }

                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }

                    return false;
                }

                request = true;
            }

            if (request || permissionStatus != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                if (newStatus.ContainsKey(permission) && newStatus[permission] != PermissionStatus.Granted)
                {
                    string title, question, positive, negative;

                    switch (permission)
                    {
                        case Permission.Location:
                            title = $"Izin Akses Lokasi";
                            question = $"Maempedia membutuhkan izin akses Lokasi untuk menjelajahi menu kuliner di sekitar anda. Pergi ke jendela Settings untuk mengaktifkan akses.";
                            positive = "Settings";
                            negative = "Mungkin nanti";
                            break;
                        case Permission.MediaLibrary:
                            title = $"Izin Akses Media";
                            question = $"Maempedia membutuhkan izin akses Media untuk mengunggah foto melalui perangkat anda. Pergi ke jendela Settings untuk mengaktifkan akses.";
                            positive = "Settings";
                            negative = "Mungkin nanti";
                            break;
                        default:
                            title = $"Izin {permission}";
                            question = $"Maempedia membutuhkan izin akses {permission}. Pergi ke jendela Settings untuk mengaktifkan akses.";
                            positive = "Settings";
                            negative = "Mungkin nanti";
                            break;
                    }

                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }
                    return false;
                }
            }

            return true;
        }
    }
}
