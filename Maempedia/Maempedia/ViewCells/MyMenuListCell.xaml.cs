using Maempedia.Data;
using Maempedia.Views.Menu;
using Maempedia.Views.Promotion;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.ViewCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyMenuListCell : ViewCell
    {
        public MyMenuListCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            var item = BindingContext as Models.Menu;
            if (item == null)
            {
                return;
            }

            this.MenuImage.Source = item.ImageSource;

            double padding = this.MenuImageLayout.Padding.Left + this.MenuImageLayout.Padding.Right;
            double margin = this.MenuImage.Margin.Left + this.MenuImage.Margin.Right;
            this.MenuImage.WidthRequest = ((App.ScreenWidth * 2 / 5) - padding - margin) * Settings.ImageQuality;
            this.MenuImage.DownsampleHeight = this.MenuImage.WidthRequest;

            this.MenuHeadlineText.Text = item.Headline.Length < 50 ? item.Headline
                                       : item.Headline.Substring(0, 50).Trim() + "...";

            this.PromoteButton.IsVisible = !item.Promoted;

            base.OnBindingContextChanged();
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

        private async void Promote_Clicked(object sender, System.EventArgs e)
        {
            var menu = BindingContext as Models.Menu;
            if (menu == null)
            {
                return;
            }

            try
            {
                var vm = this.Parent.Parent.BindingContext as BaseViewModel;
                
                await vm.NavigationService.NavigateTo(typeof(PromotionPage), new object[] { menu.ID });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}