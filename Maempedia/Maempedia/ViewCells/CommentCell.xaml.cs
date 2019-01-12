using Maempedia.Interfaces;
using Maempedia.Models;
using Maempedia.Services.WebApi;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.ViewCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentCell : ViewCell
    {
        public CommentCell()
        {
            InitializeComponent();
        }

        private void Report_Clicked(object sender, EventArgs e)
        {
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var item = this.BindingContext as Models.Comment;
            if (item == null)
            {
                return;
            }

            try
            {
                var vm = this.Parent.Parent.BindingContext as BaseViewModel;

                bool response = await vm.NavigationService.CurrentPage.DisplayAlert(
                    "Hapus Komentar",
                    "Apakah anda yakin ingin menghapus komentar?",
                    "Hapus",
                    "Batal");

                if (response)
                {
                    vm.IsBusy = true;

                    await WebApiService.Instance.Comment.DeleteComment(
                        item.ID,
                        vm.User.Username,
                        vm.User.Password);
                    ((ObservableCollection<Comment>)(this.Parent as ListView).ItemsSource).Remove(item);

                    DependencyService.Get<IMessageHelper>().ShortAlert("Komen dihapus");

                    vm.IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        protected override void OnAppearing()
        {
            var item = this.BindingContext as Models.Comment;
            if (item == null)
            {
                return;
            }

            try
            {
                var vm = this.Parent.Parent.BindingContext as BaseViewModel;

                if (item.UserID == vm.User.ID)
                {
                    this.ContextActions.Clear();
                    this.ContextActions.Add(
                        new MenuItem()
                        {
                            CommandParameter = item.ID,
                            Text = "Delete",
                            Icon = "ic_delete",
                            IsDestructive = true
                        });
                    this.ContextActions[0].Clicked += this.Delete_Clicked;
                }
                //else
                //{
                //    this.ContextActions.Add(
                //        new MenuItem()
                //        {
                //            CommandParameter = item.ID,
                //            Text = "Report",
                //            Icon = "ic_report_comment",
                //            IsDestructive = false
                //        });
                //    this.ContextActions[0].Clicked += this.Report_Clicked;
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }

            base.OnAppearing();
        }
    }
}