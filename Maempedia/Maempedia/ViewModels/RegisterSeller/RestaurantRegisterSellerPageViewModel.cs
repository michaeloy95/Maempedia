using Maempedia.Enum;
using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views.RegisterSeller;
using Plugin.Connectivity;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.RegisterSeller
{
    public class RestaurantRegisterSellerPageViewModel : BaseViewModel
    {
        public ICommand NextCommand { get; private set; }

        private Models.Owner curOwner = null;
        public Models.Owner CurOwner
        {
            get { return this.curOwner; }
            set { SetProperty<Models.Owner>(ref this.curOwner, value); }
        }

        private string nameText = string.Empty;
        public string NameText
        {
            get { return this.nameText; }
            set { SetProperty<string>(ref this.nameText, value); }
        }

        private string headlineText = string.Empty;
        public string HeadlineText
        {
            get { return this.headlineText; }
            set { SetProperty<string>(ref this.headlineText, value); }
        }

        private TimeSpan openingTime = new TimeSpan(9, 0, 0);
        public TimeSpan OpeningTime
        {
            get { return this.openingTime; }
            set { SetProperty<TimeSpan>(ref this.openingTime, value); }
        }

        private TimeSpan closingTime = new TimeSpan(20, 0, 0);
        public TimeSpan ClosingTime
        {
            get { return this.closingTime; }
            set { SetProperty<TimeSpan>(ref this.closingTime, value); }
        }

        private bool nameIsValid = false;
        public bool NameIsValid
        {
            get { return this.nameIsValid; }
            set { SetProperty<bool>(ref this.nameIsValid, value); }
        }

        private bool nameIsChecking = false;
        public bool NameIsChecking
        {
            get { return this.nameIsChecking; }
            set { SetProperty<bool>(ref this.nameIsChecking, value); }
        }

        private bool nextCommandEnabled = false;
        public bool NextCommandEnabled
        {
            get { return this.nextCommandEnabled; }
            set { SetProperty<bool>(ref this.nextCommandEnabled, value); }
        }

        public RestaurantRegisterSellerPageViewModel(Models.Owner owner)
        {
            this.NextCommand = new Command(this.GoNext);

            this.CurOwner = owner;
        }

        public void CheckValidity()
        {
            if (this.NameIsValid)
            {
                this.NextCommandEnabled = true;
            }
            else
            {
                this.NextCommandEnabled = false;
            }
        }

        public async Task<bool> CheckName()
        {
            this.NameIsValid = false;

            if (String.IsNullOrEmpty(this.NameText) ||
                String.IsNullOrWhiteSpace(this.NameText))
            {
                return false;
            }

            const int MINIM_NAME_COUNT = 5;
            if (this.NameText.Length < MINIM_NAME_COUNT)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Nama harus terdiri dari {MINIM_NAME_COUNT} karakter atau lebih.");
                return false;
            }

            Regex r = new Regex("^[a-zA-Z0-9]*$");
            if (!r.IsMatch(this.NameText.Replace(" ", string.Empty)))
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Nama hanya dapat terdiri dari huruf dan angka.");
                return false;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Gagal memuat. Periksa kembali koneksi internet anda.");
                return false;
            }

            this.NameIsChecking = true;
            var results = await AccountService.CheckNameIsValid(this.NameText);
            this.NameIsChecking = false;

            if (results != ServerResponseStatus.VALID)
            {
                DependencyService.Get<IMessageHelper>().LongAlert($"Username telah digunakan.");
                return false;
            }

            this.NameIsValid = true;
            return true;
        }

        private async void GoNext()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            if (!this.NextCommandEnabled)
            {
                this.IsBusy = false;
                return;
            }

            this.CurOwner.Name = this.NameText;
            this.CurOwner.OpeningHour = $"{this.OpeningTime:hh\\:mm}";
            this.CurOwner.ClosingHour = $"{this.ClosingTime:hh\\:mm}";
            this.CurOwner.Headline = this.HeadlineText;
            
            await this.NavigationService.NavigateTo(typeof(ProfilePictureRegisterSellerPage), new object[] { this.CurOwner });

            this.IsBusy = false;
        }
    }
}
