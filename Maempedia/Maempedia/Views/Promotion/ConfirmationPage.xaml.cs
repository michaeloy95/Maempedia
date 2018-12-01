using Maempedia.ViewModels.Promotion;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Promotion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmationPage : ContentPage
    {
        public ConfirmationPageViewModel ViewModel;

        public ConfirmationPage(string ReferenceCode)
        {
            InitializeComponent();

            this.ViewModel = new ConfirmationPageViewModel(ReferenceCode)
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}