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
    public partial class FontAndColor : IFontAndColor
    {
        /// <summary>
        /// Gets <see cref="IReadOnlyFontAndColor"/> instance with
        /// <see cref="Color.Empty"/> colors and null font.
        /// </summary>
        public static readonly IReadOnlyFontAndColor Empty
            = new FontAndColor(ExactColors.Empty, ExactColors.Empty);

        /// <summary>
        /// Gets <see cref="IReadOnlyFontAndColor"/> with all properties set to null.
        /// </summary>
        public static readonly IReadOnlyFontAndColor Null = new FontAndColor();

        private LightDarkColor? backgroundColor;
        private LightDarkColor? foregroundColor;
        private Font? font;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontAndColor"/> class.
        /// </summary>
        /// <param name="foregroundColor">Default value of the
        /// <see cref="ForegroundColor"/> property.</param>
        /// <param name="backgroundColor">Default value of the
        /// <see cref="BackgroundColor"/> property.</param>
        /// <param name="font">Default value of the <see cref="Font"/> property.</param>
        public FontAndColor(LightDarkColor? foregroundColor, LightDarkColor? backgroundColor = null, Font? font = null)
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
        /// Has <see cref="LightDarkColors.Menu"/> for the background color and
        /// <see cref="LightDarkColors.MenuText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorMenu =>
            new FontAndColor(LightDarkColors.MenuText, LightDarkColors.Menu);

        /// <summary>
        /// Has <see cref="LightDarkColors.ActiveCaption"/> for the background color and
        /// <see cref="LightDarkColors.ActiveCaptionText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorActiveCaption =>
            new FontAndColor(LightDarkColors.ActiveCaptionText, LightDarkColors.ActiveCaption);

        /// <summary>
        /// Has <see cref="LightDarkColors.InactiveCaption"/> for the background color and
        /// <see cref="LightDarkColors.InactiveCaptionText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorInactiveCaption =>
            new FontAndColor(LightDarkColors.InactiveCaptionText, LightDarkColors.InactiveCaption);

        /// <summary>
        /// Has <see cref="LightDarkColors.Info"/> for the background color and
        /// <see cref="LightDarkColors.InfoText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorInfo =>
            new FontAndColor(LightDarkColors.InfoText, LightDarkColors.Info);

        /// <summary>
        /// Has <see cref="LightDarkColors.Window"/> for the background color and
        /// <see cref="LightDarkColors.WindowText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorWindow =>
            new FontAndColor(LightDarkColors.WindowText, LightDarkColors.Window);

        /// <summary>
        /// Has <see cref="LightDarkColors.Highlight"/> for the background color and
        /// <see cref="LightDarkColors.HighlightText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorHighlight =>
            new FontAndColor(LightDarkColors.HighlightText, LightDarkColors.Highlight);

        /// <summary>
        /// Has <see cref="LightDarkColors.ButtonFace"/> for the background color and
        /// <see cref="LightDarkColors.ControlText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorButtonFace =>
            new FontAndColor(LightDarkColors.ControlText, LightDarkColors.ButtonFace);

        /// <summary>
        /// <inheritdoc cref="IFontAndColor.BackgroundColor"/>
        /// </summary>
        public virtual LightDarkColor? BackgroundColor
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
        public virtual LightDarkColor? ForegroundColor
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
        public virtual Font? Font
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

        /// <summary>
        /// Creates new <see cref="IReadOnlyFontAndColor"/> instance and assigns it to
        /// <paramref name="colors"/> with the modified background or foreground color value.
        /// </summary>
        /// <param name="colors">Colors to change.</param>
        /// <param name="value">New color value.</param>
        /// <param name="isBackground">If <c>true</c>, background color is assigned; otherwise
        /// foreground color is assigned.</param>
        /// <param name="action">When color value is really changed, this action is called.</param>
        public static void ChangeColor(
            ref IReadOnlyFontAndColor? colors,
            LightDarkColor? value,
            bool isBackground,
            Action? action = null)
        {
            if (value is null && colors is null)
                return;
            LightDarkColor? oldColor = isBackground ? colors?.BackgroundColor : colors?.ForegroundColor;
            if (oldColor == value)
                return;
            var result = new FontAndColor(null, null, colors?.Font);

            if (isBackground)
            {
                result.BackgroundColor = value;
                result.ForegroundColor = colors?.ForegroundColor;
            }
            else
            {
                result.ForegroundColor = value;
                result.BackgroundColor = colors?.BackgroundColor;
            }

            colors = result;
            action?.Invoke();
        }

        /// <inheritdoc/>
        public IReadOnlyFontAndColor WithFont(Font? font)
        {
            return new FontAndColor(ForegroundColor, BackgroundColor, font);
        }

        /// <inheritdoc/>
        public IReadOnlyFontAndColor WithForeColor(LightDarkColor? color)
        {
            return new FontAndColor(color, BackgroundColor, Font);
        }

        /// <inheritdoc/>
        public IReadOnlyFontAndColor WithBackColor(LightDarkColor? color)
        {
            return new FontAndColor(ForegroundColor, color, Font);
        }

        /// <summary>
        /// Allows to get font and color defaults for the control.
        /// </summary>
        public class ControlDefaultFontAndColor : FontAndColor, IReadOnlyFontAndColor
        {
            private readonly IControl control;

            /// <summary>
            /// Initializes a new instance of the <see cref="ControlDefaultFontAndColor"/> class.
            /// </summary>
            /// <param name="control">Control for which font and color defaults are returned.</param>
            public ControlDefaultFontAndColor(IControl control)
            {
                this.control = control;
            }

            /// <inheritdoc/>
            public override LightDarkColor? BackgroundColor => control.GetDefaultAttributesBgColor();

            /// <inheritdoc/>
            public override LightDarkColor? ForegroundColor => control.GetDefaultAttributesFgColor();

            /// <inheritdoc/>
            public override Font? Font => Control.DefaultFont;
        }
    }
}
