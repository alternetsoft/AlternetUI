using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a list control item with notification capabilities.
    /// </summary>
    public partial class ListControlItemWithNotify : ListControlItem
    {
        /// <inheritdoc/>
        public override int? ImageIndex
        {
            get => base.ImageIndex;

            set
            {
                if (base.ImageIndex == value)
                    return;
                base.ImageIndex = value;
                RaisePropertyChanged(nameof(ImageIndex));
            }
        }

        /// <inheritdoc/>
        public override string Text
        {
            get => base.Text;

            set
            {
                if (base.Text == value)
                    return;
                base.Text = value;
                RaisePropertyChanged(nameof(Text));
            }
        }

        /// <inheritdoc/>
        public override string? DisplayText
        {
            get => base.DisplayText;

            set
            {
                if (base.DisplayText == value)
                    return;
                base.DisplayText = value;
                RaisePropertyChanged(nameof(DisplayText));
            }
        }
    }
}
