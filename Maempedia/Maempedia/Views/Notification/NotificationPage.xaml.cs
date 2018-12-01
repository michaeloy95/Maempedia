using Maempedia.Models;
using Maempedia.ViewModels.Notification;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Notification
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        public NotificationPageViewModel ViewModel;

        public NotificationPage()
        {
            InitializeComponent();

            this.ViewModel = new NotificationPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;

            if (this.ViewModel.NotificationList?.Count == 0)
            {
                this.NullNotificationLayout.IsVisible = true;
                this.NotificationListView.IsVisible = false;
            }
            else
            {
                this.NullNotificationLayout.IsVisible = false;
                this.NotificationListView.IsVisible = true;
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            NotificationItem notification = e.SelectedItem as NotificationItem;
            if (notification == null)
                return;

            if (this.ViewModel.SelectItemCommand.CanExecute(notification))
            {
                this.ViewModel.SelectItemCommand.Execute(notification);
            }

            NotificationListView.SelectedItem = null;
        }
    }
}