using Maempedia.ViewModels.MVoucher;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.MVoucher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MVoucherPage : ContentPage
    {
        public MVoucherPageViewModel ViewModel;

        public MVoucherPage()
        {
            InitializeComponent();

            this.ViewModel = new MVoucherPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }
    }
}