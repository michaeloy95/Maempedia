using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maempedia.Custom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StrokeLabel : Grid
    {
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(StrokeLabel),
                propertyChanged: OnTextPropertyChanged);

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(FontAttributes),
                typeof(StrokeLabel),
                propertyChanged: OnFontAttributesPropertyChanged);

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(double),
                typeof(StrokeLabel),
                propertyChanged: OnFontSizePropertyChanged);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(Color),
                typeof(StrokeLabel),
                propertyChanged: OnTextColorPropertyChanged);

        public StrokeLabel()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (StrokeLabel)bindable;
            view.label.Text = view.Text;
        }

        private static void OnFontAttributesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (StrokeLabel)bindable;
            view.label.FontAttributes = view.FontAttributes;
        }

        private static void OnFontSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (StrokeLabel)bindable;
            view.label.FontSize = view.FontSize;
        }

        private static void OnTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (StrokeLabel)bindable;
            view.label.TextColor = view.TextColor;
        }
    }
}