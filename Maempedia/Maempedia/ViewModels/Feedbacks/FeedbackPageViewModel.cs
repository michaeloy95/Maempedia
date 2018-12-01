
using Maempedia.Interfaces;
using Maempedia.Models;
using Maempedia.Views.Feedbacks;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Maempedia.ViewModels.Feedbacks
{
    public class FeedbackPageViewModel : BaseViewModel
    {
        public ICommand SelectMenuCommand { get; private set; }

        private IList<ProfileMenu> feedbackMenuList;
        public IList<ProfileMenu> FeedbackMenuList
        {
            get { return this.feedbackMenuList; }
            set { SetProperty<IList<ProfileMenu>>(ref this.feedbackMenuList, value); }
        }

        public FeedbackPageViewModel()
        {
            this.SelectMenuCommand = new Command<ProfileMenu>(this.ExecuteMenu);
            this.PrepareMenuItems();
        }

        private void PrepareMenuItems()
        {
            this.FeedbackMenuList = new List<ProfileMenu>()
            {
                new ProfileMenu()
                {
                    Title = "Kritik dan Saran",
                    Action = () => OpenPage(typeof(SuggestionPage))
                },
                new ProfileMenu()
                {
                    Title = "Laporkan Bug",
                    Action = () => OpenPage(typeof(ReportBugPage))
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
    }
}
