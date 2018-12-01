using Maempedia.Models;
using Maempedia.Services;
using Maempedia.Views.Menu;
using Maempedia.Views.Saved;
using Plugin.Connectivity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Saved
{
    public class SavedPageViewModel : BaseViewModel
    {
        public ICommand RefreshCommand { get; private set; }

        public ICommand SelectMenuCommand { get; private set; }

        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetProperty<bool>(ref this.isRefreshing, value); }
        }
        
        private ObservableCollection<Models.Menu> menuList = null;
        public ObservableCollection<Models.Menu> MenuList
        {
            get { return this.menuList; }
            set { SetProperty<ObservableCollection<Models.Menu>>(ref this.menuList, value); }
        }

        public SavedPageViewModel()
        {
            this.RefreshCommand = new Command(async () => { await this.LoadLocalItem(); await this.SyncItem(); });
            this.SelectMenuCommand = new Command<Models.Menu>(this.SelectMenu);
        }

        public async Task LoadLocalItem()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            var list = await App.SavedMenuDatabase.GetMenusAsync();

            IList<Models.Menu> menuList = new List<Models.Menu>();
            foreach (LocalMenu savedMenu in list)
            {
                menuList.Add(new Models.Menu(savedMenu));
            }

            this.MenuList = new ObservableCollection<Models.Menu>(menuList);

            if (this.NavigationService.CurrentPage is SavedPage)
            { 
                ((SavedPage)this.NavigationService.CurrentPage).UpdateNullMenuLayout();
            }

            this.IsBusy = false;
        }

        public async Task SyncItem()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            this.IsRefreshing = true;

            var list = await App.SavedMenuDatabase.GetMenusAsync();
            if (list == null)
            {
                this.IsRefreshing = false;
                this.IsBusy = false;
                return;
            }

            foreach (LocalMenu menu in list)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(async () =>
                {
                    if (!CrossConnectivity.Current.IsConnected)
                    {
                        return;
                    }

                    Models.Menu newMenu = await MenuService.GetMenu(menu.ID);
                    if (newMenu == null)
                    {
                        return;
                    }

                    await App.SavedMenuDatabase.SaveMenuAsync(new LocalMenu(newMenu));

                    var curMenu = this.MenuList.Where(m => m.ID == newMenu.ID).FirstOrDefault();
                    if (curMenu == null)
                    {
                        return;
                    }

                    int idx = this.MenuList.IndexOf(curMenu);
                    this.MenuList[idx] = newMenu;
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }

            this.IsRefreshing = false;
            this.IsBusy = false;
        }

        private async void SelectMenu(Models.Menu menu)
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            await this.NavigationService.NavigateTo(typeof(MenuDetailPage), new object[] { menu });

            this.IsBusy = false;
        }
    }
}
