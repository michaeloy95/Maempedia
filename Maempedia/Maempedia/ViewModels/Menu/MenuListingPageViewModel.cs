using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.Menu;
using Plugin.Connectivity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Menu
{
    public class MenuListingPageViewModel : BaseViewModel
    {
        public ICommand AddMenuCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        private ObservableCollection<Models.Menu> menuList = null;
        public ObservableCollection<Models.Menu> MenuList
        {
            get { return this.menuList; }
            set { SetProperty<ObservableCollection<Models.Menu>>(ref this.menuList, value); }
        }

        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetProperty<bool>(ref this.isRefreshing, value); }
        }

        private bool hasMenu;
        public bool HasMenu
        {
            get { return this.hasMenu; }
            set { SetProperty<bool>(ref this.hasMenu, value); }
        }

        private bool hasNoMenu;
        public bool HasNoMenu
        {
            get { return this.hasNoMenu; }
            set { SetProperty<bool>(ref this.hasNoMenu, value); }
        }

        public MenuListingPageViewModel()
        {
            this.AddMenuCommand = new Command(this.AddMenu);
            this.RefreshCommand = new Command(async () => await this.LoadMenus());

            this.HasMenu = (this.MenuList?.Count > 0) && (this.MenuList != null);
            this.HasNoMenu = !this.HasMenu;
        }

        public async Task LoadMenus()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                return;
            }

            this.IsRefreshing = true;

            var list = await MenuService.GetMenusFromOwnerId(this.User.GetUser());
            list = list ?? new List<Models.Menu>();

            this.User.SetMenuList(list);
            this.MenuList = new ObservableCollection<Models.Menu>(list);

            this.IsRefreshing = false;
            this.HasMenu = (this.MenuList?.Count > 0) && (this.MenuList != null);
            this.HasNoMenu = !this.HasMenu;
        }

        public void LoadMenusLocally()
        {
            this.MenuList = this.User.MenuList;

            this.HasMenu = (this.MenuList?.Count > 0) && (this.MenuList != null);
            this.HasNoMenu = !this.HasMenu;
        }

        private async void AddMenu()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;           

            await this.NavigationService.NavigateTo(typeof(AddMenuImagePage));

            this.IsBusy = false;
        }
    }
}
