using Maempedia.ViewModels.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        public EditProfilePageViewModel ViewModel;

        public EditProfilePage()
        {
            InitializeComponent();

            this.ViewModel = new EditProfilePageViewModel()
            {
                Title = this.Title
            };
            this.BindingContext = this.ViewModel;
        }

        private void OnEntryFocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            entry.TextColor = Color.FromHex("#646464");
        }

        private async void OnNameEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = await this.ViewModel.CheckName();
            entry.TextColor = valid ? Color.FromHex("#646464")
                            : Color.FromHex("#CF000F");

            this.ViewModel.CheckValidity();
        }

        private void OnHeadlineEntryUnfocused(object sender, FocusEventArgs e)
        {
            var editor = sender as Editor;
            if (editor == null)
            {
                return;
            }

            this.ViewModel.CheckValidity();
        }

        private void OnEntryCompleted(object sender, System.EventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            entry.Unfocus();
        }

        private void OnEditorCompleted(object sender, System.EventArgs e)
        {
            var editor = sender as Editor;
            if (editor == null)
            {
                return;
            }

            editor.Unfocus();
        }

        private void ProfileImage_Success(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e)
        {
            var info = e.ImageInformation;
            if (info == null)
            {
                return;
            }

            this.ViewModel.ImageAspectRatio = (float)info.OriginalHeight / (float)info.OriginalWidth;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.ViewModel.InitialiseData();
            this.ViewModel.CheckValidity();
        }
    }
}