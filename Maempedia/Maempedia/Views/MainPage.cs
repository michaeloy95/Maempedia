using BottomBar.XamarinForms;
using Maempedia.Views.Browse;
using Maempedia.Views.MVoucher;
using Maempedia.Views.Notification;
using Maempedia.Views.Profile;
using Maempedia.Views.Saved;
using System;
using Xamarin.Forms;

namespace Maempedia.Views
{
    public class MainPage : BottomBarPage
    {
        private const int MENU_COUNT = 5;

        private string[] titles = { "Maempedia", "Daftar Simpan", "M-Voucher", "Notifikasi", "Akun" };

        private string[] iconNames = { "ic_home", "ic_saved", "ic_mvoucher", "ic_notification", "ic_profile" };

        private Type[] tabPages = new Type[MENU_COUNT] { typeof(BrowsePage), typeof(SavedPage), typeof(MVoucherPage), typeof(NotificationPage), typeof(UnloggedProfilePage) };
        
        public MainPage()
        {
            this.BarTextColor = Color.FromHex("#CF000F");
            this.BarBackgroundColor = Color.FromHex("#F8F8F8");
            this.FixedMode = true;

            if (App.User.HasLoggedIn)
            {
                this.tabPages[4] = typeof(LoggedProfilePage);
            }

            for (int i = 0; i < MENU_COUNT; i++)
            {
                FileImageSource icon = (FileImageSource)FileImageSource.FromFile(this.iconNames[i]);
                Page tabPage = (Page)Activator.CreateInstance(this.tabPages[i]);

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        this.Children.Add(new NavigationPage(tabPage)
                        {
                            Icon = icon,
                            Title = string.Empty
                        });
                        break;
                    case Device.Android:
                        tabPage.Icon = icon;
                        tabPage.Title = string.Empty;
                        this.Children.Add(tabPage);
                        break;
                }

                BottomBarPageExtensions.SetTabColor(tabPage, Color.FromHex("#F8F8F8"));
            }

            this.Title = this.titles[0];
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            
            if (this.CurrentPage.GetType() == typeof(UnloggedProfilePage))
            {
                this.Title = "";
                return;
            }

            for (int idx = 0; idx < this.Children.Count; idx++)
            {
                if (this.CurrentPage.Equals(this.Children[idx]))
                {
                    this.Title = this.titles[idx];
                }
            }
        }
    }
}
