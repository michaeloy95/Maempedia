using Maempedia.ViewModels.Feedbacks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Feedbacks
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportBugPage : ContentPage
    {
        public ReportBugPageViewModel ViewModel;

        public ReportBugPage()
        {
            InitializeComponent();

            this.ViewModel = new ReportBugPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }
    }
}