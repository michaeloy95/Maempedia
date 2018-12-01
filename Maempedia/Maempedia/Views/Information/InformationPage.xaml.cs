using Maempedia.ViewModels.Information;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Information
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InformationPage : ContentPage
    {
        public InformationPageViewModel ViewModel;

        public InformationPage()
        {
            InitializeComponent();

            this.ViewModel = new InformationPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }
    }
}