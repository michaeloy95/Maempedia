using Maempedia.Views.Menu;
using Maempedia.Views.Promotion;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Promotion
{
    public class ConfirmationPageViewModel : BaseViewModel
    {
        public ICommand ConfirmCommand { get; private set; }

        public ICommand ReturnCommand { get; private set; }

        private string referenceCode = string.Empty;
        public string ReferenceCode
        {
            get { return this.referenceCode; }
            set { this.SetProperty<string>(ref this.referenceCode, value); }
        }

        public ConfirmationPageViewModel(string ReferenceCode)
        {
            this.ConfirmCommand = new Command(this.ConfirmViaWA);
            this.ReturnCommand = new Command(this.ReturnToViewMenu);

            this.ReferenceCode = ReferenceCode;
        }

        private void ConfirmViaWA()
        {
            var stack = this.NavigationService.CurrentPage.Navigation.NavigationStack;

            string text = "";
            if (stack[stack.Count - 2] is PaymentPage page)
            {
                text = Uri.EscapeUriString($"Konfirmasi Pembayaran:\nTotal: *{page.ViewModel.PriceString}*\nKode Referensi: *{this.ReferenceCode}*");
            }

            string url = $"https://api.whatsapp.com/send?phone=6281259418874&text={text}";
            Device.OpenUri(new Uri(url));
        }

        private async void ReturnToViewMenu()
        {
            // removing all the pages in between ConfirmationPage
            var stack = this.NavigationService.CurrentPage.Navigation.NavigationStack;
            while (stack[stack.Count-2].GetType() != typeof(MenuViewPage) || stack[stack.Count - 2].GetType() != typeof(MenuListingPage))
            {
                this.NavigationService.CurrentPage.Navigation.RemovePage(stack[stack.Count - 2]);
            }

            await this.NavigationService.GoBack();
        }
    }
}
