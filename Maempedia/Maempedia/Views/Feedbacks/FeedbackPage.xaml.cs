using Maempedia.Models;
using Maempedia.ViewModels.Feedbacks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Feedbacks
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackPage : ContentPage
    {
        public FeedbackPageViewModel ViewModel;

        public FeedbackPage()
        {
            InitializeComponent();

            this.ViewModel = new FeedbackPageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        private void FeedbackMenuListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as ProfileMenu;
            if (item == null)
                return;

            if (this.ViewModel.SelectMenuCommand.CanExecute(item))
            {
                this.ViewModel.SelectMenuCommand.Execute(item);
            }

            this.FeedbackMenuListView.SelectedItem = null;
        }
    }
}