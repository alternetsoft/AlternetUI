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
        /// Has <see cref="Color.Empty"/> for the background color and
        /// <see cref="Color.Empty"/> for the foreground color.
        /// </summary>
        public static IReadOnlyFontAndColor Empty => new FontAndColor(Color.Empty, Color.Empty);

        /// <summary>
        /// Has <c>null</c> for the background and foreground colors.
        /// </summary>
        public static IReadOnlyFontAndColor Null => new FontAndColor();

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

        /// <summary>
        /// Gets <see cref="IReadOnlyFontAndColor"/> instance for the specified
        /// <paramref name="method"/>.
        /// </summary>
        /// <param name="method">Type of the colors to get.</param>
        /// <param name="control">Control instance. Used when <paramref name="method"/>
        /// is <see cref="ResetColorType.DefaultAttributes"/>.</param>
        /// <param name="renderSize">Rendering size. Used when <paramref name="method"/>
        /// is <see cref="ResetColorType.DefaultAttributesTextBox"/> or other similar values.
        /// You can skip this parameter as on most os it is ignored.</param>
        /// <returns></returns>
        public static IReadOnlyFontAndColor GetResetColors(
            ResetColorType method,
            Control? control = null,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            switch (method)
            {
                case ResetColorType.Auto:
                    return Null;
                case ResetColorType.NullColor:
                    return Null;
                case ResetColorType.EmptyColor:
                    return Empty;
                case ResetColorType.DefaultAttributes:
                    return control?.GetDefaultFontAndColor() ?? Null;
                case ResetColorType.DefaultAttributesTextBox:
                    return Control.GetStaticDefaultFontAndColor(ControlId.TextBox, renderSize);
                case ResetColorType.DefaultAttributesListBox:
                    return Control.GetStaticDefaultFontAndColor(ControlId.ListBox, renderSize);
                case ResetColorType.DefaultAttributesButton:
                    return Control.GetStaticDefaultFontAndColor(ControlId.Button, renderSize);
                case ResetColorType.ColorMenu:
                    return SystemColorMenu;
                case ResetColorType.ColorActiveCaption:
                    return SystemColorActiveCaption;
                case ResetColorType.ColorInactiveCaption:
                    return SystemColorInactiveCaption;
                case ResetColorType.ColorInfo:
                    return SystemColorInfo;
                case ResetColorType.ColorWindow:
                    return SystemColorWindow;
                case ResetColorType.ColorHighlight:
                    return SystemColorHighlight;
                case ResetColorType.ColorButtonFace:
                    return SystemColorButtonFace;
                default:
                    return Null;
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
