using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.Feedbacks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Feedbacks
{
    public class SuggestionPageViewModel : BaseViewModel
    {
        public ICommand SendMessageCommand { get; private set; }

        private string problemText;
        public string ProblemText
        {
            get { return this.problemText; }
            set { SetProperty<string>(ref this.problemText, value); }
        }

        private string suggestionText;
        public string SuggestionText
        {
            get { return this.suggestionText; }
            set { SetProperty<string>(ref this.suggestionText, value); }
        }

        public SuggestionPageViewModel()
        {
            this.SendMessageCommand = new Command(this.SendMessage);
        }

        public async void SendMessage()
        {
            await AccountService.SendFeedback(this.User.ID, this.ProblemText, this.SuggestionText);
            DependencyService.Get<IMessageHelper>().ShortAlert("Pesan telah dikirim. Terima kasih.");

            await this.NavigationService.GoBack(2);
        }
    }
}
