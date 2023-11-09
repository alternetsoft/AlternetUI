using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains platform specific settings.
    /// </summary>
    public class PlatformDefaults
    {
        static PlatformDefaults()
        {
        }

        /// <summary>
        /// Gets or sets default background color of the <see cref="RichToolTip"/>.
        /// </summary>
        public Color? RichToolTipBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets default background end color of the <see cref="RichToolTip"/>.
        /// </summary>
        public Color? RichToolTipBackgroundColorEnd { get; set; }

        /// <summary>
        /// Gets or sets default foreground color of the <see cref="RichToolTip"/>.
        /// </summary>
        public Color? RichToolTipForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets default title foreground color of the <see cref="RichToolTip"/>.
        /// </summary>
        public Color? RichToolTipTitleForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets whether to adjust height of <see cref="TextBox"/> controls
        /// to height of the <see cref="ComboBox"/> control.
        /// </summary>
        /// <remarks>
        /// Used in <see cref="LayoutFactory.AdjustTextBoxesHeight"/>.
        /// </remarks>
        public bool AdjustTextBoxesHeight { get; set; } = false;

        /// <summary>
        /// Defines default vertical spacing of the <see cref="PropertyGrid"/>.
        /// </summary>
        /// <remarks>
        /// Used in <see cref="PropertyGrid.SetVerticalSpacing"/>.
        /// </remarks>
        public int PropertyGridVerticalSpacing { get; set; } =
            Application.IsWindowsOS ? 3 : 2;

        /// <summary>
        /// Returns default property values for all controls in the library.
        /// </summary>
        public AllControlDefaults Controls { get; } = new();
    }
}