using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.Map;
using Maempedia.Views.Menu;
using Plugin.Connectivity;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Owner
{
    public class OwnerDetailPageViewModel : BaseViewModel
    {
        public ICommand SelectMenuCommand { get; private set; }

        public ICommand OpenWhatsAppCommand { get; private set; }

        public ICommand ShareCommand { get; private set; }

        public ICommand OpenMapDetailCommand { get; private set; }

        private Models.Owner selectedOwner = null;
        public Models.Owner SelectedOwner
        {
            get { return this.selectedOwner; }
            set { SetProperty<Models.Owner>(ref this.selectedOwner, value); }
        }

        private ObservableCollection<Models.Menu> menuList = null;
        public ObservableCollection<Models.Menu> MenuList
        {
            get { return this.menuList; }
            set { SetProperty<ObservableCollection<Models.Menu>>(ref this.menuList, value); }
        }

        public string WorkingHours
        {
            get { return $"{SelectedOwner.OpeningHour} - {SelectedOwner.ClosingHour}"; }
        }

        private bool hasMenu = false;
        public bool HasMenu
        {
            get { return this.hasMenu; }
            set { SetProperty<bool>(ref this.hasMenu, value); }
        }

        private bool hasNoMenu = false;
        public bool HasNoMenu
        {
            get { return this.hasNoMenu; }
            set { SetProperty<bool>(ref this.hasNoMenu, value); }
        }

        public OwnerDetailPageViewModel(Models.Owner owner)
        {
            this.SelectedOwner = owner;
            this.SelectMenuCommand = new Command<Models.Menu>(this.SelectMenu);
            this.OpenWhatsAppCommand = new Command(this.OpenWhatsApp);
            this.ShareCommand = new Command(this.ShareMenu);
            this.OpenMapDetailCommand = new Command(this.OpenMapDetail);
            Task.Run(async () => 
            {
                var list = await LoadMenus(owner);
                if (list == null)
                {
                    return;
                }

                this.MenuList = new ObservableCollection<Models.Menu>(list);

                this.HasMenu = ((this.MenuList?.Count > 0) && (this.MenuList != null)) ? true
                             : false;
                this.HasNoMenu = !this.HasMenu;
            }).Wait();
        }

        private async Task<IList<Models.Menu>> LoadMenus(Models.Owner owner)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                return new List<Models.Menu>();
            }

            return await MenuService.GetMenusFromOwnerId(owner);
        }

        private async void SelectMenu(Models.Menu menu)
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(MenuDetailPage), new object[] { menu });

            this.IsBusy = false;
        }

        private void OpenWhatsApp()
        {
            string url = $"https://api.whatsapp.com/send?phone={this.SelectedOwner.ContactWA}";
            Device.OpenUri(new Uri(url));
        }

        private void ShareMenu()
        {
            CrossShare.Current.Share(new ShareMessage
            {
                Text = $"{this.SelectedOwner.Name} - {this.SelectedOwner.Headline}",
                Title = $"{this.SelectedOwner.Name}",
                Url = $"http://maempedia.com/idm/{this.SelectedOwner.Username}"
            });
        }

        private async void OpenMapDetail()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(MapDetailPage), new object[] { this.SelectedOwner });

            this.IsBusy = false;
        }
    }
}
