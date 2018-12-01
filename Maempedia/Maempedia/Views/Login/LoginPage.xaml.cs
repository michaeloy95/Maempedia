using Maempedia.ViewModels.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPageViewModel ViewModel;

        public LoginPage()
        {
            InitializeComponent();

            this.ViewModel = new LoginPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        private void OnEntryCompleted(object sender, System.EventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            if (entry == this.UsernameEntry)
            {
                this.PasswordEntry.Focus();
            }
            else if (entry == this.PasswordEntry)
            {
                if (this.ViewModel.LoginCommand.CanExecute(null))
                {
                    this.ViewModel.LoginCommand.Execute(null);
                }
            }
        }
    }
}