using FFImageLoading.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.ViewCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HorizontalMenuCell : ViewCell
    {
        public HorizontalMenuCell()
        {
            InitializeComponent();
        }

        private void CachedImage_Success(object sender, CachedImageEvents.SuccessEventArgs e)
        {
            var info = e.ImageInformation;
            this.DetailsLayout.WidthRequest = info.CurrentWidth;
            
            this.MenuHeadlineText.Text = this.MenuHeadlineHelperText.Text.Length < 50 ? this.MenuHeadlineHelperText.Text
                           : this.MenuHeadlineHelperText.Text.Substring(0, 50).Trim() + "...";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            this.MenuHeadlineText.Text = this.MenuHeadlineText.Text.Length < 50 ? this.MenuHeadlineHelperText.Text
                           : this.MenuHeadlineHelperText.Text.Substring(0, 50).Trim() + "...";
        }
    }
}