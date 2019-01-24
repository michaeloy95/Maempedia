using Maempedia.Common;
using Maempedia.Data;
using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.Menu;
using Maempedia.Views.Promotion;
using Plugin.Connectivity;
using Plugin.Media;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Menu
{
    public class MenuViewPageViewModel : BaseViewModel
    {
        public ICommand PreviewCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand PromoteMenuCommand { get; private set; }

        public ICommand DeleteMenuCommand { get; private set; }

        public ICommand UploadCommand { get; private set; }

        private Models.Menu selectedMenu;
        public Models.Menu SelectedMenu
        {
            get { return this.selectedMenu; }
            set { SetProperty<Models.Menu>(ref this.selectedMenu, value); }
        }

        private string nameText = string.Empty;
        public string NameText
        {
            get { return this.nameText; }
            set { SetProperty<string>(ref this.nameText, value); }
        }

        private string descriptionText = string.Empty;
        public string DescriptionText
        {
            get { return this.descriptionText; }
            set { SetProperty<string>(ref this.descriptionText, value); }
        }

        private string portionText = string.Empty;
        public string PortionText
        {
            get { return this.portionText; }
            set { SetProperty<string>(ref this.portionText, value); }
        }

        private string priceText = string.Empty;
        public string PriceText
        {
            get { return this.priceText; }
            set { SetProperty<string>(ref this.priceText, value); }
        }

        private string imageSource = "profilepictureplaceholder.png";
        public string ImageSource
        {
            get { return this.imageSource; }
            set { SetProperty<string>(ref this.imageSource, value); }
        }
        
        public double MenuImageWidth
        {
            get { return (App.ScreenWidth - 60) * Settings.ImageQuality; }
        }
        
        private float imageAspectRatio;
        public float ImageAspectRatio
        {
            get { return this.imageAspectRatio; }
            set { SetProperty<float>(ref this.imageAspectRatio, value); }
        }

        private bool imageIsUpdated = false;
        public bool ImageIsUpdated
        {
            get { return this.imageIsUpdated; }
            set { SetProperty<bool>(ref this.imageIsUpdated, value); }
        }

        public MenuViewPageViewModel(Models.Menu menu)
        {
            this.SelectedMenu = menu;
            this.PromoteMenuCommand = new Command(this.PromoteMenu);
            this.PreviewCommand = new Command(this.GotoPreview);
            this.SaveCommand = new Command(this.SaveMenu);
            this.DeleteMenuCommand = new Command(this.DeleteMenu);

            this.ImageSource = menu.ImageSource;
            this.NameText = this.SelectedMenu.Name;
            this.DescriptionText = this.SelectedMenu.Headline;
            this.PortionText = this.SelectedMenu.Portion;
            this.PriceText = this.SelectedMenu.Price.ToString();
        }

        private void PromoteMenu()
        {
            if (this.SelectedMenu.Promoted)
            {
                DependencyService.Get<IMessageHelper>().ShortAlert("Promosi menu sedang berlangsung");
            }
            else
            {
                this.GotoPromotion();
            }
        }

        private async void GotoPromotion()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(PromotionPage), new object[] { this.SelectedMenu.ID });

            this.IsBusy = false;
        }

        private async void GotoPreview()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(MenuDetailPage), new object[] { this.SelectedMenu });

            this.IsBusy = false;
        }

        private async void SaveMenu()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memproses. Periksa kembali koneksi internet anda.");
                this.IsBusy = false;
                return;
            }

            var loading = DependencyService.Get<ILoadingHelper>();

            loading.Show();

            this.SelectedMenu.Name = this.NameText;
            this.SelectedMenu.Headline = this.DescriptionText;
            this.SelectedMenu.Portion = this.PortionText;
            this.SelectedMenu.Price = float.Parse(this.PriceText);

            byte[] imageBytes = null;

            if (this.ImageIsUpdated)
            {
                imageBytes = DependencyService.Get<IFileHelper>().ReadAllBytes(this.ImageSource);

                float width = Constant.MEDIA_PHOTO_MENUIMAGE_SIZE;
                float height = width * this.ImageAspectRatio;
                imageBytes = DependencyService.Get<IMediaHelper>().ResizeImage(imageBytes, width, height);
            }

            var result = await this.WebApiService.Menu.UpdateMenu(this.SelectedMenu, imageBytes, this.User.Username, this.User.Password);

            loading.Hide();

            switch (result)
            {
                case ServerResponseStatus.INVALID:
                    await this.NavigationService.CurrentPage.DisplayAlert("Pembaharuan Gagal", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;

                case ServerResponseStatus.ERROR:
                    await this.NavigationService.CurrentPage.DisplayAlert("Pembaharuan Gagal", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;
            }

            DependencyService.Get<IMessageHelper>().LongAlert("Menu telah diperbaharui.");
            this.User.MenuListFetched = false;

            this.IsBusy = false;
        }

        private async void DeleteMenu()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            var respond = await this.NavigationService.CurrentPage.DisplayAlert("Hapus Menu", "Apakah anda yakin ingin menghapus postingan menu?", "Hapus", "Batal");
            if (!respond)
            {
                this.IsBusy = false;
                return;
            }

            var loading = DependencyService.Get<ILoadingHelper>();

            loading.Show();

            var result = await this.WebApiService.Menu.DeleteMenu(this.SelectedMenu.ID, this.User.Username, this.User.Password);

            loading.Hide();

            switch (result)
            {
                case ServerResponseStatus.INVALID:
                    await this.NavigationService.CurrentPage.DisplayAlert("Hapus Menu Gagal", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;

                case ServerResponseStatus.ERROR:
                    await this.NavigationService.CurrentPage.DisplayAlert("Hapus Menu Gagal", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;
            }

            DependencyService.Get<IMessageHelper>().LongAlert("Menu telah dihapus.");
            this.User.MenuListFetched = false;

            this.IsBusy = false;

            await this.NavigationService.GoBack();
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
            this.ImageIsUpdated = true;

            this.IsBusy = false;
        }
    }
}
