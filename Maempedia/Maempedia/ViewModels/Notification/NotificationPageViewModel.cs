using Maempedia.Models;
using Maempedia.Views.Notification;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Notification
{
    public class NotificationPageViewModel : BaseViewModel
    {
        public ICommand SelectItemCommand { get; private set; }

        private ObservableCollection<NotificationItem> notificationList = null;
        public ObservableCollection<NotificationItem> NotificationList
        {
            get { return notificationList; }
            set { SetProperty<ObservableCollection<NotificationItem>>(ref notificationList, value); }
        }

        public NotificationPageViewModel()
        {
            this.SelectItemCommand = new Command<NotificationItem>(this.SelectItem);
            this.PrepareNotificationItem();
        }

        private void PrepareNotificationItem()
        {
            this.NotificationList = new ObservableCollection<NotificationItem>();
        }

        private async void SelectItem(NotificationItem notif)
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(NotificationDetailPage), new object[] { notif });

            this.IsBusy = false;
        }
    }
}
