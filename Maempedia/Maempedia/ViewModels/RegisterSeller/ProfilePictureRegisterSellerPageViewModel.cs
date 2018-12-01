using Maempedia.Data;
using Maempedia.Views.RegisterSeller;
using Plugin.Media;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.RegisterSeller
{
    public class ProfilePictureRegisterSellerPageViewModel : BaseViewModel
    {
        public ICommand UploadCommand { get; private set; }

        public ICommand NextCommand { get; private set; }

        private Models.Owner curOwner = null;
        public Models.Owner CurOwner
        {
            get { return this.curOwner; }
            set { SetProperty<Models.Owner>(ref this.curOwner, value); }
        }

        private string imageSource = "profilepictureplaceholder.png";
        public string ImageSource
        {
            get { return this.imageSource; }
            set { SetProperty<string>(ref this.imageSource, value); }
        }

        private float imageAspectRatio = 0;
        public float ImageAspectRatio
        {
            get { return this.imageAspectRatio; }
            set { SetProperty<float>(ref this.imageAspectRatio, value); }
        }

        public double ImageWidth
        {
            get { return (App.ScreenWidth - 150) * Settings.ImageQuality; }
        }

        private bool photoIsUploaded = false;
        public bool PhotoIsUploaded
        {
            get { return this.photoIsUploaded; }
            set { SetProperty<bool>(ref this.photoIsUploaded, value); }
        }

        private string nextText = "Lewati";
        public string NextText
        {
            get { return this.nextText; }
            set { SetProperty<string>(ref this.nextText, value); }
        }

        public ProfilePictureRegisterSellerPageViewModel(Models.Owner owner)
        {
            this.UploadCommand = new Command(this.UploadImage);
            this.NextCommand = new Command(this.GoNext);

            this.CurOwner = owner;
            this.ImageSource = this.CurOwner.ProfilePicture;
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
            this.PhotoIsUploaded = true;
            this.NextText = "Unggah Foto";
            this.CurOwner.ProfilePicture = file.Path;

            this.IsBusy = false;
        }

        private async void GoNext()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(AddressRegisterSellerPage), new object[] { this.CurOwner, this.ImageAspectRatio, this.PhotoIsUploaded });

            this.IsBusy = false;
        }
    }
}
