using Maempedia.ViewModels.Menu;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuViewPage : ContentPage
    {
        public MenuViewPageViewModel ViewModel;

        public MenuViewPage(Models.Menu menu)
        {
            InitializeComponent();

            this.ViewModel = new MenuViewPageViewModel(menu)
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;

            if (menu.Promoted)
            {
                this.BtnPromote.Icon = "ic_promoted_active";
            }
        }

        private void CachedImage_Success(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e)
        {
            var info = e.ImageInformation;
            this.ViewModel.ImageAspectRatio = (float)info.OriginalHeight / (float)info.OriginalWidth;
        }
    }
}