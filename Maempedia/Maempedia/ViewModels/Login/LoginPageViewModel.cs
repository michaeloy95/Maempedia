using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.Browse;
using Maempedia.Views.Menu;
using Maempedia.Views.Profile;
using Plugin.Connectivity;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Login
{
    public class LoginPageViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; private set; }

        public ICommand ForgotPasswordCommand { get; private set; }

        private string usernameText = string.Empty;
        public string UsernameText
        {
            get { return this.usernameText; }
            set { SetProperty<string>(ref this.usernameText, value); }
        }

        private string passwordText = string.Empty;
        public string PasswordText
        {
            get { return this.passwordText; }
            set { SetProperty<string>(ref this.passwordText, value); }
        }

        public LoginPageViewModel()
        {
            this.LoginCommand = new Command(this.ProcessLogin);
            this.ForgotPasswordCommand = new Command(this.OpenForgotPasswordWindow);
        }

        public async void ProcessLogin()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal masuk. Periksa kembali koneksi internet anda.");
                this.IsBusy = false;
                return;
            }

            if (String.IsNullOrEmpty(this.UsernameText) ||
                String.IsNullOrWhiteSpace(this.UsernameText))
            {
                DependencyService.Get<IMessageHelper>().ShortAlert($"Username tidak boleh kosong");
                this.IsBusy = false;
                return;
            }

            if(String.IsNullOrEmpty(this.PasswordText) ||
                String.IsNullOrWhiteSpace(this.PasswordText))
            {
                DependencyService.Get<IMessageHelper>().ShortAlert($"Password tidak boleh kosong");
                this.IsBusy = false;
                return;
            }

            var loading = DependencyService.Get<ILoadingHelper>();

            loading.Show();
            var result = await AccountService.TryLogin(this.UsernameText, this.PasswordText);
            loading.Hide();

            switch (result)
            {
                case ServerResponseStatus.INVALID:
                    await this.NavigationService.CurrentPage.DisplayAlert("Gagal Masuk", "Username atau password anda salah.", "OK");
                    this.IsBusy = false;
                    return;

                case ServerResponseStatus.ERROR:
                    await this.NavigationService.CurrentPage.DisplayAlert("Gagal Masuk", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;
            }

            DependencyService.Get<IMessageHelper>().ShortAlert("Berhasil masuk");
            this.User.SetLogin(true);

            // Organise and direct user to the Browse Page
            ((App)App.Current).ReplaceTabPage(4, new LoggedProfilePage(), "ic_profile");

            await this.NavigationService.GoBack();

            this.IsBusy = false;
        }

        private void OpenForgotPasswordWindow()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            string url = $"https://maempedia.com/lupapassword.html";
            Device.OpenUri(new Uri(url));

            this.IsBusy = false;
        }
    }
}
