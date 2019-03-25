using Maempedia.ViewModels.Menu.Discount;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Menu.Discount
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDiscountPage : ContentPage
    {
        public AddDiscountPageViewModel ViewModel;

        public AddDiscountPage(Models.Menu menu)
        {
            InitializeComponent();

            this.ViewModel = new AddDiscountPageViewModel(menu)
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        private void MenuImage_Success(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e)
        {
            var info = e.ImageInformation;
            if (info == null)
            {
                return;
            }

            this.MenuImage.HeightRequest = this.MenuImage.WidthRequest * ((double)info.OriginalHeight / (double)info.OriginalWidth);
        }

        private void OnEntryFocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            entry.TextColor = Color.FromHex("#646464");
        }

        private void OnDiscountPriceEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = this.ViewModel.CheckDiscount();
            ViewModel.DiscountIsValid = valid;
            ViewModel.DiscountIsInvalid = !valid;

            this.ViewModel.CheckValidity();
        }

        private void OnMaxClaimEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = this.ViewModel.CheckMaxClaim();
            ViewModel.MaxClaimIsValid = valid;
            ViewModel.MaxClaimIsInvalid = !valid;

            this.ViewModel.CheckValidity();
        }

        private void OnEntryCompleted(object sender, System.EventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            entry.Unfocus();

            if (entry == this.DiscountPriceEntry)
            {
                this.MaxClaimEntry.Focus();
            }
        }
    }
}