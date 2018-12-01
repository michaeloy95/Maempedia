using Maempedia.ViewModels.Profile;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewProfilePage : ContentPage
	{
        public ViewProfilePageViewModel ViewModel;

        public ViewProfilePage()
		{
			InitializeComponent ();

            this.ViewModel = new ViewProfilePageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.ViewModel?.RefreshUserDetails();

            if (this.ViewModel.UserIsMaemseller)
            {
                // Refresh Pins
                this.MyMap.Pins.Clear();
                this.MyMap.Pins.Add(
                    new Pin()
                    {
                        Type = PinType.Place,
                        Label = "Lokasi Saya",
                        Address = this.ViewModel.Address,
                        Position = this.ViewModel.Position,
                        Flat = true
                    });
            }
        }
    }
}