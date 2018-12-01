using Android.Content;
using Maempedia.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.ListView), typeof(ListViewCustomRenderer))]
namespace Maempedia.Droid.Renderers
{
    class ListViewCustomRenderer : ListViewRenderer
    {
        public ListViewCustomRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            this.Control.HorizontalScrollBarEnabled = false;
            this.Control.VerticalScrollBarEnabled = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                this.Control.HorizontalScrollBarEnabled = false;
                this.Control.VerticalScrollBarEnabled = false;
            }
        }
    }
}