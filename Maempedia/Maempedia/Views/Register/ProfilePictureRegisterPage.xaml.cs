using FFImageLoading.Forms;
using Maempedia.ViewModels.Register;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePictureRegisterPage : ContentPage
    {
        public ProfilePictureRegisterPageViewModel ViewModel;

        public ProfilePictureRegisterPage(Models.Owner owner)
        {
            InitializeComponent();

            this.ViewModel = new ProfilePictureRegisterPageViewModel(owner)
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