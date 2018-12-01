using FFImageLoading.Forms;
using Maempedia.ViewModels.Menu;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMenuImagePage : ContentPage
    {
        public AddMenuImagePageViewModel ViewModel;

        private bool firstOpen = true;

        public AddMenuImagePage()
        {
            InitializeComponent();

            this.ViewModel = new AddMenuImagePageViewModel()
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (firstOpen)
            {
                this.NextButton.IsEnabled = false;
                firstOpen = false;
            }
        }
    }
}