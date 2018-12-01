using Maempedia.Services;
using Maempedia.ViewModels.Map;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Map
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapDetailPage : ContentPage
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

        public MapDetailPageViewModel ViewModel;

        public MapDetailPage(Models.Owner owner)
        {
            InitializeComponent();
            
            this.ViewModel = new MapDetailPageViewModel(owner)
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;

            this.MyMap.MapStyle = MapStyle.FromJson(MAP_STYLE);
            this.MyMap.UiSettings.ZoomControlsEnabled = false;
            this.MyMap.UiSettings.MyLocationButtonEnabled = true;
            this.MyMap.UiSettings.RotateGesturesEnabled = false;
        }

        private async void OnMapMyLocationButtonClicked(object sender, MyLocationButtonClickedEventArgs e)
        {
            var position = await LocationService.GetCurrentLocation(this.ViewModel);
            if (position == null)
            {
                return;
            }

            this.MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(
                        position.Latitude,
                        position.Longitude),
                    Distance.FromMeters(
                        MAP_SPAN_RADIUS)));
        }

        protected override async void OnAppearing()
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
    }
}