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
        public static readonly IReadOnlyFontAndColor Empty = new FontAndColor(Color.Empty, Color.Empty);

        /// <summary>
        /// Gets <see cref="IReadOnlyFontAndColor"/> with all properties set to null.
        /// </summary>
        public static readonly IReadOnlyFontAndColor Null = new FontAndColor();

        private Color? backgroundColor;
        private Color? foregroundColor;
        private Font? font;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontAndColor"/> class.
        /// </summary>
        /// <param name="foregroundColor">Default value of the
        /// <see cref="ForegroundColor"/> property.</param>
        /// <param name="backgroundColor">Default value of the
        /// <see cref="BackgroundColor"/> property.</param>
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
        public static IReadOnlyFontAndColor SystemColorMenu =>
            new FontAndColor(SystemColors.MenuText, SystemColors.Menu);

        /// <summary>
        /// Has <see cref="SystemColors.ActiveCaption"/> for the background color and
        /// <see cref="SystemColors.ActiveCaptionText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorActiveCaption =>
            new FontAndColor(SystemColors.ActiveCaptionText, SystemColors.ActiveCaption);

        /// <summary>
        /// Has <see cref="SystemColors.InactiveCaption"/> for the background color and
        /// <see cref="SystemColors.InactiveCaptionText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorInactiveCaption =>
            new FontAndColor(SystemColors.InactiveCaptionText, SystemColors.InactiveCaption);

        /// <summary>
        /// Has <see cref="SystemColors.Info"/> for the background color and
        /// <see cref="SystemColors.InfoText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorInfo =>
            new FontAndColor(SystemColors.InfoText, SystemColors.Info);

        /// <summary>
        /// Has <see cref="SystemColors.Window"/> for the background color and
        /// <see cref="SystemColors.WindowText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorWindow =>
            new FontAndColor(SystemColors.WindowText, SystemColors.Window);

        /// <summary>
        /// Has <see cref="SystemColors.Highlight"/> for the background color and
        /// <see cref="SystemColors.HighlightText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorHighlight =>
            new FontAndColor(SystemColors.HighlightText, SystemColors.Highlight);

        /// <summary>
        /// Has <see cref="SystemColors.ButtonFace"/> for the background color and
        /// <see cref="SystemColors.ControlText"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor SystemColorButtonFace =>
            new FontAndColor(SystemColors.ControlText, SystemColors.ButtonFace);

        /// <summary>
        /// <inheritdoc cref="IFontAndColor.BackgroundColor"/>
        /// </summary>
        public virtual Color? BackgroundColor
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
        public virtual Color? ForegroundColor
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
            Color? value,
            bool isBackground,
            Action? action = null)
        {
            if (value is null && colors is null)
                return;
            Color? oldColor = isBackground ? colors?.BackgroundColor : colors?.ForegroundColor;
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
        public IReadOnlyFontAndColor WithForeColor(Color? color)
        {
            return new FontAndColor(color, BackgroundColor, Font);
        }

        /// <inheritdoc/>
        public IReadOnlyFontAndColor WithBackColor(Color? color)
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
            public override Color? BackgroundColor => control.GetDefaultAttributesBgColor();

            /// <inheritdoc/>
            public override Color? ForegroundColor => control.GetDefaultAttributesFgColor();

            /// <inheritdoc/>
            public override Font? Font => control.GetDefaultAttributesFont();
        }

        /// <summary>
        /// Allows to get font and color default for the specified <see cref="ControlTypeId"/>.
        /// </summary>
        public class ControlStaticDefaultFontAndColor : FontAndColor, IReadOnlyFontAndColor
        {
            private readonly ControlTypeId controlType;
            private readonly ControlRenderSizeVariant renderSize;

            /// <summary>
            /// Initializes a new instance of the <see cref="ControlStaticDefaultFontAndColor"/> class.
            /// </summary>
            /// <param name="controlType">Type of the control.</param>
            /// <param name="renderSize">Render size. Optional.</param>
            public ControlStaticDefaultFontAndColor(
                ControlTypeId controlType,
                ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
            {
                this.controlType = controlType;
                this.renderSize = renderSize;
            }

            /// <inheritdoc/>
            public override Color? BackgroundColor =>
                AbstractControl.GetClassDefaultAttributesBgColor(controlType, renderSize);

            /// <inheritdoc/>
            public override Color? ForegroundColor =>
                AbstractControl.GetClassDefaultAttributesFgColor(controlType, renderSize);

            /// <inheritdoc/>
            public override Font? Font =>
                AbstractControl.GetClassDefaultAttributesFont(controlType, renderSize);
        }
    }
}
