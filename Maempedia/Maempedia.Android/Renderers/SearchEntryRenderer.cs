using Android.Content;
using Maempedia.Custom;
using Maempedia.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchEntry), typeof(SearchEntryRenderer))]
namespace Maempedia.Droid.Renderers
{
    public class SearchEntryRenderer : SearchBarRenderer
    {
        public SearchEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.SearchBar> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                return;
            }

            var searchEntry = this.Control;
            searchEntry.SetBackgroundResource(Resource.Drawable.SearchEntry);
        }
    }
}