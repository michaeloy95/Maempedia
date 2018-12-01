using Maempedia.Common;
using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.Menu;
using Plugin.Connectivity;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Menu
{
    public class AddMenuDetailPageViewModel : BaseViewModel
    {
        public ICommand NextCommand { get; private set; }

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

        private bool nameIsValid = false;
        public bool NameIsValid
        {
            get { return this.nameIsValid; }
            set { SetProperty<bool>(ref this.nameIsValid, value); }
        }

        private bool descriptionIsValid = false;
        public bool DescriptionIsValid
        {
            get { return this.descriptionIsValid; }
            set { SetProperty<bool>(ref this.descriptionIsValid, value); }
        }

        private bool priceIsValid = false;
        public bool PriceIsValid
        {
            get { return this.priceIsValid; }
            set { SetProperty<bool>(ref this.priceIsValid, value); }
        }

        private bool portionIsValid = false;
        public bool PortionIsValid
        {
            get { return this.portionIsValid; }
            set { SetProperty<bool>(ref this.portionIsValid, value); }
        }

        private bool nextCommandEnabled = false;
        public bool NextCommandEnabled
        {
            get { return this.nextCommandEnabled; }
            set { SetProperty<bool>(ref this.nextCommandEnabled, value); }
        }

        public AddMenuDetailPageViewModel()
        {
            this.NextCommand = new Command(this.ProcessAddMenu);
        }

        public void CheckValidity()
        {
            if (this.NameIsValid &&
                this.DescriptionIsValid &&
                this.PortionIsValid &&
                this.PriceIsValid)
            {
                this.NextCommandEnabled = true;
            }
            else
            {
                this.NextCommandEnabled = false;
            }
        }

        public bool CheckName()
        {
            this.NameIsValid = false;
            var message = DependencyService.Get<IMessageHelper>();

            if (String.IsNullOrEmpty(this.NameText) ||
                String.IsNullOrWhiteSpace(this.NameText))
            {
                message.ShortAlert("Nama tidak boleh kosong");
                return false;
            }

            const int MINIM_TEXT_COUNT = 3;
            if (this.NameText.Length < MINIM_TEXT_COUNT)
            {
                message.ShortAlert($"Nama harus lebih dari {MINIM_TEXT_COUNT} karakter");
                return false;
            }

            this.NameIsValid = true;
            return true;
        }

        public bool CheckDescription()
        {
            this.DescriptionIsValid = false;
            var message = DependencyService.Get<IMessageHelper>();

            if (String.IsNullOrEmpty(this.DescriptionText) ||
                String.IsNullOrWhiteSpace(this.DescriptionText))
            {
                message.ShortAlert("Deskripsi tidak boleh kosong");
                return false;
            }

            const int MINIM_TEXT_COUNT = 10;
            if (this.DescriptionText.Length < MINIM_TEXT_COUNT)
            {
                message.ShortAlert($"Deskripsi harus lebih dari {MINIM_TEXT_COUNT} karakter");
                return false;
            }

            this.DescriptionIsValid = true;
            return true;
        }

        public bool CheckPortion()
        {
            this.PortionIsValid = false;
            var message = DependencyService.Get<IMessageHelper>();

            if (String.IsNullOrEmpty(this.PortionText) ||
                String.IsNullOrWhiteSpace(this.PortionText))
            {
                message.ShortAlert("Porsi tidak boleh kosong");
                return false;
            }

            this.PortionIsValid = true;
            return true;
        }

        public bool CheckPrice()
        {
            this.PriceIsValid = false;
            var message = DependencyService.Get<IMessageHelper>();

            if (String.IsNullOrEmpty(this.PriceText) ||
                String.IsNullOrWhiteSpace(this.PriceText))
            {
                message.ShortAlert("Harga tidak boleh kosong");
                return false;
            }

            bool result = double.TryParse(this.PriceText, out double value);
            if (!result)
            {
                message.ShortAlert("Format harga salah");
                return false;
            }

            if (value == 0)
            {
                return false;
            }

            this.PriceIsValid = true;
            return true;
        }

        private async void ProcessAddMenu()
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

            if (!this.CheckName() ||
                !this.CheckDescription() ||
                !this.CheckPrice() ||
                !this.CheckPortion())
            {
                this.IsBusy = false;
                return;
            }

            var loading = DependencyService.Get<ILoadingHelper>();

            loading.Show();

            var stack = this.NavigationService.CurrentPage.Navigation.NavigationStack;
            var parentPage = stack[stack.Count - 2] as AddMenuImagePage;
            if (parentPage == null)
            {
                loading.Hide();
                this.IsBusy = false;
                return;
            }

            Models.Menu menu = new Models.Menu()
            {
                Name = this.NameText,
                Headline = this.DescriptionText,
                Portion = this.PortionText,
                Price = float.Parse(this.PriceText)
            };

            var imageBytes = DependencyService.Get<IFileHelper>().ReadAllBytes(parentPage.ViewModel.ImageSource);

            float width = Constant.MEDIA_PHOTO_PROFPIC_SIZE;
            float height = width * parentPage.ViewModel.ImageAspectRatio;
            imageBytes = DependencyService.Get<IMediaHelper>().ResizeImage(imageBytes, width, height);

            var result = await MenuService.AddMenu(menu, imageBytes, this.User.ID, this.User.Username, this.User.Password);

            loading.Hide();

            switch (result)
            {
                case ServerResponseStatus.INVALID:
                    await this.NavigationService.CurrentPage.DisplayAlert("Registrasi Gagal", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;

                case ServerResponseStatus.ERROR:
                    await this.NavigationService.CurrentPage.DisplayAlert("Registrasi Gagal", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    return;
            }

            DependencyService.Get<IMessageHelper>().ShortAlert("Penambahan menu sukses.");
            this.User.MenuListFetched = false;

            // Organise and direct user to Menu View Page
            await this.NavigationService.GoBack(2);

            this.IsBusy = false;
        }
    }
}
