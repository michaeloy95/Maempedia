using Android.Content;
using Maempedia.Custom;
using Maempedia.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FlatButton), typeof(FlatButtonRenderer))]
namespace Maempedia.Droid.Renderers
{
    public class FlatButtonRenderer : ButtonRenderer
    {
        public FlatButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
                return;
            
            var element = e.NewElement as FlatButton;

            var button = this.Control;
            button.SetAllCaps(false);
            button.SetPadding(
                (int)element.Padding.Left,
                (int)element.Padding.Top,
                (int)element.Padding.Right,
                (int)element.Padding.Bottom);
        }
    }
}