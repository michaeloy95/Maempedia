using Maempedia.Views.Menu;
using Maempedia.Views.RegisterSeller;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Information
{
    public class InformationPageViewModel : BaseViewModel
    {
        public ICommand ActionCommand { get; private set; }

        private string actionText = string.Empty;
        public string ActionText
        {
            get { return this.actionText; }
            set { SetProperty<string>(ref this.actionText, value); }
        }

        public InformationPageViewModel()
        {
            this.ActionCommand = new Command(this.Action);

            if (this.User.IsMaemseller)
            {
                this.ActionText = "Pasang iklan sekarang.";
            }
            else
            {
                this.ActionText = "Jadi Maemseller.";
            }
        }

        private async void Action()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            if (this.User.IsMaemseller)
            {
                await this.NavigationService.NavigateTo(typeof(MenuListingPage));
            }
            else
            {
                var maemsellerUser = this.User.GetUser();
                maemsellerUser.IsMaemseller = true;

                await this.NavigationService.NavigateTo(typeof(RestaurantRegisterSellerPage), new object[] { maemsellerUser });
            }

            this.IsBusy = false;
        }
    }
}
