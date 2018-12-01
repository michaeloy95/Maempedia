using Maempedia.ViewModels.RegisterSeller;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.RegisterSeller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestaurantRegisterSellerPage : ContentPage
    {
        public RestaurantRegisterSellerPageViewModel ViewModel;

        public RestaurantRegisterSellerPage(Models.Owner owner)
        {
            InitializeComponent();

            this.ViewModel = new RestaurantRegisterSellerPageViewModel(owner)
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
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

        private async void OnNameEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = await this.ViewModel.CheckName();
            entry.TextColor = valid ? Color.FromHex("#646464")
                            : Color.FromHex("#CF000F");

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

            if (entry == this.NameEntry)
            {
                this.OpeningTimePicker.Focus();
            }
        }

        private void OnEditorCompleted(object sender, System.EventArgs e)
        {
            var editor = sender as Editor;
            if (editor == null)
            {
                return;
            }

            editor.Unfocus();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.ViewModel.CheckValidity();
        }
    }
}