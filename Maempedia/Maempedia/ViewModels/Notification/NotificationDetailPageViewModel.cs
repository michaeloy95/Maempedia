using Maempedia.Models;

namespace Maempedia.ViewModels.Notification
{
    public class NotificationDetailPageViewModel : BaseViewModel
    {
        private NotificationItem selectedNotification;
        public NotificationItem SelectedNotification
        {
            get { return this.selectedNotification; }
            set { SetProperty<NotificationItem>(ref this.selectedNotification, value); }
        }

        public NotificationDetailPageViewModel(NotificationItem notification)
        {
            this.SelectedNotification = notification;
        }
    }
}
