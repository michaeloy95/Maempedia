using Maempedia.Views.Login;
using Maempedia.Views.Register;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Profile
{
    public class UnloggedProfilePageViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; private set; }

        public ICommand RegisterCommand { get; private set; }

        public UnloggedProfilePageViewModel()
        {
            this.LoginCommand = new Command(this.GotoLogin);
            this.RegisterCommand = new Command(this.GotoRegister);
        }

        private async void GotoLogin()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(LoginPage));

            this.IsBusy = false;
        }

        private async void GotoRegister()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            await this.NavigationService.NavigateTo(typeof(AccountRegisterPage));

            this.IsBusy = false;
        }
    }
}
