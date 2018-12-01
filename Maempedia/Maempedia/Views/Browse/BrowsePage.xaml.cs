using Maempedia.Custom;
using Maempedia.Interfaces;
using Maempedia.ViewModels.Browse;
using Plugin.Connectivity;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Browse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrowsePage : ContentPage
    {
        public BrowsePageViewModel ViewModel;

        private int lastItemAppearedIdx = int.MaxValue;

        private bool IsFetchingData = false;

        private bool FetchedData = false;

        public BrowsePage()
        {
            InitializeComponent();

            this.ViewModel = new BrowsePageViewModel()
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

            if (currentIdx <= 1)
            {
                Parallel.Invoke(
                    () => this.AddMenuImageButton.TranslateTo(0, 0),
                    () => this.HeaderLayout.TranslateTo(0, 0),
                    () => this.MenuListLayout.TranslateTo(0, 100));
            }

            if (!this.ViewModel.ToggleFloatingButton)
            {
                return;
            }

            if (currentIdx > lastItemAppearedIdx)
            {
                Parallel.Invoke(
                    () => this.AddMenuImageButton.TranslateTo(0, 90),
                    () => this.HeaderLayout.TranslateTo(0, -100),
                    () => this.MenuListLayout.TranslateTo(0, 0));
            }
            else if (currentIdx < lastItemAppearedIdx)
            {
                Parallel.Invoke(
                    () => this.AddMenuImageButton.TranslateTo(0, 0),
                    () => this.HeaderLayout.TranslateTo(0, 0),
                    () => this.MenuListLayout.TranslateTo(0, 100));
            }

            if (currentIdx != lastItemAppearedIdx)
            {
                this.ViewModel.ToggleFloatingButton = false;
                await Task.Delay(500).ContinueWith(_ => this.ViewModel.ToggleFloatingButton = true);
            }

            this.lastItemAppearedIdx = currentIdx;
        }

        public void DisplayFloatingButtons()
        {
            this.AddMenuImageButton.TranslateTo(0, 0);
        }

        private void MenuListView_Scrolled(Point obj)
        {
            // TODO: Get point to work on scroll
            /*
            int headerHeight = 100;
            if (obj.Y <= 0)
            {
                this.HeaderLayout.TranslationY = 0;
                this.MenuListLayout.TranslationY = headerHeight;
            }
            else if (obj.Y <= headerHeight)
            {
                this.HeaderLayout.TranslationY = -obj.Y;
                this.MenuListLayout.TranslationY = -obj.Y + headerHeight;
            }
            else
            {
                this.HeaderLayout.TranslationY = -headerHeight;
                this.MenuListLayout.TranslationY = 0;
            }
            */
        }

        private void OnSortTypeButtonClicked(object sender, System.EventArgs e)
        {
            this.TerdekatButton.TextColor = Color.FromHex("#646464");
            this.TerdekatButton.BackgroundColor = Color.FromHex("#F8F8F8");
            this.TrendingButton.TextColor = Color.FromHex("#646464");
            this.TrendingButton.BackgroundColor = Color.FromHex("#F8F8F8");
            this.TerbaruButton.TextColor = Color.FromHex("#646464");
            this.TerbaruButton.BackgroundColor = Color.FromHex("#F8F8F8");

            var button = sender as FlatButton;
            button.TextColor = Color.White;
            button.BackgroundColor = Color.FromHex("#CF000F");

            if (this.ViewModel.MenuList?.Count > 0)
            {
                this.MenuListView.ScrollTo(this.ViewModel.MenuList[0], ScrollToPosition.Start, true);
            }
        }

        private void OnAddMenuTap(object sender, System.EventArgs e)
        {
            if (this.ViewModel.AddMenuCommand.CanExecute(null))
            {
                this.ViewModel.AddMenuCommand.Execute(null);
            }
        }

        private void OnMapClicked(object sender, System.EventArgs e)
        {
            if (this.ViewModel.OpenMapCommand.CanExecute(null))
            {
                this.ViewModel.OpenMapCommand.Execute(null);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            NavigationPage.SetHasNavigationBar(this.Parent, false);

            this.ViewModel.InitialiseProperties();

            this.lastItemAppearedIdx = int.MaxValue;

            await Task.Delay(100); // Bug with flying bottom bar

            if (!this.MenuListView.IsRefreshing)
            {
                this.MenuListView.IsRefreshing = true;
                this.MenuListView.IsRefreshing = false;
            }

            if (!this.FetchedData && !this.IsFetchingData)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    this.IsFetchingData = true;
                    
                    await this.ViewModel.RefreshItem();
                    this.FetchedData = true;
                    this.IsFetchingData = false;
                }
                else
                {
                    DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                }
            }

            this.ViewModel.ToggleFloatingButton = false;
            await Task.Run(async () =>
            {
                await Task.Delay(2000);
                this.ViewModel.ToggleFloatingButton = true;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            NavigationPage.SetHasNavigationBar(this.Parent, true);
        }
    }
}