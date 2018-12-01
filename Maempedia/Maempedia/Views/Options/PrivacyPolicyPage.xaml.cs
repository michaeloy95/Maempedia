using Maempedia.ViewModels.Options;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Options
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PrivacyPolicyPage : ContentPage
	{
        public PrivacyPolicyPageViewModel ViewModel;

		public PrivacyPolicyPage ()
		{
			InitializeComponent ();

            this.ViewModel = new PrivacyPolicyPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            this.ViewModel.OnNavigated();
        }

        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            this.ViewModel.OnNavigating();
        }
    }
}