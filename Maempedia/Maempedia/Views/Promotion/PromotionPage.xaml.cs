using Maempedia.ViewModels.Promotion;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Promotion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PromotionPage : ContentPage
    {
        public PromotionPageViewModel ViewModel;

        public PromotionPage(string menuID)
        {
            InitializeComponent();
            this.ViewModel = new PromotionPageViewModel(menuID)
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }
    }
}