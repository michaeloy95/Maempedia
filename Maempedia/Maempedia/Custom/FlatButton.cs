using Xamarin.Forms;

namespace Maempedia.Custom
{
    public class FlatButton : Button
    {
        public new static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(
                nameof(Padding),
                typeof(Thickness),
                typeof(BindableScrollView),
                new Thickness(25,0));

        public new Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public FlatButton()
        {
        }
    }
}
