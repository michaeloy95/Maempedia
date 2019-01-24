using Maempedia.Common;
using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.Menu;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Maempedia.ViewModels.RegisterSeller
{
    public class AddressRegisterSellerPageViewModel : BaseViewModel
    {
        public ICommand NextCommand { get; private set; }

        private Models.Owner curOwner = null;
        public Models.Owner CurOwner
        {
            get { return this.curOwner; }
            set { SetProperty<Models.Owner>(ref this.curOwner, value); }
        }

        private string addressText = string.Empty;
        public string AddressText
        {
            get { return this.addressText; }
            set { SetProperty<string>(ref this.addressText, value); }
        }

        private Position point = new Position(0, 0);
        public Position Point
        {
            get { return this.point; }
            set { SetProperty<Position>(ref this.point, value); }
        }

        private bool addressIsValid = false;
        public bool AddressIsValid
        {
            get { return this.addressIsValid; }
            set { SetProperty<bool>(ref this.addressIsValid, value); }
        }

        private bool pointIsValid = false;
        public bool PointIsValid
        {
            get { return this.pointIsValid; }
            set { SetProperty<bool>(ref this.pointIsValid, value); }
        }

        private bool addressIsChecking = false;
        public bool AddressIsChecking
        {
            get { return this.addressIsChecking; }
            set { SetProperty<bool>(ref this.addressIsChecking, value); }
        }

        private bool nextCommandEnabled = false;
        public bool NextCommandEnabled
        {
            get { return this.nextCommandEnabled; }
            set { SetProperty<bool>(ref this.nextCommandEnabled, value); }
        }

        private float imageAspectRatio = 0;
        public float ImageAspectRatio
        {
            get { return this.imageAspectRatio; }
            set { SetProperty<float>(ref this.imageAspectRatio, value); }
        }
        
        private bool photoIsUploaded = false;
        public bool PhotoIsUploaded
        {
            get { return this.photoIsUploaded; }
            set { SetProperty<bool>(ref this.photoIsUploaded, value); }
        }

        public AddressRegisterSellerPageViewModel(Models.Owner owner, float imageAspectRatio, bool photoIsUploaded = false)
        {
            this.NextCommand = new Command(this.GoNext);

            this.CurOwner = owner;
            this.ImageAspectRatio = imageAspectRatio;
            this.PhotoIsUploaded = photoIsUploaded;
        }

        public void CheckValidity()
        {
            if (this.AddressIsValid &&
                this.PointIsValid)
            {
                this.NextCommandEnabled = true;
            }
            else
            {
                this.NextCommandEnabled = false;
            }
        }

        public async Task<bool> CheckAddressValidity()
        {
            this.AddressIsValid = false;
            this.Point = new Position(0, 0);

            if (String.IsNullOrEmpty(this.AddressText) ||
                String.IsNullOrWhiteSpace(this.AddressText))
            {
                return false;
            }
            
            const int MINIM_ADDRESS_TEXT = 5;
            if (this.AddressText?.Length < MINIM_ADDRESS_TEXT)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Alamat harus terdiri dari {MINIM_ADDRESS_TEXT} huruf atau lebih.");
                return false;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                return false;
            }

            Geocoder geocoder = new Geocoder();
            IEnumerable<Position> results = null;

            this.AddressIsChecking = true;

            try
            {
                results = await geocoder.GetPositionsForAddressAsync(this.AddressText);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error", ex.Message, "OK");
            }

            this.AddressIsChecking = false;

            IList<Position> positions = results?.ToList();

            if (positions == null ||
                positions.Count == 0)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Alamat tidak dapat ditemukan.");
            }
            else
            {
                this.Point = positions[0];
            }

            this.AddressIsValid = true;
            return true;
        }

        public void SetPoint(Position pos)
        {
            this.Point = pos;
            this.PointIsValid = true;
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

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memproses registrasi. Periksa kembali koneksi internet anda.");
                this.IsBusy = false;
                return;
            }

            this.CurOwner.Location.Address = this.AddressText;
            this.CurOwner.Location.Latitude = this.Point.Latitude;
            this.CurOwner.Location.Longitude = this.Point.Longitude;

            var loading = DependencyService.Get<ILoadingHelper>();

            loading.Show();

            byte[] imageBytes = null;

            if (this.PhotoIsUploaded)
            {
                imageBytes = DependencyService.Get<IFileHelper>().ReadAllBytes(this.CurOwner.ProfilePicture);

                float width = Constant.MEDIA_PHOTO_PROFPIC_SIZE;
                float height = width * this.ImageAspectRatio;
                imageBytes = DependencyService.Get<IMediaHelper>().ResizeImage(imageBytes, width, height);

                this.CurOwner.ProfilePicture = this.User.ProfilePicture;
                this.CurOwner.ProfilePictureThumb = this.User.ProfilePictureThumb;
            }

            var result = await this.WebApiService.Account.UpdateAccount(this.CurOwner, imageBytes);

            this.User.SetUser(this.CurOwner);

            loading.Hide();

            switch (result)
            {
                case ServerResponseStatus.INVALID:
                    await this.NavigationService.CurrentPage.DisplayAlert("Aktivasi Maemseller Gagal", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;

                case ServerResponseStatus.ERROR:
                    await this.NavigationService.CurrentPage.DisplayAlert("Aktivasi Maemseller Gagal", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;
            }

            DependencyService.Get<IMessageHelper>().LongAlert("Aktivasi Maemseller Sukses.");
            
            await this.NavigationService.GoBack(3);
            await this.NavigationService.NavigateTo(typeof(MenuListingPage), null);

            this.IsBusy = false;
        }
    }
}
