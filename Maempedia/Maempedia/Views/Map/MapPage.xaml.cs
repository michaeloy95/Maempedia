using Maempedia.Interfaces;
using Maempedia.Models;
using Maempedia.Services;
using Maempedia.ViewModels.Map;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Map
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
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

        private const int MINIM_SEARCH_TEXT = 3;

        private bool appeared = false;

        public MapPageViewModel ViewModel;

        public MapPage()
        {
            InitializeComponent();
            
            this.ViewModel = new MapPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;

            this.MyMap.MapStyle = MapStyle.FromJson(MAP_STYLE);
            this.MyMap.UiSettings.ZoomControlsEnabled = false;
            this.MyMap.UiSettings.MyLocationButtonEnabled = false;
            this.MyMap.UiSettings.RotateGesturesEnabled = false;
        }

        private void OnSearchEntryFocused(object sender, FocusEventArgs e)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            this.SearchLayout.IsVisible = true;
            this.MapLayout.IsVisible = false;
        }

        private void OnSearchEntryUnfocused(object sender, FocusEventArgs e)
        {
            NavigationPage.SetHasNavigationBar(this, true);
            this.SearchLayout.IsVisible = false;
            this.MapLayout.IsVisible = true;
        }

        private async void OnSearchEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.NewTextValue) ||
                String.IsNullOrWhiteSpace(e.NewTextValue) ||
                this.SearchEntry.Text.Length < MINIM_SEARCH_TEXT)
            {
                this.ViewModel.AddressList = new List<Location>();
                return;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                return;
            }

            AutoCompleteLocationResult results = null;

            try
            {
                results = await AutoCompleteLocationService.GetPlaces(e.NewTextValue);
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Error", ex.Message, "OK");
                this.ViewModel.AddressList = new List<Location>();
                return;
            }

            if (results == null)
            {
                this.ViewModel.AddressList = new List<Location>();
                return;
            }

            var newAddressList = new List<Location>();
            foreach (AutoCompleteLocation autoCompleteLocation in results.AutoCompletePlaces)
            {
                Location location = new Location()
                {
                    Address = autoCompleteLocation.Description,
                    Place_ID = autoCompleteLocation.Place_ID
                };
                
                newAddressList.Add(location);
            }

            this.ViewModel.AddressList = newAddressList;
        }

        private async void OnSearchEntryCompleted(object sender, EventArgs e)
        {
            this.SearchEntry.Unfocus();

            if (String.IsNullOrEmpty(this.SearchEntry.Text) ||
                String.IsNullOrWhiteSpace(this.SearchEntry.Text))
            {
                return;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                return;
            }

            AutoCompleteLocationResult results = null;

            try
            {
                results = await AutoCompleteLocationService.GetPlaces(this.SearchEntry.Text);
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Error", ex.Message, "OK");
                return;
            }

            if (results == null)
            {
                await this.DisplayAlert("Lokasi Tidak Ditemukan", "Geocoder tidak dapat menemukan lokasi yang anda tuju.", "OK");
                return;
            }

            this.MyMap.Pins.Clear();

            Location location = await AutoCompleteLocationService.GetPlace(results.AutoCompletePlaces.First().Place_ID);
            Position position = new Position(
                location.Latitude, 
                location.Longitude);

            Pin locatedPin = new Pin()
            {
                Type = PinType.Place,
                Label = this.SearchEntry.Text,
                Address = this.SearchEntry.Text,
                Position = position,
                Flat = true
            };
            this.MyMap.Pins.Add(locatedPin);

            this.MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    position, 
                    Distance.FromMeters(
                        MAP_SPAN_RADIUS)));

            var pins = await CreatePins(position);
            foreach (Pin pin in pins)
            {
                this.MyMap.Pins.Add(pin);
            }
        }

        private async void AddressListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Location location = (Location)e.SelectedItem;
            if (location == null)
                return;

            this.SearchEntry.Text = location.Address;
            this.SearchEntry.Unfocus();

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                return;
            }

            this.MyMap.Pins.Clear();

            location = await AutoCompleteLocationService.GetPlace(location.Place_ID);
            Position position = new Position(
                location.Latitude, 
                location.Longitude);

            Pin locatedPin = new Pin()
            {
                Type = PinType.Place,
                Label = this.SearchEntry.Text,
                Address = this.SearchEntry.Text,
                Position = position,
                Flat = true
            };
            this.MyMap.Pins.Add(locatedPin);

            this.MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    position, 
                    Distance.FromMeters(
                        MAP_SPAN_RADIUS)));

            var pins = await CreatePins(position);
            foreach (Pin pin in pins)
            {
                this.MyMap.Pins.Add(pin);
            }

            this.AddressListView.SelectedItem = null;
        }

        private async Task<IList<Pin>> CreatePins(Position pos)
        {
            var pins = new List<Pin>();
            var OwnerList = await this.ViewModel.LoadOwners(pos);
            if (OwnerList == null)
            {
                return pins;
            }

            foreach (Models.Owner owner in OwnerList)
            {
                Pin pin = new Pin()
                {
                    Type = PinType.SearchResult,
                    Label = owner.Name,
                    Address = owner.Location.Address,
                    Position = new Position(
                        owner.Location.Latitude,
                        owner.Location.Longitude),
                    Tag = owner,
                    Flat = true,
                    Icon = BitmapDescriptorFactory.DefaultMarker(Color.Orange)
                    //Icon = BitmapDescriptorFactory.FromView(new BindingPinPage(owner.ProfilePictureThumb))
                };

                pins.Add(pin);
            }

            return pins;
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

        private void OnMapInfoWindowClicked(object sender, InfoWindowClickedEventArgs e)
        {
            var owner = e.Pin.Tag as Models.Owner;
            if (owner == null)
            {
                return;
            }

            if (this.ViewModel.OpenOwnerDetailCommand.CanExecute(owner))
            {
                this.ViewModel.OpenOwnerDetailCommand.Execute(owner);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (this.SearchEntry.IsFocused)
            {
                this.SearchEntry.Unfocus();
                return true;
            }

            return base.OnBackButtonPressed();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (this.appeared)
            {
                return;
            }

            this.LoadingLayout.IsVisible = true;

            try
            {
                await Task.Delay(100); // workaround for #30 [Android]Map.Pins.Add doesn't work when page OnAppearing

                var myLocation = await LocationService.GetCurrentLocation(this.ViewModel);
                if (myLocation == null)
                {
                    return;
                }

                Position position = new Position(
                        myLocation.Latitude,
                        myLocation.Longitude);

                await this.MyMap.AnimateCamera(
                    CameraUpdateFactory.NewCameraPosition(
                        new CameraPosition(
                            position,
                            15d,
                            0d,
                            0d)),
                    TimeSpan.FromSeconds(1));

                this.MyMap.Pins.Clear();

                Pin locatedPin = new Pin()
                {
                    Type = PinType.Place,
                    Label = "Lokasi saya",
                    Position = position,
                    Flat = true
                };
                this.MyMap.Pins.Add(locatedPin);

                var pins = await CreatePins(position);
                this.LoadingLayout.IsVisible = true;
                foreach (Pin pin in pins)
                {
                    this.MyMap.Pins.Add(pin);
                }
            }
            catch
            {
            }

            this.appeared = true;

            this.LoadingLayout.IsVisible = false;
        }

        private async void OnMyMapClicked(object sender, MapClickedEventArgs e)
        {
            this.MyMap.Pins.Clear();

            Pin locatedPin = new Pin()
            {
                Type = PinType.Place,
                Label = "Lokasi terpilih",
                Position = e.Point,
                Flat = true
            };
            this.MyMap.Pins.Add(locatedPin);

            var pins = await CreatePins(e.Point);
            foreach (Pin pin in pins)
            {
                this.MyMap.Pins.Add(pin);
            }
        }
    }
}