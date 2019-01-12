using Maempedia.Models;
using Maempedia.Views.Owner;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Maempedia.ViewModels.Map
{
    public class MapPageViewModel : BaseViewModel
    {
        public ICommand OpenOwnerDetailCommand { get; private set; }

        private IList<Location> addressList = null;
        public IList<Location> AddressList
        {
            get { return this.addressList; }
            set { SetProperty<IList<Location>>(ref this.addressList, value); }
        }

        public MapPageViewModel()
        {
            this.OpenOwnerDetailCommand = new Command<Models.Owner>(this.OpenOwnerDetail);
        }

        public async Task<IList<Models.Owner>> LoadOwners(Position pos)
        {
            const int MAX_OWNERS = 20;

            return await this.WebApiService.Owner.GetNearbyOwners(pos.Latitude, pos.Longitude, MAX_OWNERS);
        }

        private async void OpenOwnerDetail(Models.Owner owner)
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            await this.NavigationService.NavigateTo(typeof(OwnerDetailPage), new object[] { owner });

            this.IsBusy = false;
        }
    }
}
