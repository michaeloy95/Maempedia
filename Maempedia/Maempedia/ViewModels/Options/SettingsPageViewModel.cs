using Maempedia.Interfaces;
using Maempedia.Views.Options;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Options
{
    public class SettingsPageViewModel : BaseViewModel
    {
        public ICommand OpenPrivacyPolicyCommand { get; private set; }

        public bool IsPushNotification
        {
            get { return Data.Settings.AllowPushNotification; }
            set { Data.Settings.AllowPushNotification = value; }
        }

        public string VersionText
        {
            get
            {
                var device = DependencyService.Get<IDeviceUtility>();
                return $"{device.AppVersion} / {device.BuildVersion}";
            }
        }

        public SettingsPageViewModel()
        {
            this.OpenPrivacyPolicyCommand = new Command(OpenPrivacyPolicy);
        }

        private async void OpenPrivacyPolicy()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(PrivacyPolicyPage));

            this.IsBusy = false;
        }
    }
}
