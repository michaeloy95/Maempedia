using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Models;
using Maempedia.Views.Login;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Menu
{
    public class CommentPageViewModel : BaseViewModel
    {
        public ICommand PostCommentCommand { get; private set; }

        public ICommand GotoProfileCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        private Models.Menu selectedMenu;
        public Models.Menu SelectedMenu
        {
            get { return this.selectedMenu; }
            set { SetProperty<Models.Menu>(ref this.selectedMenu, value); }
        }

        private ObservableCollection<Comment> commentList;
        public ObservableCollection<Comment> CommentList
        {
            get { return this.commentList; }
            set { SetProperty<ObservableCollection<Comment>>(ref this.commentList, value); }
        }

        private int startComment = 1;

        private readonly int countComment = 10;

        private string commentText = string.Empty;
        public string CommentText
        {
            get { return this.commentText; }
            set { SetProperty<string>(ref this.commentText, value); }
        }

        private string profilePictureThumb = string.Empty;
        public string ProfilePictureThumb
        {
            get { return this.profilePictureThumb; }
            set { SetProperty<string>(ref this.profilePictureThumb, value); }
        }

        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetProperty<bool>(ref this.isRefreshing, value); }
        }

        private bool isLoggedIn = false;
        public bool IsLoggedIn
        {
            get { return this.isLoggedIn; }
            set { SetProperty<bool>(ref this.isLoggedIn, value); }
        }

        private bool isNotLoggedIn = false;
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

        private bool isLoadingMore = false;
        public bool IsLoadingMore
        {
            get { return this.isLoadingMore; }
            set { SetProperty<bool>(ref this.isLoadingMore, value); }
        }

        public CommentPageViewModel(Models.Menu menu)
        {
            this.SelectedMenu = menu;
            this.PostCommentCommand = new Command(this.PostComment);
            this.GotoProfileCommand = new Command(this.GotoProfile);
            this.RefreshCommand = new Command(this.Refresh);
        }

        private async void InitialiseFields()
        {
            this.ProfilePictureThumb = this.User.ProfilePictureThumb;
            this.IsLoggedIn = this.User.HasLoggedIn;
            this.IsNotLoggedIn = !this.IsLoggedIn;

            this.IsRefreshing = true;

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                this.IsRefreshing = false;
                return;
            }

            var commentsData = await this.WebApiService.Comment.GetComments(
                this.SelectedMenu.ID,
                this.startComment,
                this.countComment);

            if (this.CommentList == null)
            {
                this.CommentList = new ObservableCollection<Comment>(commentsData.Item1);
            }
            else
            {
                int cpIdx = 0;
                foreach (Comment comment in commentsData.Item1)
                {
                    if (this.CommentList[cpIdx].ID == comment.ID)
                    {
                        break;
                    }
                    this.CommentList.Insert(cpIdx++, comment);
                }
            }

            this.IsRefreshing = false;
        }

        private void Refresh()
        {
            this.InitialiseFields();
        }

        public async Task LoadMoreComments()
        {
            if (this.IsLoadingMore)
                return;
            this.IsLoadingMore = true;

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                this.IsLoadingMore = false;
                return;
            }

            try
            {
                var moreCommentList = await this.WebApiService.Comment.GetComments(
                    this.SelectedMenu.ID,
                    this.CommentList.Count + 1,
                    this.countComment);

                if (moreCommentList == null)
                {
                    DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                    this.IsLoadingMore = false;
                    return;
                }

                foreach (Models.Comment comment in moreCommentList.Item1)
                {
                    this.CommentList.Add(comment);
                }
            }
            catch (Exception ex)
            {
                await this.NavigationService.CurrentPage.DisplayAlert("Terjadi Kesalahan", $"Error: {ex.Message}", "OK");
            }
            finally
            {
                this.IsLoadingMore = false;
            }

            this.IsLoadingMore = false;
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
