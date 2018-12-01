using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Plugin.Connectivity;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Profile
{
    public class EditAccountPageViewModel : BaseViewModel
    {
        public ICommand SaveCommand { get; private set; }

        private string usernameText = string.Empty;
        public string UsernameText
        {
            get { return this.usernameText; }
            set { SetProperty<string>(ref this.usernameText, value); }
        }

        private string oldPasswordText = string.Empty;
        public string OldPasswordText
        {
            get { return this.oldPasswordText; }
            set { SetProperty<string>(ref this.oldPasswordText, value); }
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

        private bool usernameIsValid = true;
        public bool UsernameIsValid
        {
            get { return this.usernameIsValid; }
            set { SetProperty<bool>(ref this.usernameIsValid, value); }
        }

        private bool oldPasswordIsValid = false;
        public bool OldPasswordIsValid
        {
            get { return this.oldPasswordIsValid; }
            set { SetProperty<bool>(ref this.oldPasswordIsValid, value); }
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

        private bool saveCommandEnabled = false;
        public bool SaveCommandEnabled
        {
            get { return this.saveCommandEnabled; }
            set { SetProperty<bool>(ref this.saveCommandEnabled, value); }
        }

        public EditAccountPageViewModel()
        {
            this.SaveCommand = new Command(this.Save);
            this.UsernameText = this.User.Username;
        }

        public void CheckValidity()
        {
            if (this.UsernameIsValid &&
                this.OldPasswordIsValid &&
                this.PasswordIsValid &&
                this.RePasswordIsValid)
            {
                this.SaveCommandEnabled = true;
            }
            else
            {
                this.SaveCommandEnabled = false;
            }
        }

        public async Task<bool> CheckUsername()
        {
            this.UsernameIsValid = false;

            if (this.User.Username == this.UsernameText)
            {
                return true;
            }

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
            var results = await AccountService.CheckUsernameIsValid(this.UsernameText);
            this.UsernameIsChecking = false;

            if (results != ServerResponseStatus.VALID)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Username telah digunakan.");
                return false;
            }

            this.UsernameIsValid = true;
            return true;
        }

        public bool CheckOldPassword()
        {
            this.OldPasswordIsValid = false;

            if (String.IsNullOrEmpty(this.OldPasswordText) ||
                String.IsNullOrWhiteSpace(this.OldPasswordText))
            {
                return false;
            }

            if (this.User.Password != this.OldPasswordText)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Password tidak sesuai.");
                return false;
            }

            this.OldPasswordIsValid = true;
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

            if (this.User.Password == this.PasswordText)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Password baru tidak boleh sama dengan password lama.");
                return false;
            }

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

        private async void Save()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            if (!this.SaveCommandEnabled)
            {
                this.IsBusy = false;
                return;
            }

            var loading = DependencyService.Get<ILoadingHelper>();

            loading.Show();

            Models.Owner owner = this.User.GetUser();
            owner.Username = this.UsernameText;
            owner.Password = this.PasswordText;

            var result = await AccountService.UpdateAccount(owner, null);

            loading.Hide();

            switch (result)
            {
                case ServerResponseStatus.INVALID:
                    await this.NavigationService.CurrentPage.DisplayAlert("Pembaharuan Gagal", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;

                case ServerResponseStatus.ERROR:
                    await this.NavigationService.CurrentPage.DisplayAlert("Pembaharuan Gagal", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;
            }

            this.User.SetUser(owner);

            DependencyService.Get<IMessageHelper>().LongAlert("Akun telah diperbaharui.");

            await this.NavigationService.GoBack();

            this.IsBusy = false;
        }
    }
}
