using Maempedia.Interfaces;
using Maempedia.Models;
using Maempedia.Views.Browse;
using Maempedia.Views.Feedbacks;
using Maempedia.Views.Information;
using Maempedia.Views.Options;
using Maempedia.Views.Profile;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Profile
{
    public class LoggedProfilePageViewModel : BaseViewModel
    {
        public ICommand ViewProfileCommand { get; private set; }

        public ICommand SelectProfileMenuCommand { get; private set; }

        private string name;
        public string Name
        {
            get { return this.name; }
            set { SetProperty<string>(ref this.name, value); }
        }

        private string profilePicture;
        public string ProfilePicture
        {
            get { return this.profilePicture; }
            set { SetProperty<string>(ref this.profilePicture, value); }
        }

        public string WorkingHours
        {
            get { return $"{this.User.OpeningHour} - {this.User.ClosingHour}"; }
        }

        private IList<ProfileMenu> profileMenuList;
        public IList<ProfileMenu> ProfileMenuList
        {
            get { return this.profileMenuList; }
            set { SetProperty<IList<ProfileMenu>>(ref this.profileMenuList, value); }
        }

        public LoggedProfilePageViewModel()
        {
            this.ViewProfileCommand = new Command(this.ViewProfile);
            this.SelectProfileMenuCommand = new Command<ProfileMenu>(this.ExecuteMenu);
            this.RefreshUserDetails();
            this.PrepareMenuItems();
        }

        private async void ViewProfile()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            
            await this.NavigationService.NavigateTo(typeof(ViewProfilePage));

            this.IsBusy = false;
        }

        private void PrepareMenuItems()
        {
            this.ProfileMenuList = new List<ProfileMenu>()
            {
                new ProfileMenu()
                {
                    IconSource = "ic_contact.png",
                    Title = "Kontak",
                    //Action = () => OpenPage(typeof(...))
                },
                new ProfileMenu()
                {
                    IconSource = "ic_invite.png",
                    Title = "Undang teman",
                    //Action = () => OpenPage(typeof(...))
                },
                new ProfileMenu()
                {
                    IconSource = "ic_settings.png",
                    Title = "Pengaturan",
                    Action = () => OpenPage(typeof(SettingsPage))
                },
                new ProfileMenu()
                {
                    IconSource = "ic_info.png",
                    Title = "Informasi",
                    Action = () => OpenPage(typeof(InformationPage))
                },
                new ProfileMenu()
                {
                    IconSource = "ic_report.png",
                    Title = "Beri Masukan",
                    Action = () => OpenPage(typeof(FeedbackPage))
                },
                new ProfileMenu()
                {
                    IconSource = "ic_logout.png",
                    Title = "Keluar",
                    Action = Logout
                }
            };
        }

        private void ExecuteMenu(ProfileMenu menu)
        {
            try
            {
                menu.Action();
            }
            catch
            {
                DependencyService.Get<IMessageHelper>().ShortAlert("Menu belum tersedia.");
            }
        }

        private async void OpenPage(Type pageType)
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            await this.NavigationService.NavigateTo(pageType);

            this.IsBusy = false;
        }

        private async void Logout()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            bool logout = await this.NavigationService.CurrentPage.DisplayAlert(
                "Keluar Akun",
                "Apakah anda yakin ingin keluar dari akun Maempedia?",
                "Keluar",
                "Batal");

            if (logout)
            {
                this.User.SetLogin(false);

                // Direct user to explore
                ((App)App.Current).ReplaceTabPage(0, new BrowsePage(), "ic_home");
                ((App)App.Current).SwitchTab(0);
                ((App)App.Current).ReplaceTabPage(4, new UnloggedProfilePage(), "ic_profile");

                DependencyService.Get<IMessageHelper>().ShortAlert($"Anda telah keluar.");
            }

            this.IsBusy = false;
        }

        public void RefreshUserDetails()
        {
            this.Name = this.User.Name;
            this.ProfilePicture = this.User.ProfilePictureThumb;
        }
    }
}
