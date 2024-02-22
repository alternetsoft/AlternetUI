namespace Alternet.UI
{
    public partial class GenericToolBarSet
    {
        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        [Browsable(false)]
        internal new bool IsBold
        {
            get => base.IsBold;
            set => base.IsBold = value;
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }
    }
}
