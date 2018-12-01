using Maempedia.ViewModels.Menu;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuListingPage : ContentPage
    {
        public MenuListingPageViewModel ViewModel;

        public MenuListingPage()
        {
            InitializeComponent();

            this.ViewModel = new MenuListingPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Models.Menu menu = e.SelectedItem as Models.Menu;
            if (menu == null)
                return;

            await this.Navigation.PushAsync(new MenuViewPage(menu), true);

            MenuListView.SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!App.User.MenuListFetched)
            {
                await this.ViewModel.LoadMenus();
                App.User.MenuListFetched = true;
            }
            else
            {
                this.ViewModel.LoadMenusLocally();
            }
        }
    }
}
