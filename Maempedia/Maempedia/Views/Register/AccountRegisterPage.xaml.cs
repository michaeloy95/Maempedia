using Maempedia.ViewModels.Register;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountRegisterPage : ContentPage
    {
        public AccountRegisterPageViewModel ViewModel;

        public AccountRegisterPage()
        {
            InitializeComponent();

            this.ViewModel = new AccountRegisterPageViewModel()
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

        private async void OnUsernameEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = await this.ViewModel.CheckUsername();
            entry.TextColor = valid ? Color.FromHex("#646464")
                            : Color.FromHex("#CF000F");

            this.ViewModel.CheckValidity();
        }

        private async void OnEmailEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = await this.ViewModel.CheckEmail();
            entry.TextColor = valid ? Color.FromHex("#646464")
                            : Color.FromHex("#CF000F");

            this.ViewModel.CheckValidity();
        }

        private void OnPasswordEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = this.ViewModel.CheckPassword();
            entry.TextColor = valid ? Color.FromHex("#646464")
                            : Color.FromHex("#CF000F");

            this.ViewModel.CheckValidity();
        }

        private void OnRePasswordEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = this.ViewModel.CheckRePassword();
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
            
            if (entry == this.UsernameEntry)
            {
                this.EmailEntry.Focus();
            }
            else if (entry == this.EmailEntry)
            {
                this.PasswordEntry.Focus();
            }
            else if (entry == this.PasswordEntry)
            {
                this.RePasswordEntry.Focus();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.ViewModel.CheckValidity();
        }
    }
}