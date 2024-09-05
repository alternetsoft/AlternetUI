using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Localization
{
    /// <summary>
    /// Defines localizations for property names.
    /// </summary>
    public class PropNameStrings
    {
        /// <summary>
        /// Current localizations for property names.
        /// </summary>
        public static PropNameStrings Default { get; set; } = new();

        /// <summary>
        /// Gets or sets localized property name.
        /// </summary>
        public string Left { get; set; } = "Left";

        /// <inheritdoc cref="Left"/>
        public string Top { get; set; } = "Top";

        /// <inheritdoc cref="Left"/>
        public string Right { get; set; } = "Right";

        /// <inheritdoc cref="Left"/>
        public string Bottom { get; set; } = "Bottom";

        /// <inheritdoc cref="Left"/>
        public string Width { get; set; } = "Width";

        /// <inheritdoc cref="Left"/>
        public string Height { get; set; } = "Height";

        /// <inheritdoc cref="Left"/>
        public string Center { get; set; } = "Center";

        /// <inheritdoc cref="Left"/>
        public string X { get; set; } = "X";

        /// <inheritdoc cref="Left"/>
        public string Y { get; set; } = "Y";

        /// <summary>
        /// Contains localizations for <see cref="Control"/> property names.
        /// </summary>
        public static class ControlProperties
        {
            /// <summary>
            /// Get or sets default localization for the corresponding property
            /// of the <see cref="Control"/>.
            /// </summary>
            public static string? Layout;

            /// <see cref="Layout"/>
            public static string? Title;

            /// <see cref="Layout"/>
            public static string? Dock;

            /// <see cref="Layout"/>
            public static string? Text;

            /// <see cref="Layout"/>
            public static string? ToolTip;

            /// <see cref="Layout"/>
            public static string? Left;

            /// <see cref="Layout"/>
            public static string? Top;

            /// <see cref="Layout"/>
            public static string? Visible;

            /// <see cref="Layout"/>
            public static string? Enabled;

            /// <see cref="Layout"/>
            public static string? Width;

            /// <see cref="Layout"/>
            public static string? Height;

            /// <see cref="Layout"/>
            public static string? SuggestedWidth;

            /// <see cref="Layout"/>
            public static string? SuggestedHeight;

            /// <see cref="Layout"/>
            public static string? MinChildMargin;

            /// <see cref="Layout"/>
            public static string? Margin;

            /// <see cref="Layout"/>
            public static string? Padding;

            /// <see cref="Layout"/>
            public static string? MinWidth;

            /// <see cref="Layout"/>
            public static string? MinHeight;

            /// <see cref="Layout"/>
            public static string? MaxWidth;

            /// <see cref="Layout"/>
            public static string? MaxHeight;

            /// <see cref="Layout"/>
            public static string? BackgroundColor;

            /// <see cref="Layout"/>
            public static string? ParentBackColor;

            /// <see cref="Layout"/>
            public static string? ParentForeColor;

            /// <see cref="Layout"/>
            public static string? ParentFont;

            /// <see cref="Layout"/>
            public static string? ForegroundColor;

            /// <see cref="Layout"/>
            public static string? Font;

            /// <see cref="Layout"/>
            public static string? IsBold;

            /// <see cref="Layout"/>
            public static string? VerticalAlignment;

            /// <see cref="Layout"/>
            public static string? HorizontalAlignment;
        }
    }
}