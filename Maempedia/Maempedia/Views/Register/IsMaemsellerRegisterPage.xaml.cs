using Maempedia.ViewModels.Register;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IsMaemsellerRegisterPage : ContentPage
    {
        public IsMaemsellerRegisterPageViewModel ViewModel;

        public IsMaemsellerRegisterPage()
        {
            InitializeComponent();

            this.ViewModel = new IsMaemsellerRegisterPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }
    }
}