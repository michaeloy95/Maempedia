using BottomBar.XamarinForms;
using Maempedia.ViewModels.Profile;
using Maempedia.Views.Login;
using Maempedia.Views.Register;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnloggedProfilePage : ContentPage
    {
        public UnloggedProfilePageViewModel ViewModel;

        public UnloggedProfilePage()
        {
            InitializeComponent();

            this.ViewModel = new UnloggedProfilePageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            NavigationPage navPage = null;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    navPage = (App.Current.MainPage as BottomBarPage).SelectedItem as NavigationPage;
                    break;
                case Device.Android:
                    navPage = App.Current.MainPage as NavigationPage;
                    break;
            }

            navPage.BarBackgroundColor = Color.FromHex("#FFFFFF");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            NavigationPage navPage = null;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    navPage = (App.Current.MainPage as BottomBarPage).SelectedItem as NavigationPage;
                    break;
                case Device.Android:
                    navPage = App.Current.MainPage as NavigationPage;
                    break;
            }

            navPage.BarBackgroundColor = Color.FromHex("#F8F8F8");
        }
    }
}