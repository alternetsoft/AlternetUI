namespace Alternet.UI
{
    public partial class Button
    {
        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
        }

        [Browsable(false)]
        internal new Thickness Padding
        {
            get => base.Padding;
        }
    }
}
