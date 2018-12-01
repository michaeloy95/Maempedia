using Maempedia.ViewModels.Owner;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Owner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OwnerDetailPage : ContentPage
    {
        private const int MAP_SPAN_RADIUS = 2000;

        public OwnerDetailPageViewModel ViewModel;

        public OwnerDetailPage(Models.Owner owner)
        {
            InitializeComponent();

            this.ViewModel = new OwnerDetailPageViewModel(owner)
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

            await Task.Delay(100); // workaround for #30 [Android]Map.Pins.Add doesn't work when page OnAppearing

            Pin pin = new Pin()
            {
                Type = PinType.Place,
                Label = this.ViewModel.SelectedOwner.Name,
                Address = this.ViewModel.SelectedOwner.Location.Address,
                Position = new Position(
                        this.ViewModel.SelectedOwner.Location.Latitude,
                        this.ViewModel.SelectedOwner.Location.Longitude),
                Icon = BitmapDescriptorFactory.DefaultMarker(Color.Orange),
                Flat = true
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
        }

        private void MenuListView_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            Models.Menu menu = e.Item as Models.Menu;
            if (menu == null)
                return;

            if (this.ViewModel.SelectMenuCommand.CanExecute(menu))
            {
                this.ViewModel.SelectMenuCommand.Execute(menu);
            }
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            if (this.ViewModel.OpenMapDetailCommand.CanExecute(null))
            {
                this.ViewModel.OpenMapDetailCommand.Execute(null);
            }
        }
    }
}