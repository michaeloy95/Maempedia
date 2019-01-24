using Maempedia.Data;
using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Models;
using Maempedia.Views.Login;
using Maempedia.Views.Map;
using Maempedia.Views.Menu;
using Maempedia.Views.Owner;
using Plugin.Connectivity;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Menu
{
    public class MenuDetailPageViewModel : BaseViewModel
    {
        public ICommand OpenOwnerCommand { get; private set; }

        public ICommand OpenWhatsAppCommand { get; private set; }

        public ICommand ShareCommand { get; private set; }

        public ICommand OpenMapDetailCommand { get; private set; }

        public ICommand RefreshPageCommand { get; private set; }

        public ICommand PostCommentCommand { get; private set; }

        public ICommand GotoProfileCommand { get; private set; }

        public ICommand ViewCommentsCommand { get; private set; }

        private Models.Menu selectedMenu;
        public Models.Menu SelectedMenu
        {
            get { return this.selectedMenu; }
            set { SetProperty<Models.Menu>(ref this.selectedMenu, value); }
        }

        public double MenuImageWidth
        {
            get { return App.ScreenWidth * Settings.ImageQuality; }
        }

        public string WorkingHours
        {
            get { return $"{SelectedMenu.Owner.OpeningHour} - {SelectedMenu.Owner.ClosingHour}"; }
        }
        
        private ObservableCollection<Comment> commentList;
        public ObservableCollection<Comment> CommentList
        {
            get { return this.commentList; }
            set { SetProperty<ObservableCollection<Comment>>(ref this.commentList, value); }
        }

        private string viewCommentsText = string.Empty;
        public string ViewCommentsText
        {
            get { return this.viewCommentsText; }
            set { SetProperty<string>(ref this.viewCommentsText, value); }
        }

        private string commentText = string.Empty;
        public string CommentText
        {
            get { return this.commentText; }
            set { SetProperty<string>(ref this.commentText, value); }
        }

        private bool noComments = true;
        public bool NoComments
        {
            get { return this.noComments; }
            set { SetProperty<bool>(ref this.noComments, value); }
        }

        private bool showViewComments = false;
        public bool ShowViewComments
        {
            get { return this.showViewComments; }
            set { SetProperty<bool>(ref this.showViewComments, value); }
        }

        private string profilePictureThumb = string.Empty;
        public string ProfilePictureThumb
        {
            get { return this.profilePictureThumb; }
            set { SetProperty<string>(ref this.profilePictureThumb, value); }
        }

        private bool isLoggedIn = true;
        public bool IsLoggedIn
        {
            get { return this.isLoggedIn; }
            set { SetProperty<bool>(ref this.isLoggedIn, value); }
        }

        private bool isNotLoggedIn = true;
        public bool IsNotLoggedIn
        {
            get { return this.isNotLoggedIn; }
            set { SetProperty<bool>(ref this.isNotLoggedIn, value); }
        }

        private bool canPostComment = false;
        public bool CanPostComment
        {
            get { return this.canPostComment; }
            set { SetProperty<bool>(ref this.canPostComment, value); }
        }

        public MenuDetailPageViewModel(Models.Menu menu)
        {
            this.SelectedMenu = menu;
            this.OpenOwnerCommand = new Command(this.OpenOwner);
            this.OpenWhatsAppCommand = new Command(this.OpenWhatsApp);
            this.ShareCommand = new Command(this.ShareMenu);
            this.OpenMapDetailCommand = new Command(this.OpenMapDetail);
            this.RefreshPageCommand = new Command(this.RefreshPage);
            this.PostCommentCommand = new Command(this.PostComment);
            this.GotoProfileCommand = new Command(this.GotoProfile);
            this.ViewCommentsCommand = new Command(this.ViewComments);

            this.InitialiseFields();
        }

        private async void InitialiseFields()
        {
            this.ProfilePictureThumb = this.User.ProfilePictureThumb;
            var commentsData = await this.WebApiService.Comment.GetComments(this.SelectedMenu.ID, 1, 2);

            if (commentsData != null)
            {
                this.CommentList = new ObservableCollection<Comment>(commentsData.Item1);

                this.NoComments = this.CommentList == null || commentsData.Item2 == 0;
                this.ShowViewComments = commentsData.Item2 > 2;

                if (this.ShowViewComments)
                {
                    this.ViewCommentsText = $"Lihat {commentsData.Item2 - 2} komentar lainnya.";
                }
            }

            this.IsLoggedIn = this.User.HasLoggedIn;
            this.IsNotLoggedIn = !this.IsLoggedIn;
        }

        public void RefreshPage()
        {
            this.InitialiseFields();
        }
        
        private async void OpenOwner()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(OwnerDetailPage), new object[] { this.SelectedMenu.Owner });

            this.IsBusy = false;
        }

        private void OpenWhatsApp()
        {
            string url = $"https://api.whatsapp.com/send?phone={this.SelectedMenu.Owner.ContactWA}";
            Device.OpenUri(new Uri(url));
        }

        private void ShareMenu()
        {
            CrossShare.Current.Share(new ShareMessage
            {
                Text = $"{this.SelectedMenu.Name} - {this.SelectedMenu.Headline}",
                Title = $"{this.SelectedMenu.Name}",
                Url = $"http://maempedia.com/detailmenu.html?id={this.SelectedMenu.PostID}"
            });
        }

        private async void OpenMapDetail()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(MapDetailPage), new object[] { this.SelectedMenu.Owner });

            this.IsBusy = false;
        }

        private void ViewComments()
        {
            this.NavigationService.NavigateTo(typeof(CommentPage), new object[] { this.SelectedMenu });
        }

        public void CheckComment()
        {
            if (String.IsNullOrEmpty(this.CommentText) ||
                String.IsNullOrWhiteSpace(this.CommentText))
            {
                this.CanPostComment = false;
            }
            else
            {
                this.CanPostComment = true;
            }
        }

        private async void PostComment()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            if (!this.CanPostComment)
            {
                this.IsBusy = false;
                return;
            }

            var loading = DependencyService.Get<ILoadingHelper>();
            loading.Show();

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal mengirim. Periksa kembali koneksi internet anda.");
                this.IsBusy = false;
                return;
            }

            var result = await this.WebApiService.Comment.AddComment(
                this.CommentText,
                this.User.ID,
                this.SelectedMenu.ID,
                this.User.Username,
                this.User.Password);

            switch (result)
            {
                case ServerResponseStatus.INVALID:
                    await this.NavigationService.CurrentPage.DisplayAlert("Gagal Memposting Komentar", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    loading.Hide();
                    return;

                case ServerResponseStatus.ERROR:
                    await this.NavigationService.CurrentPage.DisplayAlert("Gagal Memposting Komentar", "Terjadi kesalahan pada server. Coba lagi nanti.", "OK");
                    this.IsBusy = false;
                    loading.Hide();
                    return;
            }

            var newComment = (await this.WebApiService.Comment.GetComments(this.SelectedMenu.ID, 1, 1)).Item1[0];
            this.CommentList.Insert(0, newComment);

            this.CommentText = string.Empty;

            loading.Hide();

            this.IsBusy = false;
        }

        private void GotoProfile()
        {
            this.NavigationService.NavigateTo(typeof(LoginPage));
        }
    }
}
