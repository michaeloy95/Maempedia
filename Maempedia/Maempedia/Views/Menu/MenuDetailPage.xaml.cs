using FFImageLoading.Forms;
using Maempedia.Interfaces;
using Maempedia.Models;
using Maempedia.ViewModels.Menu;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuDetailPage : ContentPage
    {
        private const int MAP_SPAN_RADIUS = 1000;

        public MenuDetailPageViewModel ViewModel;

        public MenuDetailPage(Models.Menu menu)
        {
            InitializeComponent();

            this.ViewModel = new MenuDetailPageViewModel(menu)
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;

            this.MyMap.UiSettings.MyLocationButtonEnabled = false;
            this.MyMap.UiSettings.RotateGesturesEnabled = false;
            this.MyMap.UiSettings.ScrollGesturesEnabled = false;
            this.MyMap.UiSettings.TiltGesturesEnabled = false;
            this.MyMap.UiSettings.ZoomControlsEnabled = false;
            this.MyMap.UiSettings.ZoomGesturesEnabled = false;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            this.ViewModel.RefreshPage();

            var savedMenu = await App.SavedMenuDatabase.GetMenuAsync(this.ViewModel.SelectedMenu.ID);
            if (savedMenu != null)
            {
                this.LikeImage.Source = "heart_full.png";
            }
            else
            {
                this.LikeImage.Source = "heart_blank.png";
            }

            var item = this.ViewModel.SelectedMenu;
            var hasDiscount = item.Discount != 0 && item.DiscountDaysLeft >= 0 && item.RemainingClaim > 0;
            this.PriceWithDiscountLayout.IsVisible = hasDiscount;
            this.PriceNoDiscount.IsVisible = !hasDiscount;
            this.DiscountedPrice.Text = String.Format(new CultureInfo("id-ID"), "Rp. {0:N}", item.Price - (item.Price * item.Discount));

            await Task.Delay(500); // workaround for #30 [Android]Map.Pins.Add doesn't work when page OnAppearing

            Pin pin = new Pin()
            {
                Type = PinType.Place,
                Label = this.ViewModel.SelectedMenu.Owner.Name,
                Address = this.ViewModel.SelectedMenu.Owner.Location.Address,
                Position = new Position(
                        this.ViewModel.SelectedMenu.Owner.Location.Latitude,
                        this.ViewModel.SelectedMenu.Owner.Location.Longitude),
                Flat = true,
                Icon = BitmapDescriptorFactory.DefaultMarker(Color.Orange)
            };
            this.MyMap.Pins.Add(pin);

            await this.MyMap.AnimateCamera(
                CameraUpdateFactory.NewCameraPosition(
                    new CameraPosition(
                        pin.Position,
                        15d,
                        0d,
                        0d)),
                TimeSpan.FromSeconds(2));

            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        private void OnOwnerTap(object sender, System.EventArgs e)
        {
            if (this.ViewModel.OpenOwnerCommand.CanExecute(null))
            {
                this.ViewModel.OpenOwnerCommand.Execute(null);
            }
        }

        private async void OnLikeImageTapped(object sender, EventArgs e)
        {
            var menu = this.ViewModel.SelectedMenu;
            if (menu == null)
            {
                return;
            }

            if ((this.LikeImage.Source as FileImageSource).File == "heart_full.png")
            {
                this.LikeImage.Source = "heart_blank.png";
                await App.SavedMenuDatabase.DeleteMenuAsync(new LocalMenu(menu));
                DependencyService.Get<IMessageHelper>().ShortAlert($"Unsaved {menu.Name}.");
            }
            else
            {
                this.LikeImage.Source = "heart_full.png";
                await App.SavedMenuDatabase.SaveMenuAsync(new LocalMenu(menu));
                DependencyService.Get<IMessageHelper>().ShortAlert($"Saved {menu.Name}.");
            }
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            if (this.ViewModel.OpenMapDetailCommand.CanExecute(null))
            {
                this.ViewModel.OpenMapDetailCommand.Execute(null);
            }
        }

        private void CommentEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != e.OldTextValue)
            {
                this.ViewModel.CheckComment();
            }
        }

        private void MenuImage_Success(object sender, CachedImageEvents.SuccessEventArgs e)
        {
            var info = e.ImageInformation;
            if (info == null)
            {
                return;
            }

            this.MenuImage.HeightRequest = this.MenuImage.WidthRequest * ((double)info.OriginalHeight / (double)info.OriginalWidth);
        }

        private void Claim_Clicked(object sender, ClickedEventArgs e)
        {
            this.ViewModel.ClaimMenu();
        }
    }
}