using Maempedia.ViewModels.Feedbacks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Feedbacks
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuggestionPage : ContentPage
    {
        public SuggestionPageViewModel ViewModel;

        public SuggestionPage()
        {
            InitializeComponent();

            this.ViewModel = new SuggestionPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }
    }
}