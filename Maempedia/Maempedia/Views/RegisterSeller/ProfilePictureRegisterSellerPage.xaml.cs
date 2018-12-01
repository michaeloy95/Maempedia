using FFImageLoading.Forms;
using Maempedia.ViewModels.RegisterSeller;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.RegisterSeller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePictureRegisterSellerPage : ContentPage
    {
        public ProfilePictureRegisterSellerPageViewModel ViewModel;

        public ProfilePictureRegisterSellerPage(Models.Owner owner)
        {
            InitializeComponent();

            this.ViewModel = new ProfilePictureRegisterSellerPageViewModel(owner)
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        private void CachedImage_Success(object sender, CachedImageEvents.SuccessEventArgs e)
        {
            var info = e.ImageInformation;
            this.ViewModel.ImageAspectRatio = (float)info.OriginalHeight / (float)info.OriginalWidth;
        }
    }
}