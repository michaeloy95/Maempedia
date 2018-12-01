using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.Menu;
using Maempedia.Views.Promotion;
using Plugin.Connectivity;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Promotion
{
    public class PromotionPageViewModel : BaseViewModel
    {
        public ICommand Option1Callback { get; private set; }

        public ICommand Option2Callback { get; private set; }

        public ICommand Option3Callback { get; private set; }

        public ICommand NextCommand { get; private set; }

        private bool check10Visible = false;
        public bool Check10Visible
        {
            get { return this.check10Visible; }
            set { this.SetProperty<bool>(ref this.check10Visible, value); }
        }

        private bool check30Visible = false;
        public bool Check30Visible
        {
            get { return this.check30Visible; }
            set { this.SetProperty<bool>(ref this.check30Visible, value); }
        }

        private bool check60Visible = false;
        public bool Check60Visible
        {
            get { return this.check60Visible; }
            set { this.SetProperty<bool>(ref this.check60Visible, value); }
        }

        private int promotionPrice = 0;
        public int PromotionPrice
        {
            get { return this.promotionPrice; }
            set { this.SetProperty<int>(ref this.promotionPrice, value); }
        }

        private string priceString = "Rp. 0";
        public string PriceString
        {
            get { return this.priceString; }
            set { this.SetProperty<string>(ref this.priceString, value); }
        }

        private int dayCount = 0;
        public int DayCount
        {
            get { return this.dayCount; }
            set { this.SetProperty<int>(ref this.dayCount, value); }
        }

        private bool nextIsValid = false;
        public bool NextIsValid
        {
            get { return this.nextIsValid; }
            set { this.SetProperty<bool>(ref this.nextIsValid, value); }
        }

        private string menuID = string.Empty;
        public string MenuID
        {
            get { return this.menuID; }
            set { this.SetProperty<string>(ref this.menuID, value); }
        }

        public PromotionPageViewModel(string menuID)
        {
            this.Option1Callback = new Command(() => this.SelectPromotionOption(1));
            this.Option2Callback = new Command(() => this.SelectPromotionOption(2));
            this.Option3Callback = new Command(() => this.SelectPromotionOption(3));
            this.NextCommand = new Command(this.GotoPaymentPage);
            this.SelectPromotionOption(1);

            this.MenuID = menuID;
        }

        private void SelectPromotionOption(int option)
        {
            this.Check10Visible = false;
            this.Check30Visible = false;
            this.Check60Visible = false;

            switch (option)
            {
                case 1:
                    this.PromotionPrice = 30000;
                    this.DayCount = 10;
                    this.Check10Visible = true;
                    break;
                case 2:
                    this.PromotionPrice = 60000;
                    this.DayCount = 30;
                    this.Check30Visible = true;
                    break;
                case 3:
                    this.PromotionPrice = 100000;
                    this.DayCount = 60;
                    this.Check60Visible = true;
                    break;
            }

            this.PriceString = $"Rp. {String.Format("{0:n0}", this.PromotionPrice)}";
            this.NextIsValid = true;
        }

        private async void GotoPaymentPage()
        {
            if (!this.NextIsValid)
            {
                DependencyService.Get<IMessageHelper>().ShortAlert("Pilih jenis promosi");
                return;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memproses. Periksa kembali koneksi internet anda.");
                return;
            }

            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            var loading = DependencyService.Get<ILoadingHelper>();

            loading.Show();
            
            var result = await MenuService.RequestPromotion(this.MenuID, this.PromotionPrice, this.DayCount, string.Empty, this.User.Username, this.User.Password);

            loading.Hide();

            switch (result.Item1)
            {
                case ServerResponseStatus.INVALID:
                    DependencyService.Get<IMessageHelper>().ShortAlert("Terjadi kesalahan.");
                    this.IsBusy = false;
                    return;

                case ServerResponseStatus.ERROR:
                    DependencyService.Get<IMessageHelper>().ShortAlert("Terjadi kesalahan.");
                    this.IsBusy = false;
                    return;
            }

            await this.NavigationService.NavigateTo(typeof(PaymentPage), new object[] { this.PromotionPrice, result.Item2 });

            this.IsBusy = false;
        }
    }
}
