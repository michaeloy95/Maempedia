using Maempedia.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.ViewCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationListCell : ViewCell
    {
        public NotificationListCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            var item = BindingContext as Models.NotificationItem;
            if (item == null)
            {
                return;
            }

            this.NotificationImage.Source = item.ImageSource;
            this.NotificationImage.WidthRequest = (App.ScreenWidth/4 - NotificationImage.Margin.Left - NotificationImage.Margin.Right) * Settings.ImageQuality;

            base.OnBindingContextChanged();
        }
    }
}