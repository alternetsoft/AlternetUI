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
        /// Gets or sets native <see cref="IControlPainterHandler"/> which is used
        /// for the cases when there is not platform independent implementation.
        /// </summary>
        public static IControlPainterHandler? NativeHandler;

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
        /// Gets or sets the SVG image used when the radio button is unchecked.
        /// </summary>
        public static SvgImage? RadioButtonImageUnchecked;

        /// <summary>
        /// Gets or sets the SVG image used when the radio button is checked.
        /// </summary>
        public static SvgImage? RadioButtonImageChecked;

        /// <summary>
        /// Gets or sets the SVG image used when the radio button is in an indeterminate state.
        /// </summary>
        public static SvgImage? RadioButtonImageIndeterminate;

        /// <summary>
        /// Gets or sets the colors associated with different visual states of a checkbox.
        /// </summary>
        public static EnumArray<VisualControlState, Color?> CheckBoxStateColors = new();

        /// <summary>
        /// Represents a delegate that returns a <see cref="SvgImage"/>
        /// based on a specified <see cref="CheckState"/>.
        /// </summary>
        /// <param name="checkState">
        /// The current <see cref="CheckState"/> of the checkbox, which
        /// can be <c>Unchecked</c>, <c>Checked</c>, or <c>Indeterminate</c>.
        /// </param>
        /// <returns>
        /// An <see cref="SvgImage"/> instance that visually represents the given check state.
        /// </returns>
        public delegate SvgImage GetCheckBoxSvgDelegate(CheckState checkState);

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
        /// Gets the SVG image corresponding to the specified radio button state.
        /// </summary>
        /// <param name="state">The state of the radio button (unchecked, checked,
        /// or indeterminate).</param>
        /// <returns>The SVG image representing the radio button state.</returns>
        public static SvgImage GetRadioButtonSvg(CheckState state)
        {
            SvgImage svgImage;

            switch (state)
            {
                case CheckState.Unchecked:
                default:
                    svgImage = RadioButtonImageUnchecked ?? KnownSvgImages.ImgCircle;
                    break;
                case CheckState.Checked:
                    svgImage = RadioButtonImageChecked ?? KnownSvgImages.ImgCircleDot;
                    break;
                case CheckState.Indeterminate:
                    svgImage = RadioButtonImageIndeterminate ?? KnownSvgImages.ImgCircleDotFilled;
                    break;
            }

            return svgImage;
        }

        /// <summary>
        /// Draws a radio button using an SVG-based visual representation within
        /// the specified bounds and state.
        /// </summary>
        /// <param name="control">
        /// The <see cref="AbstractControl"/> instance representing where
        /// the radio button will be drawn. It used in order to get scale factor and dark mode
        /// settings.
        /// </param>
        /// <param name="canvas">
        /// The <see cref="Graphics"/> surface onto which the radio button
        /// will be rendered.
        /// </param>
        /// <param name="rect">
        /// The <see cref="RectD"/> defining the layout bounds of the radio button.
        /// </param>
        /// <param name="isChecked">
        /// Specifies whether the radio button is currently in a checked state.
        /// </param>
        /// <param name="controlState">
        /// The <see cref="VisualControlState"/> representing the current interaction state
        /// (e.g., normal, hovered, disabled) used for rendering.
        /// </param>
        public static void DrawRadioButtonSvg(
            AbstractControl control,
            Graphics canvas,
            RectD rect,
            bool isChecked,
            VisualControlState controlState)
        {
            DrawCheckBoxSvg(
                control,
                canvas,
                rect,
                isChecked ? CheckState.Checked : CheckState.Unchecked,
                controlState,
                GetRadioButtonSvg);
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
        /// <param name="getSvg">The optional delegate used to get svg images.</param>
        public static void DrawCheckBoxSvg(
            AbstractControl control,
            Graphics canvas,
            RectD rect,
            CheckState checkState,
            VisualControlState controlState,
            GetCheckBoxSvgDelegate? getSvg = null)
        {
            getSvg ??= GetCheckBoxSvg;
            var svg = getSvg(checkState);
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
                        color = DefaultColors.DefaultCheckBoxColor.LightOrDark(isDark: true);
                        image = svg.ImageWithColor(size, color);
                        break;
                    default:
                        if (checkState == CheckState.Unchecked)
                        {
                            color = DefaultColors.GetBorderColor(control.IsDarkBackground);
                        }
                        else
                        {
                            color = DefaultColors.DefaultCheckBoxColor
                                .LightOrDark(control.IsDarkBackground);
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

        /// <inheritdoc/>
        public void DrawPushButton(
            AbstractControl control,
            Graphics canvas,
            RectD rect,
            VisualControlState controlState)
        {
            NativeHandler?.DrawPushButton(control, canvas, rect, controlState);
        }

        /// <inheritdoc/>
        public void DrawRadioButton(
            AbstractControl control,
            Graphics canvas,
            RectD rect,
            bool isChecked,
            VisualControlState controlState)
        {
            DrawRadioButtonSvg(
                control,
                canvas,
                rect,
                isChecked,
                controlState);
        }
    }
}
