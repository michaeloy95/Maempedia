using Maempedia.Enum;
using Maempedia.Interfaces;
using Plugin.Connectivity;
using System;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Menu.Discount
{
    public class AddDiscountPageViewModel : BaseViewModel
    {
        private Models.Menu selectedMenu;
        public Models.Menu SelectedMenu
        {
            get { return this.selectedMenu; }
            set { SetProperty<Models.Menu>(ref this.selectedMenu, value); }
        }

        private string discountPrice;
        public string DiscountPrice
        {
            get { return this.discountPrice; }
            set { SetProperty<string>(ref this.discountPrice, value); }
        }

        private string discount;
        public string Discount
        {
            get { return this.discount; }
            set { SetProperty<string>(ref this.discount, value); }
        }

        private string maxClaim;
        public string MaxClaim
        {
            get { return this.maxClaim; }
            set { SetProperty<string>(ref this.maxClaim, value); }
        }

        private string discountCaption;
        public string DiscountCaption
        {
            get { return this.discountCaption; }
            set { SetProperty<string>(ref this.discountCaption, value); }
        }

        private bool endPriceIsValid = false;
        public bool EndPriceIsValid
        {
            get { return this.endPriceIsValid; }
            set { SetProperty<bool>(ref this.endPriceIsValid, value); }
        }

        private bool discountIsValid = false;
        public bool DiscountIsValid
        {
            get { return this.discountIsValid; }
            set { SetProperty<bool>(ref this.discountIsValid, value); }
        }

        private bool discountIsInvalid = false;
        public bool DiscountIsInvalid
        {
            get { return this.discountIsInvalid; }
            set { SetProperty<bool>(ref this.discountIsInvalid, value); }
        }

        private bool maxClaimIsValid = false;
        public bool MaxClaimIsValid
        {
            get { return this.maxClaimIsValid; }
            set { SetProperty<bool>(ref this.maxClaimIsValid, value); }
        }

        private bool maxClaimIsInvalid = false;
        public bool MaxClaimIsInvalid
        {
            get { return this.maxClaimIsInvalid; }
            set { SetProperty<bool>(ref this.maxClaimIsInvalid, value); }
        }

        private bool submitCommandEnabled = false;
        public bool SubmitCommandEnabled
        {
            get { return this.submitCommandEnabled; }
            set { SetProperty<bool>(ref this.submitCommandEnabled, value); }
        }

        public ICommand SubmitDiscountCommand { get; private set; }

        public AddDiscountPageViewModel(Models.Menu menu)
        {
            this.SelectedMenu = menu;
            this.DiscountPrice = menu.PriceString;
            this.DiscountCaption =
@"Catatan:
* Diskon berlaku 7 hari.
* Terdiskon hanya untuk 1 menu tiap Maemseller.
* Pastikan tawaran anda menarik.";
            this.EndPriceIsValid = this.DiscountIsValid = this.DiscountIsInvalid = this.MaxClaimIsValid = this.MaxClaimIsInvalid = false;

            this.SubmitDiscountCommand = new Command(SubmitDiscount);
        }

        public void CheckValidity()
        {
            if (this.DiscountIsValid &&
                this.MaxClaimIsValid)
            {
                this.SubmitCommandEnabled = true;
            }
            else
            {
                this.SubmitCommandEnabled = false;
            }
        }

        public bool CheckEndPrice()
        {
            return this.DiscountIsValid;
        }

        public bool CheckDiscount()
        {
            this.DiscountIsValid = false;
            this.DiscountIsInvalid = true;

            if (String.IsNullOrEmpty(this.Discount) ||
                String.IsNullOrWhiteSpace(this.Discount))
            {
                return false;
            }

            int disc = 0;
            if (!int.TryParse(this.Discount, out disc))
            {
                return false;
            }
            
            if (disc <= 0 || disc >= 100)
            {
                return false;
            }

            var newPrice = this.SelectedMenu.Price - (this.SelectedMenu.Price * (double)disc / 100.0);
            this.DiscountPrice = String.Format(new CultureInfo("id-ID"), "Rp. {0:N}", newPrice);

            return true;
        }

        public bool CheckMaxClaim()
        {
            this.MaxClaimIsValid = false;
            this.MaxClaimIsInvalid = true;

            if (String.IsNullOrEmpty(this.MaxClaim) ||
                String.IsNullOrWhiteSpace(this.MaxClaim))
            {
                return false;
            }

            int maxClaim = 0;
            if (!int.TryParse(this.MaxClaim, out maxClaim))
            {
                return false;
            }

            if (maxClaim <= 0 || maxClaim >= 100)
            {
                return false;
            }

            return true;
        }

        public async void SubmitDiscount()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memproses. Periksa kembali koneksi internet anda.");
                this.IsBusy = false;
                return;
            }

            if (!this.SubmitCommandEnabled)
            {
                this.IsBusy = false;
                return;
            }

            var loading = DependencyService.Get<ILoadingHelper>();

            loading.Show();

            var result = await this.WebApiService.Menu.AddDiscount(this.SelectedMenu.ID, int.Parse(this.Discount), int.Parse(this.MaxClaim), this.User.Username, this.User.Password);

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

            DependencyService.Get<IMessageHelper>().ShortAlert("Diskon telah ditambahkan kedalam menu.");
            this.User.MenuListFetched = false;
            
            await this.NavigationService.GoBack();

            this.IsBusy = false;
        }
    }
}
