using Maempedia.Common;
using Maempedia.Interfaces;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Maempedia.Services
{
    public static class LocationService
    {
        private const int LOCATION_DESIRED_ACCURACY = 500;

        public static async Task<Position> GetCurrentLocation(BaseViewModel ViewModel)
        {
            try
            {
                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission)
                {
                    return null;
                }

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = LOCATION_DESIRED_ACCURACY;

                var position = await locator.GetPositionAsync(
                    TimeSpan.FromSeconds(10),
                    null,
                    false);

                if (position == null)
                {
                    await ViewModel.NavigationService.CurrentPage.DisplayAlert(
                        "Tidak Ada Lokasi",
                        "Lokasi anda tidak dapat ditemukan.",
                        "OK");
                    return null;
                }

                return position;
            }
            catch
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Lokasi anda tidak aktif.");

                return null;
            }
        }
    }
}
