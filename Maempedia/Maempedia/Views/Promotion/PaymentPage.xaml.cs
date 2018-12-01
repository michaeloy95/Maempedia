using Maempedia.ViewModels.Promotion;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Promotion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        public PaymentPageViewModel ViewModel;

        public PaymentPage(int PromotionPrice, string ReferenceCode)
        {
            InitializeComponent();

            this.ViewModel = new PaymentPageViewModel(PromotionPrice, ReferenceCode)
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }
    }
}