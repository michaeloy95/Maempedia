using Maempedia.Interfaces;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Options
{
    public class PrivacyPolicyPageViewModel : BaseViewModel
    {
        public PrivacyPolicyPageViewModel()
        {
        }

        public void OnNavigating()
        {
            DependencyService.Get<ILoadingHelper>().Show();
        }

        public void OnNavigated()
        {
            DependencyService.Get<ILoadingHelper>().Hide();
        }
    }
}
