using Android.Content;
using Maempedia.Custom;
using Maempedia.Droid.Renderers;
using System;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(AdvancedListView), typeof(AdvancedListViewRenderer))]
namespace Maempedia.Droid.Renderers
{
    public class AdvancedListViewRenderer : ListViewRenderer
    {
        private void Control_Scrolled(object sender, EventArgs e)
        {
            var advancedListView = this.Element as AdvancedListView;
            if (advancedListView != null &&
                advancedListView.Scrolled != null)
            {
                advancedListView.Scrolled(new Xamarin.Forms.Point(Control.ScrollX, Control.ScrollY));                
            }
        }

        public AdvancedListViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            this.Control.Scroll += Control_Scrolled;
        }
    }
}