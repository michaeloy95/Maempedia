using Maempedia.ViewModels.Map;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Map
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BindingPinPage : StackLayout
    {
        public BindingPinPageViewModel ViewModel;

        public BindingPinPage(string imageSource)
        {
            InitializeComponent();

            this.ViewModel = new BindingPinPageViewModel(imageSource);
            this.BindingContext = this.ViewModel;
        }

        private void CachedImage_Success(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e)
        {
            var info = e.ImageInformation;
            if (info == null)
            {
                return;
            }

            this.ProfilePicture.WidthRequest = 50;
            this.ProfilePicture.HeightRequest = this.ProfilePicture.Width * ((double)info.OriginalHeight / (double)info.OriginalWidth);
        }
    }
}