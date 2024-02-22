using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class FindReplaceControl
    {
        [Browsable(false)]
        internal new Color? ForegroundColor
        {
            get => base.ForegroundColor;
            set => base.ForegroundColor = value;
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }
    }
}
