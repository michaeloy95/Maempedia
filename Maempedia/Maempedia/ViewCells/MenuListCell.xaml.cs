using Maempedia.Data;
using Maempedia.Interfaces;
using Maempedia.Models;
using Maempedia.Views.Owner;
using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.ViewCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuListCell : ViewCell
    {
        public MenuListCell()
        {
            InitializeComponent();
        }

        protected async override void OnBindingContextChanged()
        {
            var menu = BindingContext as Models.Menu;
            if (menu == null)
            {
                return;
            }

            try
            {
                this.OwnerProfilePicture.Source = menu.Owner.ProfilePictureThumb;
                this.MenuImage.Source = menu.ImageSource;
                this.MenuImage.WidthRequest = App.ScreenWidth * Settings.ImageQuality;
                this.MenuImage.DownsampleHeight = App.ScreenWidth * Settings.ImageQuality;

                var savedMenu = await App.SavedMenuDatabase.GetMenuAsync(menu.ID);
                if (savedMenu != null)
                {
                    this.LikeImage.Source = "heart_full.png";
                }
                else
                {
                    this.LikeImage.Source = "heart_blank.png";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            var hasDiscount = menu.Discount != 0 && menu.DiscountDaysLeft >= 0 && menu.RemainingClaim > 0;
            this.DiscountLayout.IsVisible = this.ClaimButtonLayout.IsVisible = this.ClaimLayout.IsVisible = this.PriceWithDiscountLayout.IsVisible = hasDiscount;
            this.PriceNoDiscountLayout.IsVisible = !hasDiscount;
            Grid.SetRow(this.AddressLayout, !hasDiscount ? 2 : 3);
            
            this.DiscountedPrice.Text = String.Format(new CultureInfo("id-ID"), "Rp. {0:N}", menu.Price - (menu.Price * menu.Discount));
            this.DiscountTextLabel.Text = $"{menu.Discount*100}% OFF";
            this.RemainingDaysLabel.Text = $"Berakhir {menu.DiscountDaysLeft} hari lagi.";
            this.RemainingClaimLabel.Text = $"Sisa {menu.RemainingClaim} klaim";

            base.OnBindingContextChanged();
        }

        private async void OnOwnerProfileTapped(object sender, EventArgs e)
        {
            var menu = BindingContext as Models.Menu;
            if (menu == null)
            {
                return;
            }

            try
            {
                var vm = this.Parent.Parent.BindingContext as BaseViewModel;
                await vm.NavigationService.CurrentPage.Navigation.PushAsync(new OwnerDetailPage(menu.Owner));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        private async void OnLikeImageTapped(object sender, EventArgs e)
        {
            var menu = BindingContext as Models.Menu;
            if (menu == null)
            {
                return;
            }

            if ((this.LikeImage.Source as FileImageSource).File == "heart_full.png")
            {
                this.LikeImage.Source = "heart_blank.png";
                await App.SavedMenuDatabase.DeleteMenuAsync(new LocalMenu(menu));
                DependencyService.Get<IMessageHelper>().ShortAlert($"{menu.Name} telah dikeluarkan dari daftar simpan.");
            }
            else
            {
                this.LikeImage.Source = "heart_full.png";
                await App.SavedMenuDatabase.SaveMenuAsync(new LocalMenu(menu));
                DependencyService.Get<IMessageHelper>().ShortAlert($"{menu.Name} telah tersimpan.");
            }
        }

        private void OnWhatsAppTapped(object sender, EventArgs e)
        {
            var menu = BindingContext as Models.Menu;
            if (menu == null)
            {
                return;
            }

            string text = "Hai Maemseller...";

            if (menu.Promoted)
            {
                string Url = $"http://maempedia.com/detailmenu.html?id={menu.PostID}";
                text = $"Hai Maemseller, saya tertarik dengan menu premium ini.\n*{menu.Name}* - {menu.Headline}\n\n{Url}";
            }

            string url = $"https://api.whatsapp.com/send?phone={menu.Owner.ContactWA}&text={text}";
            Device.OpenUri(new Uri(url));
        }

        private void Claim_Clicked(object sender, ClickedEventArgs e)
        {
            var menu = BindingContext as Models.Menu;
            if (menu == null)
            {
                return;
            }

            string text = $"Hai Maemseller, saya tertarik dengan menu terdiskon anda *{menu.Name}* apa masih tersedia? Terimakasih...";

            string url = $"https://api.whatsapp.com/send?phone={menu.Owner.ContactWA}&text={text}";
            Device.OpenUri(new Uri(url));
        }

        private void MenuImage_Success(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e)
        {
            var info = e.ImageInformation;
            if (info == null)
            {
                return;
            }

            this.MenuImage.HeightRequest = this.MenuImage.WidthRequest * ((double)info.OriginalHeight / (double)info.OriginalWidth);
        }
    }
}