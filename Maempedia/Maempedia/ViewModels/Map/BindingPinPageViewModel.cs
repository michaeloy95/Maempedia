using System;

namespace Maempedia.ViewModels.Map
{
    public class BindingPinPageViewModel : BaseViewModel
    {
        private string imageSource = String.Empty;
        public string ImageSource
        {
            get { return this.imageSource; }
            set { SetProperty<string>(ref this.imageSource, value); }
        }

        public BindingPinPageViewModel(string imageSource)
        {
            this.ImageSource = imageSource;
        }
    }
}
