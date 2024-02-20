using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class TabControl
    {
        /// <inheritdoc/>
        [Browsable(false)]
        public override Thickness Padding
        {
            get => base.Padding;
            set
            {
            }
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set
            {
            }
        }

        [Browsable(false)]
        internal new string? ToolTip
        {
            get => base.ToolTip;
            set
            {
            }
        }

        [Browsable(false)]
        internal new bool IsBold
        {
            get => base.IsBold;
            set
            {
            }
        }

        [Browsable(false)]
        internal new Font? Font
        {
            get => base.Font;
            set
            {
            }
        }

        [Browsable(false)]
        internal new Color? ForegroundColor
        {
            get => base.ForegroundColor;
            set
            {
            }
        }
    }
}
