using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.Register;
using Plugin.Connectivity;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Register
{
    public class AccountRegisterPageViewModel : BaseViewModel
    {
        public ICommand NextCommand { get; private set; }

        private string usernameText = string.Empty;
        public string UsernameText
        {
            get { return this.usernameText; }
            set { SetProperty<string>(ref this.usernameText, value); }
        }

        private string emailText = string.Empty;
        public string EmailText
        {
            get { return this.emailText; }
            set { SetProperty<string>(ref this.emailText, value); }
        }

        private string passwordText = string.Empty;
        public string PasswordText
        {
            get { return this.passwordText; }
            set { SetProperty<string>(ref this.passwordText, value); }
        }

        private string rePasswordText = string.Empty;
        public string RePasswordText
        {
            get { return this.rePasswordText; }
            set { SetProperty<string>(ref this.rePasswordText, value); }
        }

        private bool usernameIsValid = false;
        public bool UsernameIsValid
        {
            get { return this.usernameIsValid; }
            set { SetProperty<bool>(ref this.usernameIsValid, value); }
        }

        private bool emailIsValid = false;
        public bool EmailIsValid
        {
            get { return this.emailIsValid; }
            set { SetProperty<bool>(ref this.emailIsValid, value); }
        }

        private bool passwordIsValid = false;
        public bool PasswordIsValid
        {
            get { return this.passwordIsValid; }
            set { SetProperty<bool>(ref this.passwordIsValid, value); }
        }

        private bool rePasswordIsValid = false;
        public bool RePasswordIsValid
        {
            get { return this.rePasswordIsValid; }
            set { SetProperty<bool>(ref this.rePasswordIsValid, value); }
        }

        private bool usernameIsChecking = false;
        public bool UsernameIsChecking
        {
            get { return this.usernameIsChecking; }
            set { SetProperty<bool>(ref this.usernameIsChecking, value); }
        }

        private bool emailIsChecking = false;
        public bool EmailIsChecking
        {
            get { return this.emailIsChecking; }
            set { SetProperty<bool>(ref this.emailIsChecking, value); }
        }

        private bool nextCommandEnabled = false;
        public bool NextCommandEnabled
        {
            get { return this.nextCommandEnabled; }
            set { SetProperty<bool>(ref this.nextCommandEnabled, value); }
        }

        public AccountRegisterPageViewModel()
        {
            this.NextCommand = new Command(this.GoNext);
        }

        public void CheckValidity()
        {
            if (this.UsernameIsValid &&
                this.EmailIsValid &&
                this.PasswordIsValid &&
                this.RePasswordIsValid)
            {
                this.NextCommandEnabled = true;
            }
            else
            {
                this.NextCommandEnabled = false;
            }
        }

        public async Task<bool> CheckUsername()
        {
            this.UsernameIsValid = false;

            if (String.IsNullOrEmpty(this.UsernameText) ||
                String.IsNullOrWhiteSpace(this.UsernameText))
            {
                return false;
            }

            const int MINIM_USERNAME_COUNT = 3;
            if (this.UsernameText.Length < MINIM_USERNAME_COUNT)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Username harus terdiri dari {MINIM_USERNAME_COUNT} karakter atau lebih.");
                return false;
            }

            Regex r = new Regex("^[a-zA-Z0-9]*$");
            if (!r.IsMatch(this.UsernameText))
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Username hanya dapat terdiri dari huruf dan angka.");
                return false;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                return false;
            }

            this.UsernameIsChecking = true;
            var results = await this.WebApiService.Account.CheckUsernameIsValid(this.UsernameText);
            this.UsernameIsChecking = false;

            if (results != ServerResponseStatus.VALID)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Username telah digunakan.");
                return false;
            }

            this.UsernameIsValid = true;
            return true;
        }

        public async Task<bool> CheckEmail()
        {
            this.EmailIsValid = false;

            if (String.IsNullOrEmpty(this.EmailText) ||
                String.IsNullOrWhiteSpace(this.EmailText))
            {
                return false;
            }

            const int MINIM_EMAIL_COUNT = 3;
            if (this.EmailText.Length < MINIM_EMAIL_COUNT)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Format email salah.");
                return false;
            }
            
            if (!this.EmailText.Contains("@") ||
                !this.EmailText.Contains("."))
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Format email salah.");
                return false;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                return false;
            }

            this.EmailIsChecking = true;
            var results = await this.WebApiService.Account.CheckEmailIsValid(this.EmailText);
            this.EmailIsChecking = false;

            if (results != ServerResponseStatus.VALID)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Email telah terdaftar.");
                return false;
            }

            this.EmailIsValid = true;
            return true;
        }

        public bool CheckPassword()
        {
            this.PasswordIsValid = false;

            if (String.IsNullOrEmpty(this.PasswordText) ||
                String.IsNullOrWhiteSpace(this.PasswordText))
            {
                return false;
            }

            const int MINIM_PASS_COUNT = 6;
            if (this.PasswordText.Length < MINIM_PASS_COUNT)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Password harus terdiri dari {MINIM_PASS_COUNT} karakter atau lebih.");
                return false;
            }

            //if (Regex.Matches(this.PasswordText, @"[a-zA-Z]").Count <= 0 ||
            //    Regex.Matches(this.PasswordText, @"[0-9]").Count <= 0)
            //{
            //    DependencyService.Get<IMessageHelper>().LongAlert($"Password harus terdiri dari kombinasi huruf dan angka.");
            //    return false;
            //}

            this.PasswordIsValid = true;
            return true;
        }

        public bool CheckRePassword()
        {
            this.RePasswordIsValid = false;

            if (!this.PasswordIsValid ||
                String.IsNullOrEmpty(this.RePasswordText) ||
                String.IsNullOrWhiteSpace(this.RePasswordText))
            {
                return false;
            }

            if (this.RePasswordText != this.PasswordText)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Ulangi password tidak sesuai.");
                return false;
            }

            this.RePasswordIsValid = true;
            return true;
        }

        private async void GoNext()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            if (!this.NextCommandEnabled)
            {
                this.IsBusy = false;
                return;
            }

            var owner = new Models.Owner()
            {
                Username = this.UsernameText,
                Email = this.EmailText,
                Password = this.PasswordText,
                Location = new Models.Location()
            };
            
            await this.NavigationService.NavigateTo(typeof(ProfilePictureRegisterPage), new object[] { owner });

            this.IsBusy = false;
        }
    }
}
