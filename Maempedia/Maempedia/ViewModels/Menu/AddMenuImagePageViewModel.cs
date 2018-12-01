using Maempedia.Data;
using Maempedia.Views.Menu;
using Plugin.Media;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Menu
{
    public class AddMenuImagePageViewModel : BaseViewModel
    {
        public ICommand UploadCommand { get; private set; }

        public ICommand NextCommand { get; private set; }

        private string imageSource = "menuplaceholder.png";
        public string ImageSource
        {
            get { return this.imageSource; }
            set { SetProperty<string>(ref this.imageSource, value); }
        }

        public double ImageWidth
        {
            get { return (App.ScreenWidth - 150) * Settings.ImageQuality; }
        }

        private float imageAspectRatio;
        public float ImageAspectRatio
        {
            get { return this.imageAspectRatio; }
            set { SetProperty<float>(ref this.imageAspectRatio, value); }
        }

        private bool nextCommandEnabled = false;
        public bool NextCommandEnabled
        {
            get { return this.nextCommandEnabled; }
            set { SetProperty<bool>(ref this.nextCommandEnabled, value); }
        }

        public AddMenuImagePageViewModel()
        {
            this.UploadCommand = new Command(this.UploadImage);
            this.NextCommand = new Command(this.GoNext);
        }

        private async void UploadImage()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            var media = CrossMedia.Current;
            var file = await media.PickPhotoAsync();

            if (file == null)
            {
                this.IsBusy = false;
                return;
            }
            
            this.ImageSource = file.Path;
            this.NextCommandEnabled = true;

            this.IsBusy = false;
        }

        private async void GoNext()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            await this.NavigationService.NavigateTo(typeof(AddMenuDetailPage));

            this.IsBusy = false;
        }
    }
}
