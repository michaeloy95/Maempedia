using Maempedia.Services;
using Maempedia.ViewModels.RegisterSeller;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.RegisterSeller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddressRegisterSellerPage : ContentPage
    {
        private const string MAP_STYLE = "[\n" +
                                         "  {\n" +
                                         "    \"featureType\": \"administrative\",\n" +
                                         "    \"elementType\": \"geometry\",\n" +
                                         "    \"stylers\": [\n" +
                                         "       {\n" +
                                         "        \"visibility\": \"off\"\n" +
                                         "      }\n" +
                                         "    ]\n" +
                                         "  },\n" +
                                         "  {\n" +
                                         "    \"featureType\": \"poi\",\n" +
                                         "    \"stylers\": [\n" +
                                         "      {\n" +
                                         "        \"visibility\": \"off\"\n" +
                                         "      }\n" +
                                         "    ]\n" +
                                         "  },\n" +
                                         "  {\n" +
                                         "    \"featureType\": \"road\",\n" +
                                         "    \"elementType\": \"labels.icon\",\n" +
                                         "    \"stylers\": [\n" +
                                         "      {\n" +
                                         "        \"visibility\": \"off\"\n" +
                                         "      }\n" +
                                         "    ]\n" +
                                         "  },\n" +
                                         "  {\n" +
                                         "    \"featureType\": \"transit\",\n" +
                                         "    \"stylers\": [\n" +
                                         "      {\n" +
                                         "        \"visibility\": \"off\"\n" +
                                         "      }\n" +
                                         "    ]\n" +
                                         "  }\n" +
                                         "]\n";

        private const int MAP_SPAN_RADIUS = 1000;

        public AddressRegisterSellerPageViewModel ViewModel;

        public AddressRegisterSellerPage(Models.Owner owner, float imageAspectRatio, bool photoIsUploaded = false)
        {
            InitializeComponent();

            this.ViewModel = new AddressRegisterSellerPageViewModel(owner, imageAspectRatio, photoIsUploaded)
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;

            this.MyMap.MapStyle = MapStyle.FromJson(MAP_STYLE);
            this.MyMap.UiSettings.ZoomControlsEnabled = false;
            this.MyMap.UiSettings.MyLocationButtonEnabled = true;
            this.MyMap.UiSettings.RotateGesturesEnabled = false;

            Task.Run(async () =>
            {
                var myLocation = await LocationService.GetCurrentLocation(this.ViewModel);
                if (myLocation == null)
                {
                    return;
                }

                Position position = new Position(
                    myLocation.Latitude,
                    myLocation.Longitude);

                this.MyMap.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        position,
                        Distance.FromMeters(
                            MAP_SPAN_RADIUS)));
            });

            this.AddressEntry.Focus();
        }

        private void OnEntryFocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            entry.TextColor = Color.FromHex("#646464");
        }

        private async void OnAddressEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = await this.ViewModel.CheckAddressValidity();
            entry.TextColor = valid ? Color.FromHex("#646464")
                            : Color.FromHex("#CF000F");

            if (this.ViewModel.Point.Latitude != 0 ||
                this.ViewModel.Point.Longitude != 0)
            {
                this.CreatePin(this.ViewModel.Point);

                await this.MyMap.AnimateCamera(
                    CameraUpdateFactory.NewCameraPosition(
                        new CameraPosition(
                            this.ViewModel.Point,
                            15d,
                            0d,
                            0d)),
                    TimeSpan.FromSeconds(2));
            }

            this.ViewModel.CheckValidity();
        }

        private void OnEntryCompleted(object sender, System.EventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            entry.Unfocus();
        }

        private void OnMapClicked(object sender, Xamarin.Forms.GoogleMaps.MapClickedEventArgs e)
        {
            this.CreatePin(e.Point);
        }

        private async void OnMapMyLocationButtonClicked(object sender, Xamarin.Forms.GoogleMaps.MyLocationButtonClickedEventArgs e)
        {
            var position = await LocationService.GetCurrentLocation(this.ViewModel);
            if (position == null)
            {
                return;
            }

            var pos = new Position(position.Latitude, position.Longitude);
            CreatePin(pos);

            this.MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    pos,
                    Distance.FromMeters(
                        MAP_SPAN_RADIUS)));
        }

        private void CreatePin(Position pos)
        {
            this.MyMap.Pins.Clear();

            Pin locatedPin = new Pin()
            {
                Type = PinType.Place,
                Label = "Lokasi kuliner saya",
                Position = pos,
                Flat = true
            };
            this.MyMap.Pins.Add(locatedPin);

            this.ViewModel.SetPoint(pos);
            this.ViewModel.CheckValidity();
        }
        
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
            this.ViewModel.CheckValidity();

            await Task.Delay(500); // workaround for #30 [Android]Map.Pins.Add doesn't work when page OnAppearing

            var position = await LocationService.GetCurrentLocation(this.ViewModel);
            if (position == null)
            {
                return;
            }

            var pos = new Position(position.Latitude, position.Longitude);

            await this.MyMap.AnimateCamera(
                CameraUpdateFactory.NewCameraPosition(
                    new CameraPosition(
                        pos,
                        15d,
                        0d,
                        0d)),
                TimeSpan.FromSeconds(2));
        }
    }
}