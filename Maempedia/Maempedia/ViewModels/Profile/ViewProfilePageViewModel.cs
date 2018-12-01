using Maempedia.Views.Menu;
using Maempedia.Views.Profile;
using Maempedia.Views.RegisterSeller;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Maempedia.ViewModels.Profile
{
    public class ViewProfilePageViewModel : BaseViewModel
    {
        public ICommand EditProfileCommand { get; private set; }

        public ICommand EditContactCommand { get; private set; }

        public ICommand EditLocationCommand { get; private set; }

        public ICommand EditLoginCommand { get; private set; }

        public ICommand BecomeMaemsellerCommand { get; private set; }

        public ICommand ViewMyMenuCommand { get; private set; }

        private bool userIsMaemseller;
        public bool UserIsMaemseller
        {
            get { return this.userIsMaemseller; }
            set { SetProperty<bool>(ref this.userIsMaemseller, value); }
        }

        private bool userIsNotMaemseller;
        public bool UserIsNotMaemseller
        {
            get { return !this.userIsMaemseller; }
            set { SetProperty<bool>(ref this.userIsNotMaemseller, value); }
        }

        private string name;
        public string Name
        {
            get { return this.name; }
            set { SetProperty<string>(ref this.name, value); }
        }

        private string profilePictureThumb;
        public string ProfilePictureThumb
        {
            get { return this.profilePictureThumb; }
            set { SetProperty<string>(ref this.profilePictureThumb, value); }
        }

        private string email;
        public string Email
        {
            get { return this.email; }
            set { SetProperty<string>(ref this.email, value); }
        }

        private string headline;
        public string Headline
        {
            get { return this.headline; }
            set { SetProperty<string>(ref this.headline, value); }
        }

        private string contactNumber;
        public string ContactNumber
        {
            get { return this.contactNumber; }
            set { SetProperty<string>(ref this.contactNumber, value); }
        }

        private string contactWA;
        public string ContactWA
        {
            get { return this.contactWA; }
            set { SetProperty<string>(ref this.contactWA, value); }
        }

        private string address;
        public string Address
        {
            get { return this.address; }
            set { SetProperty<string>(ref this.address, value); }
        }

        private string username;
        public string Username
        {
            get { return this.username; }
            set { SetProperty<string>(ref this.username, value); }
        }

        private string workingHours;
        public string WorkingHours
        {
            get { return this.workingHours; }
            set { SetProperty<string>(ref this.workingHours, value); }
        }

        private Position position;
        public Position Position
        {
            get { return this.position; }
            set { this.SetProperty(ref this.position, value); }
        }

        private CameraUpdate mapCameraPosition;
        public CameraUpdate MapCameraPosition
        {
            get { return this.mapCameraPosition; }
            set { this.SetProperty(ref this.mapCameraPosition, value); }
        }

        public ViewProfilePageViewModel()
        {
            this.EditProfileCommand = new Command(this.OpenEditProfile);
            this.EditContactCommand = new Command(this.OpenEditContact);
            this.EditLocationCommand = new Command(this.OpenEditLocation);
            this.EditLoginCommand = new Command(this.OpenEditLogin);
            this.BecomeMaemsellerCommand = new Command(this.BecomeMaemseller);
            this.ViewMyMenuCommand = new Command(this.GotoMyMenu);

            this.RefreshUserDetails();
        }

        private async void OpenEditProfile()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            await this.NavigationService.NavigateTo(typeof(EditProfilePage));

            this.IsBusy = false;
        }

        private async void OpenEditContact()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            await this.NavigationService.NavigateTo(typeof(EditContactPage));

            this.IsBusy = false;
        }

        private async void OpenEditLocation()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(EditLocationPage));

            this.IsBusy = false;
        }

        private async void OpenEditLogin()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(EditAccountPage));

            this.IsBusy = false;
        }

        private async void BecomeMaemseller()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            var maemsellerUser = this.User.GetUser();
            maemsellerUser.IsMaemseller = true;

            await this.NavigationService.NavigateTo(typeof(RestaurantRegisterSellerPage), new object[] { maemsellerUser });

            this.IsBusy = false;
        }

        private void GotoMyMenu()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            this.NavigationService.NavigateTo(typeof(MenuListingPage), null);

            this.IsBusy = false;
        }

        public void RefreshUserDetails()
        {
            this.UserIsMaemseller = this.User.IsMaemseller;
            this.UserIsNotMaemseller = !this.User.IsMaemseller;
            this.Name = this.User.Name;
            this.ProfilePictureThumb = this.User.ProfilePictureThumb;
            this.Email = this.User.Email;
            this.ContactNumber = this.User.ContactNumber;
            this.ContactWA = this.User.ContactWA;
            this.Headline = this.User.Headline;
            this.Username = this.User.Username;

            // Position of center Surabaya
            this.Position = new Position(
                    -7.25,
                    112.75);

            if (this.UserIsMaemseller)
            {
                this.WorkingHours = $"{this.User.OpeningHour.ToString()} - {this.User.ClosingHour.ToString()}";
                this.Address = this.User.Address;
                this.Position = new Position(
                    this.User.Latitude,
                    this.User.Longitude);
            }

            this.MapCameraPosition = CameraUpdateFactory.NewCameraPosition(
                    new CameraPosition(
                        this.Position,
                        17));
        }
    }
}
