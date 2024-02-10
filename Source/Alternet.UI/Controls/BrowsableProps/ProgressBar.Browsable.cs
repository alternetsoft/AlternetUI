namespace Alternet.UI
{
    public partial class ProgressBar
    {
        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }
    }
}
