using BottomBar.XamarinForms;
using Maempedia.Data;
using Maempedia.Interfaces;
using Maempedia.Services;
using Maempedia.Views;
using Plugin.Connectivity;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Maempedia
{
    public partial class App : Xamarin.Forms.Application
    {
        public static double ScreenWidth
        {
            get;
            set;
        }
        
        public static double ScreenHeight
        {
            get;
            set;
        }

        public static double Scale
        {
            get;
            set;
        }

        public static double NavigationBarHeight
        {
            get;
            set;
        }

        public static User user = null;
        public static User User
        {
            get
            {
                // first initialisation
                user = user ?? new User();

                // synchronise user's details
                if (user.HasLoggedIn && !user.ProfileSynchronised)
                {
                    Task.Run(async () =>
                    {
                        user.ProfileSynchronised = true;

                        if (!CrossConnectivity.Current.IsConnected)
                        {
                            user.ProfileSynchronised = false;
                            return;
                        }

                        var owner = await OwnerService.GetOwner(user.ID);
                        if (owner == null)
                        {
                            user.ProfileSynchronised = false;
                            return;
                        }

                        user.SetUser(owner);
                    });
                }

                return user;
            }
        }

        public static LocalDatabaseSavedMenu savedMenuDatabase = null;
        public static LocalDatabaseSavedMenu SavedMenuDatabase
        {
            get
            {
                if (savedMenuDatabase == null)
                {
                    savedMenuDatabase = new LocalDatabaseSavedMenu(DependencyService.Get<IFileHelper>().GetLocalFilePath("SavedMenuDB.db3"));
                }
                return savedMenuDatabase;
            }
        }

        public static INavigationService navigationService = null;
        public static INavigationService NavigationService
        {
            get
            {
                navigationService = navigationService ?? new NavigationService();
                return navigationService;
            }
        }

        public App()
        {
            InitializeComponent();
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    this.MainPage = new MainPage();
                    break;
                case Device.Android:
                    this.MainPage = new NavigationPage(new MainPage());
                    break;
            }

            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        public void SwitchTab(int tabPageNum)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    ((BottomBarPage)MainPage).CurrentPage = ((BottomBarPage)MainPage).Children[tabPageNum];
                    break;
                case Device.Android:
                    var bottomBarPage = ((NavigationPage)MainPage).Navigation.NavigationStack[0] as BottomBarPage;
                    bottomBarPage.CurrentPage = bottomBarPage.Children[tabPageNum];
                    break;
            }
        }

        public void ReplaceTabPage(int tabPageNum, Page tabPage, string iconName)
        {
            FileImageSource icon = (FileImageSource)FileImageSource.FromFile(iconName);

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    ((BottomBarPage)MainPage).Children[tabPageNum] = new NavigationPage(tabPage) { Icon = icon };
                    break;
                case Device.Android:
                    var bottomBarPage = ((NavigationPage)MainPage).Navigation.NavigationStack[0] as BottomBarPage;
                    tabPage.Icon = icon;
                    bottomBarPage.Children[tabPageNum] = tabPage;
                    break;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}