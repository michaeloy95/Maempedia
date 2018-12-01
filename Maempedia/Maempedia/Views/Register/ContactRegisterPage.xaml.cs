using Maempedia.ViewModels.Register;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactRegisterPage : ContentPage
    {
        public ContactRegisterPageViewModel ViewModel;

        public ContactRegisterPage(Models.Owner owner, float imageAspectRatio)
        {
            InitializeComponent();

            this.ViewModel = new ContactRegisterPageViewModel(owner, imageAspectRatio)
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

        private async void OnContactEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = await this.ViewModel.CheckContact();
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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.ViewModel.CheckValidity();
        }
    }
}