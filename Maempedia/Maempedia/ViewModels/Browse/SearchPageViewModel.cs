using FFImageLoading;
using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.Menu;
using Plugin.Connectivity;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Browse
{
    public class SearchPageViewModel : BaseViewModel
    {
        public ICommand SearchCommand { get; private set; }

        public ICommand SelectMenuCommand { get; private set; }

        private Position myPosition = null;
        public Position MyPosition
        {
            get { return this.myPosition; }
            set { SetProperty<Position>(ref this.myPosition, value); }
        }

        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetProperty<bool>(ref this.isRefreshing, value); }
        }

        private bool isLoadingMore = false;
        public bool IsLoadingMore
        {
            get { return this.isLoadingMore; }
            set { SetProperty<bool>(ref this.isLoadingMore, value); }
        }

        private ObservableCollection<Models.Menu> menuList = null;
        public ObservableCollection<Models.Menu> MenuList
        {
            get { return this.menuList; }
            set { SetProperty<ObservableCollection<Models.Menu>>(ref this.menuList, value); }
        }

        private string searchEntryText = string.Empty;
        public string SearchEntryText
        {
            get { return this.searchEntryText; }
            set { SetProperty<string>(ref this.searchEntryText, value); }
        }

        public SearchPageViewModel()
        {

            this.SelectMenuCommand = new Command<Models.Menu>(this.SelectMenu);
            this.SearchCommand = new Command(
                async () =>
                {
                    await this.RefreshItem(this.SearchEntryText);
                });
        }

        public async Task RefreshItem(string keyword)
        {
            this.IsRefreshing = true;

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                this.IsRefreshing = false;
                return;
            }

            try
            {
                this.MyPosition = this.MyPosition ?? await LocationService.GetCurrentLocation(this);

                var list = this.MyPosition == null ? await this.WebApiService.Menu.GetMenus(1, SortMenuBy.Search, keyword)
                    : await this.WebApiService.Menu.GetMenus(1, SortMenuBy.Search, keyword, this.MyPosition.Latitude, this.MyPosition.Longitude);

                if (list == null)
                {
                    DependencyService.Get<IMessageHelper>().LongAlert($"Terjadi Kesalahan.");
                    this.IsRefreshing = false;
                    return;
                }

                await ImageService.Instance.InvalidateCacheAsync(FFImageLoading.Cache.CacheType.All);
                this.MenuList = new ObservableCollection<Models.Menu>(list);
            }
            catch (Exception ex)
            {
                await this.NavigationService.CurrentPage.DisplayAlert("Terjadi Kesalahan", $"Error: {ex.Message}", "OK");
            }

            this.IsRefreshing = false;
        }

        private async void SelectMenu(Models.Menu menu)
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            await this.NavigationService.NavigateTo(typeof(MenuDetailPage), new object[] { menu });

            this.IsBusy = false;
        }

        public async Task LoadMoreMenu()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                this.IsRefreshing = false;
                this.IsBusy = false;
                return;
            }

            try
            {
                this.IsLoadingMore = true;

                var moreMenuList = this.MyPosition == null ? await this.WebApiService.Menu.GetMenus(this.MenuList.Count + 1, SortMenuBy.Search, this.SearchEntryText)
                        : await this.WebApiService.Menu.GetMenus(this.MenuList.Count + 1, SortMenuBy.Search, this.SearchEntryText, this.MyPosition.Latitude, this.MyPosition.Longitude);

                if (moreMenuList == null)
                {
                    DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                    this.IsLoadingMore = false;
                    return;
                }

                foreach (Models.Menu menu in moreMenuList)
                {
                    this.MenuList.Add(menu);
                }

                this.IsLoadingMore = false;
            }
            catch (Exception ex)
            {
                await this.NavigationService.CurrentPage.DisplayAlert("Terjadi Kesalahan", $"Error: {ex.Message}", "OK");
            }

            this.IsBusy = false;
        }
    }
}
