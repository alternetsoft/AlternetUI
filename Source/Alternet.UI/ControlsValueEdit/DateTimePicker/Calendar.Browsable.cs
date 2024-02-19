using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Calendar
    {
        /// <inheritdoc/>
        [Browsable(false)]
        public override Color? BackgroundColor
        {
            get => base.BackgroundColor;
            set => base.BackgroundColor = value;
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override Color? ForegroundColor
        {
            get => base.ForegroundColor;
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override Font? Font
        {
            get => base.Font;
            set => base.Font = value;
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override bool IsBold
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

        [Browsable(false)]
        internal new Thickness Padding
        {
            get => base.Padding;
            set => base.Padding = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }
    }
}
