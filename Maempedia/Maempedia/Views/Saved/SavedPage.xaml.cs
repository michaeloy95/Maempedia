using Maempedia.Interfaces;
using Maempedia.ViewModels.Saved;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Saved
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedPage : ContentPage
    {
        public SavedPageViewModel ViewModel;

        private bool SyncItem = false;

        public SavedPage()
        {
            InitializeComponent();

            this.ViewModel = new SavedPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Models.Menu menu = e.SelectedItem as Models.Menu;
            if (menu == null)
                return;

            if (this.ViewModel.SelectMenuCommand.CanExecute(menu))
            {
                this.ViewModel.SelectMenuCommand.Execute(menu);
            }

            MenuListView.SelectedItem = null;
        }
        
        private void OnSearchClicked(object sender, System.EventArgs e)
        {
            this.SearchBar.IsVisible = !(this.SearchBar.IsVisible);

            if (this.SearchBar.IsVisible)
            {
                this.SearchBar.Focus();
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await this.ViewModel.LoadLocalItem();

            this.UpdateNullMenuLayout();

            if (!this.SyncItem)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    await this.ViewModel.SyncItem();
                }
                else
                {
                    DependencyService.Get<IMessageHelper>().ShortAlert($"Gagal memproses. Periksa kembali koneksi internet anda.");
                }
                this.SyncItem = true;
            }
        }

        public void UpdateNullMenuLayout()
        {
            if (this.ViewModel.MenuList == null || 
                this.ViewModel.MenuList?.Count == 0)
            {
                this.NullMenuLayout.IsVisible = true;
            }
            else
            {
                this.NullMenuLayout.IsVisible = false;
            }
        }
    }
}