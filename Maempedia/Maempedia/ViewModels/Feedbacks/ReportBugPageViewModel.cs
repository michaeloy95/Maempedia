using Maempedia.Interfaces;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Feedbacks
{
    public class ReportBugPageViewModel : BaseViewModel
    {
        public ICommand SendReportCommand { get; private set; }

        private string messageText;
        public string MessageText
        {
            get { return this.messageText; }
            set { SetProperty<string>(ref this.messageText, value); }
        }

        public ReportBugPageViewModel()
        {
            this.SendReportCommand = new Command(this.SendReport);
        }

        public async void SendReport()
        {
            await this.WebApiService.Account.ReportBugs(this.User.ID, this.MessageText);
            DependencyService.Get<IMessageHelper>().ShortAlert("Pesan telah dikirim. Terima kasih.");
            
            await this.NavigationService.GoBack(2);
        }
    }
}
