using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Profile
{
    public class EditContactPageViewModel : BaseViewModel
    {
        public ICommand SaveCommand { get; private set; }

        private string contactText = string.Empty;
        public string ContactText
        {
            get { return this.contactText; }
            set { SetProperty<string>(ref this.contactText, value); }
        }

        private string waContactText = string.Empty;
        public string WAContactText
        {
            get { return this.waContactText; }
            set { SetProperty<string>(ref this.waContactText, value); }
        }
       
        private string emailText = string.Empty;
        public string EmailText
        {
            get { return this.emailText; }
            set { SetProperty<string>(ref this.emailText, value); }
        }

        private bool waContactIsValid = true;
        public bool WAContactIsValid
        {
            get { return this.waContactIsValid; }
            set { SetProperty<bool>(ref this.waContactIsValid, value); }
        }

        private bool emailIsValid = true;
        public bool EmailIsValid
        {
            get { return this.emailIsValid; }
            set { SetProperty<bool>(ref this.emailIsValid, value); }
        }

        private bool waContactIsChecking = false;
        public bool WAContactIsChecking
        {
            get { return this.waContactIsChecking; }
            set { SetProperty<bool>(ref this.waContactIsChecking, value); }
        }

        private bool emailIsChecking = false;
        public bool EmailIsChecking
        {
            get { return this.emailIsChecking; }
            set { SetProperty<bool>(ref this.emailIsChecking, value); }
        }

        private bool saveCommandEnabled = false;
        public bool SaveCommandEnabled
        {
            get { return this.saveCommandEnabled; }
            set { SetProperty<bool>(ref this.saveCommandEnabled, value); }
        }

        public EditContactPageViewModel()
        {
            this.SaveCommand = new Command(this.Save);
        }

        public void InitialiseData()
        {
            this.ContactText = this.User.ContactNumber;
            this.WAContactText = this.User.ContactWA;
            this.EmailText = this.User.Email;
        }

        public void CheckValidity()
        {
            if (!this.EmailIsValid)
            {
                this.SaveCommandEnabled = false;
                return;
            }

            if (this.ContactText == this.User.ContactNumber &&
                this.WAContactText == this.User.ContactWA &&
                this.EmailText == this.User.Email)
            {
                this.SaveCommandEnabled = false;
                return;
            }

            this.SaveCommandEnabled = true;
        }

        public async Task<bool> CheckWAContact()
        {
            this.WAContactIsValid = false;

            if (this.User.ContactWA == this.WAContactText)
            {
                return true;
            }

            if (String.IsNullOrEmpty(this.EmailText) ||
                String.IsNullOrWhiteSpace(this.EmailText))
            {
                return false;
            }

            const int MINIM_CONTACT_COUNT = 7;
            if (this.WAContactText.Length < MINIM_CONTACT_COUNT)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Kontak harus terdiri dari {MINIM_CONTACT_COUNT} angka atau lebih.");
                return false;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                return false;
            }

            this.WAContactIsChecking = true;
            var results = await AccountService.CheckContactIsValid(this.WAContactText);
            this.WAContactIsChecking = false;

            if (results != ServerResponseStatus.VALID)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"No. WhatsApp telah digunakan.");
                return false;
            }

            this.WAContactIsValid = true;
            return true;
        }

        public async Task<bool> CheckEmail()
        {
            this.EmailIsValid = false;

            if (this.User.Email == this.EmailText)
            {
                return true;
            }

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
            var results = await AccountService.CheckEmailIsValid(this.EmailText);
            this.EmailIsChecking = false;

            if (results != ServerResponseStatus.VALID)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Username telah digunakan.");
                return false;
            }

            this.EmailIsValid = true;
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
            owner.ContactNumber = this.WAContactText;
            owner.ContactWA = this.WAContactText;
            owner.Email = this.EmailText;

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

            DependencyService.Get<IMessageHelper>().LongAlert("Kontak telah diperbaharui.");

            await this.NavigationService.GoBack();

            this.IsBusy = false;
        }
    }
}
