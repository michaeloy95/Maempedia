using System;
using System.Collections;
using Xamarin.Forms;

namespace Maempedia.Custom
{
    public class BindableScrollView : ScrollView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(BindableScrollView),
                default(IEnumerable));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(BindableScrollView),
                default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly BindableProperty HeaderProperty =
            BindableProperty.Create(
                nameof(Header),
                typeof(object),
                typeof(BindableScrollView),
                default(object));

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly BindableProperty FooterProperty =
            BindableProperty.Create(
                nameof(Footer),
                typeof(object),
                typeof(BindableScrollView),
                default(object));

        public object Footer
        {
            get { return (object)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        public event EventHandler<ItemTappedEventArgs> ItemSelected;

        public void Render()
        {
            if (this.ItemTemplate == null || this.ItemsSource == null)
                return;

            var layout = new StackLayout();
            layout.Orientation = this.Orientation == ScrollOrientation.Vertical
                                    ? StackOrientation.Vertical 
                                    : StackOrientation.Horizontal;

            if (this.Header != null)
            {
                layout.Children.Add(this.Header as View);
            }

            foreach (var item in this.ItemsSource)
            {
                var viewCell = this.ItemTemplate.CreateContent() as ViewCell;
                viewCell.View.BindingContext = item;

                viewCell.View.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command((obj) =>
                    {
                        var args = new ItemTappedEventArgs(ItemsSource, item);
                        ItemSelected?.Invoke(this, args);
                    }),
                    NumberOfTapsRequired = 1
                });

                layout.Children.Add(viewCell.View);
            }

            if (this.Footer != null)
            {
                layout.Children.Add(this.Footer as View);
            }

            this.Content = layout;
        }
    }
}
