using Maempedia.Custom;
using Maempedia.iOS.Renderers;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AdvancedListView), typeof(AdvancedListViewRenderer))]
namespace Maempedia.iOS.Renderers
{
    public class AdvancedListViewRenderer : ListViewRenderer
    {
        private void Control_Scrolled(object sender, EventArgs e)
        {
            var advancedListView = this.Element as AdvancedListView;
            if (advancedListView != null &&
                advancedListView.Scrolled != null)
            {
                advancedListView.Scrolled(new Point(Control.ContentOffset.X, Control.ContentOffset.Y));
            }
        }

        public AdvancedListViewRenderer()
        {
            UIApplication.CheckForEventAndDelegateMismatches = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            this.Control.Scrolled += Control_Scrolled;
        }
    }
}
