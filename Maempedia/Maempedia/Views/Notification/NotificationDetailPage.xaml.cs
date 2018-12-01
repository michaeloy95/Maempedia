using Maempedia.Models;
using Maempedia.ViewModels.Notification;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Notification
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationDetailPage : ContentPage
    {
        public NotificationDetailPageViewModel ViewModel;

        public NotificationDetailPage(NotificationItem notification)
        {
            InitializeComponent();

            this.ViewModel = new NotificationDetailPageViewModel(notification)
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }
    }
}