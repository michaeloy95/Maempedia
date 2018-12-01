using Maempedia.Interfaces;
using Maempedia.Views.Promotion;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Promotion
{
    public class PaymentPageViewModel : BaseViewModel
    {
        public ICommand NextCommand { get; private set; }

        private static Random random = new Random();

        private int promotionPrice = 0;
        public int PromotionPrice
        {
            get { return this.promotionPrice; }
            set { this.SetProperty<int>(ref this.promotionPrice, value); }
        }

        private string priceString = "Rp. 0";
        public string PriceString
        {
            get { return $"Rp. {String.Format("{0:n0}", this.PromotionPrice)}"; }
            set { this.SetProperty<string>(ref this.priceString, value); }
        }

        private string descriptionMessage = string.Empty;
        public string DescriptionMessage
        {
            get { return this.descriptionMessage; }
            set { this.SetProperty<string>(ref this.descriptionMessage, value); }
        }

        private string referenceCode = string.Empty;
        public string ReferenceCode
        {
            get { return this.referenceCode; }
            set { this.SetProperty<string>(ref this.referenceCode, value); }
        }

        public PaymentPageViewModel(int PromotionPrice, string ReferenceCode)
        {
            this.NextCommand = new Command(this.GotoConfirmationtPage);

            this.PromotionPrice = PromotionPrice;
            this.PriceString = $"Rp. {String.Format("{0:n0}", this.PromotionPrice)}";

            this.ReferenceCode = ReferenceCode;
            this.DescriptionMessage = $"Deskripsi: {this.ReferenceCode}";
        }

        private async void GotoConfirmationtPage()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            var loading = DependencyService.Get<ILoadingHelper>();

            loading.Show();

            await Task.Delay(3000);

            loading.Hide();

            await this.NavigationService.NavigateTo(typeof(ConfirmationPage), new object[] { this.ReferenceCode });

            this.IsBusy = false;
        }
    }
}
