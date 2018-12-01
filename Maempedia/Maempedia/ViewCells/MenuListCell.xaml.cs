using Maempedia.Data;
using Maempedia.Interfaces;
using Maempedia.Models;
using Maempedia.Views.Owner;
using System;
using System.Diagnostics;
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
    }
}