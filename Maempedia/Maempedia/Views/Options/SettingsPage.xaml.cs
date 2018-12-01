using Maempedia.ViewModels.Options;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Options
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
        public SettingsPageViewModel ViewModel;

		public SettingsPage ()
		{
			InitializeComponent ();

            this.ViewModel = new SettingsPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }
	}
}