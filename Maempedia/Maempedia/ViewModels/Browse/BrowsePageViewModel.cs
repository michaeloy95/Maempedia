using FFImageLoading;
using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.Browse;
using Maempedia.Views.Map;
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
    public class BrowsePageViewModel : BaseViewModel
    {
        public ICommand RefreshCommand { get; private set; }

        public ICommand GotoSearchPageCommand { get; private set; }

        public ICommand NearbyCommand { get; private set; }

        public ICommand TrendingCommand { get; private set; }

        public ICommand LatestCommand { get; private set; }

        public ICommand SelectMenuCommand { get; private set; }

        public ICommand AddMenuCommand { get; private set; }

        public ICommand OpenMapCommand { get; private set; }

        public bool ToggleFloatingButton = false;

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

        private SortMenuBy sortBy;
        public SortMenuBy SortBy
        {
            get { return this.sortBy; }
            set { SetProperty<SortMenuBy>(ref this.sortBy, value); }
        }

        private ObservableCollection<Models.Menu> menuList = null;
        public ObservableCollection<Models.Menu> MenuList
        {
            get { return this.menuList; }
            set { SetProperty<ObservableCollection<Models.Menu>>(ref this.menuList, value); }
        }

        //private string searchEntryText = string.Empty;
        //public string SearchEntryText
        //{
        //    get { return this.searchEntryText; }
        //    set { SetProperty<string>(ref this.searchEntryText, value); }
        //}

        private bool userIsMaemseller;
        public bool UserIsMaemseller
        {
            get { return this.userIsMaemseller; }
            set { SetProperty<bool>(ref this.userIsMaemseller, value); }
        }

        public BrowsePageViewModel()
        {
            this.SortBy = SortMenuBy.Nearby;
            this.GotoSearchPageCommand = new Command(this.GotoSearchPage);
            this.NearbyCommand = new Command(async () => await this.RefreshNearby());
            this.TrendingCommand = new Command(async () => await this.RefreshTrending());
            this.LatestCommand = new Command(async () => await this.RefreshLatest());
            this.SelectMenuCommand = new Command<Models.Menu>(this.SelectMenu);
            this.AddMenuCommand = new Command(this.AddMenu);
            this.OpenMapCommand = new Command(this.OpenMap);

            this.RefreshCommand = new Command(
                async () =>
                {
                    await this.RefreshItem();
                });
        }

        public void InitialiseProperties()
        {
            this.UserIsMaemseller = this.User.IsMaemseller;
        }

        public async Task RefreshItem()
        {
            this.IsRefreshing = true;
            this.ToggleFloatingButton = false;

            try
            {
                if (this.NavigationService.CurrentPage is BrowsePage)
                {
                    ((BrowsePage)this.NavigationService.CurrentPage).DisplayFloatingButtons();
                }
            }
            catch
            {
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                this.IsRefreshing = false;
                return;
            }

            try
            {
                this.MyPosition = this.MyPosition ?? await LocationService.GetCurrentLocation(this);
                
                var list = this.MyPosition == null ? await this.WebApiService.Menu.GetMenus(1, this.SortBy, string.Empty)
                    : await this.WebApiService.Menu.GetMenus(1, this.SortBy, string.Empty, this.MyPosition.Latitude, this.MyPosition.Longitude);
                
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
            
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(async () =>
            {
                await Task.Delay(2000);
                this.ToggleFloatingButton = true;
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            this.IsRefreshing = false;
        }

        //private async Task SearchItem()
        //{
        //    this.SortBy = BrowsePage.SortBy.Search;
        //    await this.RefreshItem(this.SearchEntryText);
        //}

        private async Task RefreshNearby()
        {
            this.SortBy = SortMenuBy.Nearby;
            await this.RefreshItem();
        }

        private async Task RefreshTrending()
        {
            this.SortBy = SortMenuBy.Trending;
            await this.RefreshItem();
        }

        private async Task RefreshLatest()
        {
            this.SortBy = SortMenuBy.Latest;
            await this.RefreshItem();
        }

        private async void SelectMenu(Models.Menu menu)
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(MenuDetailPage), new object[] { menu });

            this.IsBusy = false;
        }

        private async void GotoSearchPage()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            await this.NavigationService.NavigateTo(typeof(SearchPage));

            this.IsBusy = false;
        }

        private async void AddMenu()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(MenuListingPage));

            this.IsBusy = false;
        }

        private async void OpenMap()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            await this.NavigationService.NavigateTo(typeof(MapPage));

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

                var moreMenuList = this.MyPosition == null ? await this.WebApiService.Menu.GetMenus(this.MenuList.Count + 1, this.SortBy, string.Empty)
                        : await this.WebApiService.Menu.GetMenus(this.MenuList.Count + 1, this.SortBy, string.Empty, this.MyPosition.Latitude, this.MyPosition.Longitude);
                
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
