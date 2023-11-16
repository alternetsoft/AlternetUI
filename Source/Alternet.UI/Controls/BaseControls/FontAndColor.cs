using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Default <see cref="IFontAndColor"/> and <see cref="IReadOnlyFontAndColor"/>
    /// implementation. Also contains static properties to get system colors
    /// as <see cref="IReadOnlyFontAndColor"/>.
    /// </summary>
    public class FontAndColor : IFontAndColor
    {
        private Color? backgroundColor;
        private Color? foregroundColor;
        private Font? font;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontAndColor"/> class.
        /// </summary>
        /// <param name="foregroundColor">Default value of the <see cref="ForegroundColor"/> property.</param>
        /// <param name="backgroundColor">Default value of the <see cref="BackgroundColor"/> property.</param>
        /// <param name="font">Default value of the <see cref="Font"/> property.</param>
        public FontAndColor(Color? foregroundColor, Color? backgroundColor = null, Font? font = null)
        {
            this.foregroundColor = foregroundColor;
            this.backgroundColor = backgroundColor;
            this.font = font;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontAndColor"/> class.
        /// </summary>
        public FontAndColor()
        {
        }

        /// <summary>
        /// Has <see cref="SystemColors.Menu"/> for the background color and
        /// <see cref="SystemColors.MenuText"/> for the foreground color.
        /// </summary>
        public IReadOnlyFontAndColor SystemColorMenu =>
            new FontAndColor(SystemColors.MenuText, SystemColors.Menu);

        /// <summary>
        /// Has <see cref="SystemColors.ActiveCaption"/> for the background color and
        /// <see cref="SystemColors.ActiveCaptionText"/> for the foreground color.
        /// </summary>
        public IReadOnlyFontAndColor SystemColorActiveCaption =>
            new FontAndColor(SystemColors.ActiveCaptionText, SystemColors.ActiveCaption);

        /// <summary>
        /// Has <see cref="SystemColors.InactiveCaption"/> for the background color and
        /// <see cref="SystemColors.InactiveCaptionText"/> for the foreground color.
        /// </summary>
        public IReadOnlyFontAndColor SystemColorInactiveCaption =>
            new FontAndColor(SystemColors.InactiveCaptionText, SystemColors.InactiveCaption);

        /// <summary>
        /// Has <see cref="SystemColors.Info"/> for the background color and
        /// <see cref="SystemColors.InfoText"/> for the foreground color.
        /// </summary>
        public IReadOnlyFontAndColor SystemColorInfo =>
            new FontAndColor(SystemColors.InfoText, SystemColors.Info);

        /// <summary>
        /// Has <see cref="SystemColors.Window"/> for the background color and
        /// <see cref="SystemColors.WindowText"/> for the foreground color.
        /// </summary>
        public IReadOnlyFontAndColor SystemColorWindow =>
            new FontAndColor(SystemColors.WindowText, SystemColors.Window);

        /// <summary>
        /// Has <see cref="SystemColors.Highlight"/> for the background color and
        /// <see cref="SystemColors.HighlightText"/> for the foreground color.
        /// </summary>
        public IReadOnlyFontAndColor SystemColorHighlight =>
            new FontAndColor(SystemColors.HighlightText, SystemColors.Highlight);

        /// <summary>
        /// Has <see cref="SystemColors.ButtonFace"/> for the background color and
        /// <see cref="SystemColors.ControlText"/> for the foreground color.
        /// </summary>
        public IReadOnlyFontAndColor SystemColorButtonFace =>
            new FontAndColor(SystemColors.ControlText, SystemColors.ButtonFace);

        /// <summary>
        /// <inheritdoc cref="IFontAndColor.BackgroundColor"/>
        /// </summary>
        public Color? BackgroundColor
        {
            get
            {
                return backgroundColor;
            }

            set
            {
                backgroundColor = value;
            }
        }

        /// <summary>
        /// <inheritdoc cref="IFontAndColor.ForegroundColor"/>
        /// </summary>
        public Color? ForegroundColor
        {
            get
            {
                return foregroundColor;
            }

            set
            {
                foregroundColor = value;
            }
        }

        /// <summary>
        /// <inheritdoc cref="IFontAndColor.Font"/>
        /// </summary>
        public Font? Font
        {
            get
            {
                return font;
            }

            set
            {
                font = value;
            }
        }

        internal class ControlStaticDefaultFontAndColor : IReadOnlyFontAndColor
        {
            private readonly ControlId controlType;
            private readonly ControlRenderSizeVariant renderSize;

            public ControlStaticDefaultFontAndColor(
                ControlId controlType,
                ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
            {
                this.controlType = controlType;
                this.renderSize = renderSize;
            }

            public Color? BackgroundColor =>
                Control.GetClassDefaultAttributesBgColor(controlType, renderSize);

            public Color? ForegroundColor =>
                Control.GetClassDefaultAttributesFgColor(controlType, renderSize);

            public Font? Font =>
                Control.GetClassDefaultAttributesFont(controlType, renderSize);
        }

        internal class ControlDefaultFontAndColor : IReadOnlyFontAndColor
        {
            private readonly Control control;

            public ControlDefaultFontAndColor(Control control)
            {
                this.control = control;
            }

            public Color? BackgroundColor => control.GetDefaultAttributesBgColor();

            public Color? ForegroundColor => control.GetDefaultAttributesFgColor();

            public Font? Font => control.GetDefaultAttributesFont();
        }
    }
}
