using Maempedia.ViewModels.Menu;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMenuDetailPage : ContentPage
    {
        public AddMenuDetailPageViewModel ViewModel;

        public AddMenuDetailPage()
        {
            InitializeComponent();

            this.ViewModel = new AddMenuDetailPageViewModel()
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

        private void OnEditorFocused(object sender, FocusEventArgs e)
        {
            var editor = sender as Editor;
            if (editor == null)
            {
                return;
            }

            editor.TextColor = Color.FromHex("#646464");
        }

        private void OnNameEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = this.ViewModel.CheckName();
            entry.TextColor = valid ? Color.FromHex("#646464")
                            : Color.FromHex("#CF000F");

            this.ViewModel.CheckValidity();
        }

        private void OnDescriptionEntryUnfocused(object sender, FocusEventArgs e)
        {
            var editor = sender as Editor;
            if (editor == null)
            {
                return;
            }

            bool valid = this.ViewModel.CheckDescription();
            editor.TextColor = valid ? Color.FromHex("#646464")
                             : Color.FromHex("#CF000F");

            this.ViewModel.CheckValidity();
        }

        private void OnPriceEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = this.ViewModel.CheckPrice();
            entry.TextColor = valid ? Color.FromHex("#646464")
                            : Color.FromHex("#CF000F");

            this.ViewModel.CheckValidity();
        }

        private void OnPortionEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null)
            {
                return;
            }

            bool valid = this.ViewModel.CheckPortion();
            entry.TextColor = valid ? Color.FromHex("#646464")
                            : Color.FromHex("#CF000F");

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

            if (entry == this.NameEntry)
            {
                this.DescriptionEditor.Focus();
            }
            else if (entry == this.PriceEntry)
            {
                this.PortionEntry.Focus();
            }
        }

        private void OnEditorCompleted(object sender, System.EventArgs e)
        {
            var editor = sender as Editor;
            if (editor == null)
            {
                return;
            }

            editor.Unfocus();

            if (editor == this.DescriptionEditor)
            {
                this.PriceEntry.Focus();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.ViewModel.CheckValidity();
        }
    }
}