namespace Alternet.UI
{
    public partial class FindReplaceControl
    {
        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }
    }
}
