using Maempedia.Common;
using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.Browse;
using Maempedia.Views.Profile;
using Maempedia.Views.Register;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Register
{
    public class ContactRegisterPageViewModel : BaseViewModel
    {
        public ICommand RegisterCommand { get; private set; }

        private Models.Owner curOwner = null;
        public Models.Owner CurOwner
        {
            get { return this.curOwner; }
            set { SetProperty<Models.Owner>(ref this.curOwner, value); }
        }

        private float imageAspectRatio;
        public float ImageAspectRatio
        {
            get { return this.imageAspectRatio; }
            set { SetProperty<float>(ref this.imageAspectRatio, value); }
        }

        private string contactText = string.Empty;
        public string ContactText
        {
            get { return this.contactText; }
            set { SetProperty<string>(ref this.contactText, value); }
        }

        private bool contactIsValid = false;
        public bool ContactIsValid
        {
            get { return this.contactIsValid; }
            set { SetProperty<bool>(ref this.contactIsValid, value); }
        }

        private bool contactIsChecking = false;
        public bool ContactIsChecking
        {
            get { return this.contactIsChecking; }
            set { SetProperty<bool>(ref this.contactIsChecking, value); }
        }

        private bool nextCommandEnabled = false;
        public bool NextCommandEnabled
        {
            get { return this.nextCommandEnabled; }
            set { SetProperty<bool>(ref this.nextCommandEnabled, value); }
        }

        public ContactRegisterPageViewModel(Models.Owner owner, float imageAspectRatio)
        {
            this.RegisterCommand = new Command(this.ProcessRegistration);

            this.CurOwner = owner;
            this.ImageAspectRatio = imageAspectRatio;
        }

        public void CheckValidity()
        {
            if (this.ContactIsValid)
            {
                this.NextCommandEnabled = true;
            }
            else
            {
                this.NextCommandEnabled = false;
            }
        }

        public async Task<bool> CheckContact()
        {
            this.ContactIsValid = false;

            if (String.IsNullOrEmpty(this.ContactText) ||
                String.IsNullOrWhiteSpace(this.ContactText))
            {
                return false;
            }

            const int MINIM_CONTACT_COUNT = 7;
            if (this.ContactText.Length < MINIM_CONTACT_COUNT)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Kontak harus terdiri dari {MINIM_CONTACT_COUNT} angka atau lebih.");
                return false;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                return false;
            }

            this.ContactIsChecking = true;
            var results = await AccountService.CheckContactIsValid(this.ContactText);
            this.ContactIsChecking = false;

            if (results != ServerResponseStatus.VALID)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"No. WhatsApp telah digunakan.");
                return false;
            }

            this.ContactIsValid = true;
            return true;
        }

        private async void ProcessRegistration()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            if (!this.NextCommandEnabled)
            {
                this.IsBusy = false;
                return;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memproses registrasi. Periksa kembali koneksi internet anda.");
                this.IsBusy = false;
                return;
            }
            
            this.CurOwner.ContactWA = this.ContactText;

            var loading = DependencyService.Get<ILoadingHelper>();

            loading.Show();

            var result = await AccountService.TryRegister(this.CurOwner.Username, this.CurOwner.Password, this.CurOwner.Email, this.CurOwner.ContactWA);

            if (result == ServerResponseStatus.VALID &&
                !String.IsNullOrEmpty(this.CurOwner.ProfilePicture))
            {
                var imageBytes = DependencyService.Get<IFileHelper>().ReadAllBytes(this.CurOwner.ProfilePicture ?? "");

                float width = Constant.MEDIA_PHOTO_PROFPIC_SIZE;
                float height = width * this.ImageAspectRatio;
                imageBytes = DependencyService.Get<IMediaHelper>().ResizeImage(imageBytes, width, height);

                result = await AccountService.UploadImage(imageBytes, this.User.ID);
            }

            loading.Hide();

            switch (result)
            {
                case ServerResponseStatus.INVALID:
                    await this.NavigationService.CurrentPage.DisplayAlert("Registrasi Gagal", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;

                case ServerResponseStatus.ERROR:
                    await this.NavigationService.CurrentPage.DisplayAlert("Registrasi Gagal", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;
            }
            
            this.User.SetLogin(true);
            
            ((App)App.Current).ReplaceTabPage(4, new LoggedProfilePage(), "ic_profile");
            ((App)App.Current).SwitchTab(4);

            await this.NavigationService.CurrentPage.Navigation.PopToRootAsync(false);
            await this.NavigationService.NavigateTo(typeof(IsMaemsellerRegisterPage));

            this.IsBusy = false;
        }
    }
}
