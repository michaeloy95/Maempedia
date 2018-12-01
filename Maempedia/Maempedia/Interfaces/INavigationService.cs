using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Maempedia.Interfaces
{
    public interface INavigationService
    {
        Page CurrentPage { get; }

        Page CurrentMainPage { get; }

        Task GoBack();

        Task GoBack(int pages);

        Task NavigateTo(Type type);

        Task NavigateTo(Type type, object[] parameters);

        Task SwitchTo(Type type);

        Task SwitchTo(Type type, object[] parameters);
    }
}
