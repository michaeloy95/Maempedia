using Maempedia.Models;
using Maempedia.ViewModels.Profile;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoggedProfilePage : ContentPage
    {
        public LoggedProfilePageViewModel ViewModel;

        public LoggedProfilePage()
        {
            InitializeComponent();

            this.ViewModel = new LoggedProfilePageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        private void ProfileMenuListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as ProfileMenu;
            if (item == null)
                return;

            if (this.ViewModel.SelectProfileMenuCommand.CanExecute(item))
            {
                this.ViewModel.SelectProfileMenuCommand.Execute(item);
            }

            this.ProfileMenuListView.SelectedItem = null;
        }

        private void OnViewTapped(object sender, System.EventArgs e)
        {
            if (this.ViewModel.ViewProfileCommand.CanExecute(null))
            {
                this.ViewModel.ViewProfileCommand.Execute(null);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            NavigationPage.SetHasNavigationBar(this.Parent, false);

            await Task.Delay(100); // Bug with flying bottom bar

            if (!this.ProfileMenuListView.IsRefreshing)
            {
                this.ProfileMenuListView.IsRefreshing = true;
                this.ProfileMenuListView.IsRefreshing = false;
            }

            this.ViewModel?.RefreshUserDetails();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            NavigationPage.SetHasNavigationBar(this.Parent, true);
        }
    }
}