using Maempedia.Interfaces;
using Maempedia.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Maempedia.Services
{
    public class NavigationService : INavigationService
    {
        public Page CurrentPage
        {
            get
            {
                NavigationPage navPage = null;
                var mainPage = Application.Current.MainPage as MainPage;
                if (mainPage == null)
                {
                    navPage = Application.Current.MainPage as NavigationPage;

                    if (navPage.CurrentPage.GetType() == typeof(MainPage))
                    {
                        mainPage = navPage.CurrentPage as MainPage;
                        return mainPage.CurrentPage;
                    }

                    return navPage.CurrentPage;
                }

                navPage = mainPage.CurrentPage as NavigationPage;
                return navPage.CurrentPage;
            }
        }

        public Page CurrentMainPage
        {
            get
            {
                return Application.Current.MainPage;
            }
        }

        public NavigationService()
        {
        }

        public async Task GoBack()
        {
            await this.CurrentPage.Navigation.PopAsync();
        }

        public async Task GoBack(int pages)
        {
            var stack = this.CurrentMainPage.Navigation.NavigationStack;
            for (int i = 0; i < pages - 1; i++)
            {
                this.CurrentMainPage.Navigation.RemovePage(stack[stack.Count - 2]);
            }
            await this.GoBack();
        }

        public async Task NavigateTo(Type type)
        {
            await this.NavigateTo(type, null);
        }

        public async Task NavigateTo(Type type, object[] parameters)
        {
            Page page = (Page)Activator.CreateInstance(type, parameters);

            var stack = this.CurrentMainPage.Navigation.NavigationStack;
            if (stack[stack.Count - 1].GetType() != type)
            {
                await this.CurrentMainPage.Navigation.PushAsync(page, true);
            }
        }

        public async Task SwitchTo(Type type)
        {
            await this.SwitchTo(type, null);
        }

        public async Task SwitchTo(Type type, object[] parameters)
        {
            Page page = (Page)Activator.CreateInstance(type, parameters);

            var stack = this.CurrentMainPage.Navigation.NavigationStack;
            if (stack[stack.Count - 1].GetType() != type)
            {
                await this.CurrentMainPage.Navigation.PushAsync(page, true);
            }

            this.CurrentMainPage.Navigation.RemovePage(stack[stack.Count - 2]);
        }
    }
}
