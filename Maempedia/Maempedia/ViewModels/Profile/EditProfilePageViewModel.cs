using FFImageLoading;
using Maempedia.Common;
using Maempedia.Data;
using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Plugin.Connectivity;
using Plugin.Media;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Profile
{
    public class EditProfilePageViewModel : BaseViewModel
    {
        public ICommand UploadCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        private string imageSource;
        public string ImageSource
        {
            get { return this.imageSource; }
            set { SetProperty<string>(ref this.imageSource, value); }
        }

        public double ImageWidth
        {
            get { return (App.ScreenWidth - 150) * Settings.ImageQuality; }
        }

        private float imageAspectRatio;
        public float ImageAspectRatio
        {
            get { return this.imageAspectRatio; }
            set { SetProperty<float>(ref this.imageAspectRatio, value); }
        }

        private string nameText = string.Empty;
        public string NameText
        {
            get { return this.nameText; }
            set { SetProperty<string>(ref this.nameText, value); }
        }

        private string headlineText = string.Empty;
        public string HeadlineText
        {
            get { return this.headlineText; }
            set { SetProperty<string>(ref this.headlineText, value); }
        }

        private TimeSpan openingTime = new TimeSpan(9, 0, 0);
        public TimeSpan OpeningTime
        {
            get { return this.openingTime; }
            set
            {
                SetProperty<TimeSpan>(ref this.openingTime, value);
                this.CheckValidity();
            }
        }

        private TimeSpan closingTime = new TimeSpan(20, 0, 0);
        public TimeSpan ClosingTime
        {
            get { return this.closingTime; }
            set
            {
                SetProperty<TimeSpan>(ref this.closingTime, value);
                this.CheckValidity();
            }
        }

        private bool nameIsValid = true;
        public bool NameIsValid
        {
            get { return this.nameIsValid; }
            set { SetProperty<bool>(ref this.nameIsValid, value); }
        }

        private bool nameIsChecking = false;
        public bool NameIsChecking
        {
            get { return this.nameIsChecking; }
            set { SetProperty<bool>(ref this.nameIsChecking, value); }
        }

        private bool imageIsChanged = false;
        public bool ImageIsChanged
        {
            get { return this.imageIsChanged; }
            set { SetProperty<bool>(ref this.imageIsChanged, value); }
        }

        private bool saveCommandEnabled = false;
        public bool SaveCommandEnabled
        {
            get { return this.saveCommandEnabled; }
            set { SetProperty<bool>(ref this.saveCommandEnabled, value); }
        }

        public bool UserIsMaemseller
        {
            get { return this.User.IsMaemseller; }
        }

        public EditProfilePageViewModel()
        {
            this.UploadCommand = new Command(this.UploadImage);
            this.SaveCommand = new Command(this.Save);
        }

        public void InitialiseData()
        {
            this.ImageSource = this.User.ProfilePicture;
            this.NameText = this.User.Name;
            this.HeadlineText = this.User.Headline ?? "";

            if (this.User.IsMaemseller)
            {
                this.OpeningTime = TimeSpan.Parse(this.User.OpeningHour ?? "0");
                this.ClosingTime = TimeSpan.Parse(this.User.ClosingHour ?? "0");
            }
        }

        private async void UploadImage()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            var media = CrossMedia.Current;
            var file = await media.PickPhotoAsync();

            if (file == null)
            {
                this.IsBusy = false;
                return;
            }

            this.ImageSource = file.Path;
            this.ImageIsChanged = true;

            this.CheckValidity();

            this.IsBusy = false;
        }

        public void CheckValidity()
        {
            if (!this.NameIsValid)
            {
                this.SaveCommandEnabled = false;
                return;
            }

            if (this.NameText == this.User.Name &&
                this.ImageSource == this.User.ProfilePicture &&
                this.HeadlineText == this.User.Headline &&
                this.OpeningTime == TimeSpan.Parse(this.User.OpeningHour) &&
                this.ClosingTime == TimeSpan.Parse(this.User.ClosingHour))
            {
                this.SaveCommandEnabled = false;
                return;
            }

            this.SaveCommandEnabled = true;
        }

        public async Task<bool> CheckName()
        {
            this.NameIsValid = false;

            if (this.NameText == this.User.Name)
            {
                this.NameIsValid = true;
                return true;
            }

            if (String.IsNullOrEmpty(this.NameText) ||
                String.IsNullOrWhiteSpace(this.NameText))
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Nama tidak boleh kosong.");
                return false;
            }

            const int MINIM_NAME_COUNT = 5;
            if (this.NameText.Length < MINIM_NAME_COUNT)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Nama harus terdiri dari {MINIM_NAME_COUNT} karakter atau lebih.");
                return false;
            }

            Regex r = new Regex("^[a-zA-Z0-9]*$");
            if (!r.IsMatch(this.NameText.Replace(" ", string.Empty)))
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Nama hanya dapat terdiri dari huruf dan angka.");
                return false;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                return false;
            }

            this.NameIsChecking = true;
            var results = await this.WebApiService.Account.CheckNameIsValid(this.NameText);
            this.NameIsChecking = false;

            if (results != ServerResponseStatus.VALID)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Username telah digunakan.");
                return false;
            }

            this.NameIsValid = true;
            return true;
        }

        private async void Save()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            if (!this.saveCommandEnabled)
            {
                this.IsBusy = false;
                return;
            }

            var loading = DependencyService.Get<ILoadingHelper>();

            loading.Show();

            Models.Owner owner = this.User.GetUser();
            owner.Name = this.NameText;
            owner.Headline = this.HeadlineText;
            owner.OpeningHour = $"{this.OpeningTime:hh\\:mm}";
            owner.ClosingHour = $"{this.ClosingTime:hh\\:mm}";

            byte[] imageBytes = null;

            if (this.ImageIsChanged)
            {
                imageBytes = DependencyService.Get<IFileHelper>().ReadAllBytes(this.ImageSource);

                float width = Constant.MEDIA_PHOTO_MENUIMAGE_SIZE;
                float height = width * this.ImageAspectRatio;
                imageBytes = DependencyService.Get<IMediaHelper>().ResizeImage(imageBytes, width, height);
            }

            var result = await this.WebApiService.Account.UpdateAccount(owner, imageBytes);

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

            DependencyService.Get<IMessageHelper>().ShortAlert("Akun telah diperbaharui.");
            
            await ImageService.Instance.InvalidateCacheAsync(FFImageLoading.Cache.CacheType.All);

            await this.NavigationService.GoBack();

            this.IsBusy = false;
        }
    }
}
