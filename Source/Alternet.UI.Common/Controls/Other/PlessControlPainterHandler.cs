using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides methods to paint and measure control parts such as checkboxes,
    /// expanders, and header buttons.
    /// </summary>
    public class PlessControlPainterHandler : DisposableObject, IControlPainterHandler
    {
        /// <summary>
        /// Gets or sets the default color of a checkbox in its normal state.
        /// </summary>
        public static LightDarkColor DefaultCheckBoxColor
            = new(light: (0, 103, 192) /*(0, 95, 184)*/, dark: new(76, 194, 255));

        /// <summary>
        /// Gets or sets the default size of a check mark in device-independent units.
        /// </summary>
        public static SizeD DefaultCheckMarkSize = (20, 20);

        /// <summary>
        /// Gets or sets the default size of a checkbox in device-independent units.
        /// </summary>
        public static SizeD DefaultCheckBoxSize = (16, 16);

        /// <summary>
        /// Gets or sets the default size of an expander in device-independent units.
        /// </summary>
        public static SizeD DefaultExpanderSize = (9, 9);

        /// <summary>
        /// Gets or sets the default height of a header button in device-independent units.
        /// </summary>
        public static Coord DefaultHeaderButtonHeight = 23;

        /// <summary>
        /// Gets or sets the default margin of a
        /// header button's label in device-independent units.
        /// </summary>
        public static Coord DefaultHeaderButtonMargin = 6;

        /// <summary>
        /// Gets or sets the default size of a collapse button in device-independent units.
        /// </summary>
        public static SizeD DefaultCollapseButtonSize = (19, 21);

        /// <summary>
        /// Gets or sets the SVG image used when the checkbox is unchecked.
        /// </summary>
        public static SvgImage? CheckBoxImageUnchecked;

        /// <summary>
        /// Gets or sets the SVG image used when the checkbox is checked.
        /// </summary>
        public static SvgImage? CheckBoxImageChecked;

        /// <summary>
        /// Gets or sets the SVG image used when the checkbox is in an indeterminate state.
        /// </summary>
        public static SvgImage? CheckBoxImageIndeterminate;

        /// <summary>
        /// Gets or sets the colors associated with different visual states of a checkbox.
        /// </summary>
        public static EnumArray<VisualControlState, Color?> CheckBoxStateColors = new();

        /// <summary>
        /// Gets the SVG image corresponding to the specified checkbox state.
        /// </summary>
        /// <param name="checkState">The state of the checkbox (unchecked, checked,
        /// or indeterminate).</param>
        /// <returns>The SVG image representing the checkbox state.</returns>
        public static SvgImage GetCheckBoxSvg(CheckState checkState)
        {
            SvgImage svgImage;

            switch (checkState)
            {
                case CheckState.Unchecked:
                default:
                    svgImage = CheckBoxImageUnchecked ?? KnownSvgImages.ImgSquare;
                    break;
                case CheckState.Checked:
                    svgImage = CheckBoxImageChecked ?? KnownSvgImages.ImgSquareCheckFilled;
                    break;
                case CheckState.Indeterminate:
                    svgImage = CheckBoxImageIndeterminate ?? KnownSvgImages.ImgSquareMinusFilled;
                    break;
            }

            return svgImage;
        }

        /// <summary>
        /// Draws a checkbox using SVG images.
        /// </summary>
        /// <param name="control">The control associated with the checkbox.</param>
        /// <param name="canvas">The graphics context where the checkbox will be drawn.</param>
        /// <param name="rect">The rectangle defining the bounds of the checkbox.</param>
        /// <param name="checkState">The state of the checkbox (checked, unchecked,
        /// or indeterminate).</param>
        /// <param name="controlState">The visual state of the control
        /// (e.g., normal, hovered, pressed).</param>
        public static void DrawCheckBoxSvg(
            AbstractControl control,
            Graphics canvas,
            RectD rect,
            CheckState checkState,
            VisualControlState controlState)
        {
            var svg = GetCheckBoxSvg(checkState);
            var size = control.PixelFromDip(rect.Size.Width);

            Image? image;

            var color = CheckBoxStateColors[controlState];

            if(color is null)
            {
                switch (controlState)
                {
                    case VisualControlState.Disabled:
                        image = svg.AsDisabledImage(size, control.IsDarkBackground);
                        break;
                    case VisualControlState.Selected:
                        color = DefaultCheckBoxColor.LightOrDark(isDark: true);
                        image = svg.ImageWithColor(size, color);
                        break;
                    default:
                        if (checkState == CheckState.Unchecked)
                        {
                            color = DefaultColors.GetBorderColor(control.IsDarkBackground);
                        }
                        else
                        {
                            color = DefaultCheckBoxColor.LightOrDark(control.IsDarkBackground);
                        }

                        image = svg.ImageWithColor(size, color);
                        break;
                }
            }
            else
            {
                image = svg.ImageWithColor(size, color);
            }

            if (image is not null)
            {
                canvas.DrawImage(image, rect.Location);
                return;
            }
        }

        /// <inheritdoc/>
        public virtual void DrawCheckBox(
            AbstractControl control,
            Graphics canvas,
            RectD rect,
            CheckState checkState,
            VisualControlState controlState)
        {
            DrawCheckBoxSvg(control, canvas, rect, checkState, controlState);
        }

        /// <inheritdoc/>
        public SizeD GetCheckBoxSize(
            AbstractControl control,
            CheckState checkState = CheckState.Unchecked,
            VisualControlState controlState = VisualControlState.Normal)
        {
            return DefaultCheckBoxSize;
        }

        /// <inheritdoc/>
        public virtual SizeD GetCheckMarkSize(AbstractControl control)
        {
            return DefaultCheckMarkSize;
        }

        /// <inheritdoc/>
        public virtual SizeD GetCollapseButtonSize(AbstractControl control, Graphics dc)
        {
            return DefaultCollapseButtonSize;
        }

        /// <inheritdoc/>
        public virtual SizeD GetExpanderSize(AbstractControl control)
        {
            return DefaultExpanderSize;
        }

        /// <inheritdoc/>
        public virtual Coord GetHeaderButtonHeight(AbstractControl control)
        {
            return DefaultHeaderButtonHeight;
        }

        /// <inheritdoc/>
        public virtual Coord GetHeaderButtonMargin(AbstractControl control)
        {
            return DefaultHeaderButtonMargin;
        }
    }
}
