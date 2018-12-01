using Maempedia.ViewModels.Browse;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Browse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPageViewModel ViewModel;

        private int lastItemAppearedIdx = int.MaxValue;

        public SearchPage()
        {
            InitializeComponent();

            this.ViewModel = new SearchPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;

            this.MenuListView.ItemAppearing += this.MenuListView_ItemAppearing;
            this.MenuListView.Scrolled += this.MenuListView_Scrolled;
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

            this.MenuListView.SelectedItem = null;
        }

        private async void MenuListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            int currentIdx = ViewModel.MenuList.IndexOf((Models.Menu)e.Item);

            if (this.ViewModel.MenuList.Count <= currentIdx + 3)
            {
                await this.ViewModel.LoadMoreMenu();
            }

            this.lastItemAppearedIdx = currentIdx;
        }

        private void MenuListView_Scrolled(Point obj)
        {
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.SearchEntry.Focus();

            this.lastItemAppearedIdx = int.MaxValue;
        }
    }
}