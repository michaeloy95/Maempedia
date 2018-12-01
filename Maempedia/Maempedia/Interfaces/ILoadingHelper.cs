namespace Maempedia.Interfaces
{
    public interface ILoadingHelper
    {
        void Show(string message = "Sedang memuat...");

        void Hide();
    }
}
