using Maempedia.Views.RegisterSeller;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Register
{
    public class IsMaemsellerRegisterPageViewModel : BaseViewModel
    {
        public ICommand CompleteCommand { get; private set; }

        public ICommand RegisterMaemsellerCommand { get; private set; }

        public IsMaemsellerRegisterPageViewModel()
        {
            this.CompleteCommand = new Command(this.CompleteRegistration);
            this.RegisterMaemsellerCommand = new Command(this.GoToMaemsellerRegistration);
        }

        public void CompleteRegistration()
        {
            this.NavigationService.GoBack();
        }

        public void GoToMaemsellerRegistration()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            var maemsellerUser = this.User.GetUser();
            maemsellerUser.IsMaemseller = true;

            this.NavigationService.SwitchTo(typeof(RestaurantRegisterSellerPage), new object[] { maemsellerUser });

            this.IsBusy = false;
        }
    }
}
