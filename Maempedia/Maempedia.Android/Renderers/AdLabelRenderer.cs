using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Maempedia.Droid.Renderers;
using Maempedia.Custom;
using Android.Content;

[assembly: ExportRenderer(typeof(AdLabel), typeof(AdLabelRenderer))]
namespace Maempedia.Droid.Renderers
{
    class AdLabelRenderer : LabelRenderer
    {
        public AdLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
        {
            base.OnElementChanged(e);
            var label = this.Control;
            label.SetBackgroundResource(Resource.Drawable.AdLabel);
            label.SetPadding(10, 5, 10, 5);
        }
    }
}