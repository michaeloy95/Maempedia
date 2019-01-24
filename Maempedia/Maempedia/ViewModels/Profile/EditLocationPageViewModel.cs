using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Maempedia.ViewModels.Profile
{
    public class EditLocationPageViewModel : BaseViewModel
    {
        public ICommand SaveCommand { get; private set; }

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

        private bool addressIsValid = true;
        public bool AddressIsValid
        {
            get { return this.addressIsValid; }
            set { SetProperty<bool>(ref this.addressIsValid, value); }
        }

        private bool pointIsValid = true;
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

        private bool saveCommandEnabled = false;
        public bool SaveCommandEnabled
        {
            get { return this.saveCommandEnabled; }
            set { SetProperty<bool>(ref this.saveCommandEnabled, value); }
        }

        public EditLocationPageViewModel()
        {
            this.SaveCommand = new Command(this.Save);
        }

        public void InitialiseData()
        {
            this.AddressText = this.User.Address;
            this.Point = new Position(this.User.Latitude, this.User.Longitude);
        }

        public void CheckValidity()
        {
            if (!this.AddressIsValid ||
                !this.PointIsValid)
            { 
                this.SaveCommandEnabled = false;
                return;
            }

            if (this.AddressText == this.User.Address &&
                this.Point.Latitude == this.User.Latitude &&
                this.Point.Longitude == this.User.Longitude)
            {
                this.SaveCommandEnabled = false;
                return;
            }

            this.SaveCommandEnabled = true;
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
            owner.Location.Address = this.AddressText;
            owner.Location.Latitude = this.Point.Latitude;
            owner.Location.Longitude = this.Point.Longitude;

            var result = await this.WebApiService.Account.UpdateAccount(owner, null);

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

            DependencyService.Get<IMessageHelper>().LongAlert("Lokasi telah diperbaharui.");

            await this.NavigationService.GoBack();

            this.IsBusy = false;
        }
    }
}
