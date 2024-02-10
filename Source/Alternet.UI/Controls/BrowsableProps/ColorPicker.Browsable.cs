namespace Alternet.UI
{
    public partial class ColorPicker
    {
        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }
    }
}
