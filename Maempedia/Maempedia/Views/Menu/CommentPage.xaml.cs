using Maempedia.Models;
using Maempedia.ViewModels.Menu;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentPage : ContentPage
    {
        public CommentPageViewModel ViewModel;

        public CommentPage(Models.Menu menu)
        {
            InitializeComponent();

            this.ViewModel = new CommentPageViewModel(menu)
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        private async void CommentsListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            int currentIdx = ViewModel.CommentList.IndexOf((Models.Comment)e.Item);

            if (this.ViewModel.CommentList.Count <= currentIdx + 1)
            {
                await this.ViewModel.LoadMoreComments();
            }
        }

        private void CommentEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != e.OldTextValue)
            {
                this.ViewModel.CheckComment();
            }
        }

        private void PostButton_Tapped(object sender, System.EventArgs e)
        {
            this.CommentsListView
                .ScrollTo(
                    this.ViewModel.CommentList[0],
                    ScrollToPosition.Start,
                    true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.ViewModel.RefreshCommand.Execute(null);
        }
    }
}