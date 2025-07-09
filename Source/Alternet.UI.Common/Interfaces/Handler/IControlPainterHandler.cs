using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods which can paint and measure control parts. For example,
    /// <see cref="DrawCheckBox"/> method paints check box in different states.
    /// </summary>
    public interface IControlPainterHandler : IDisposable
    {
        /// <summary>
        /// Gets default checkbox size.
        /// </summary>
        /// <param name="control">Control where checkbox will be painted.</param>
        /// <param name="checkState">Check state.</param>
        /// <param name="controlState">Control part state.</param>
        /// <returns></returns>
        SizeD GetCheckBoxSize(
            AbstractControl control,
            CheckState checkState = CheckState.Unchecked,
            VisualControlState controlState = VisualControlState.Normal);

        /// <summary>
        /// Returns the default size of a check mark in dips.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <returns></returns>
        SizeD GetCheckMarkSize(AbstractControl control);

        /// <summary>
        /// Returns the default size of a expander in dips.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <returns></returns>
        SizeD GetExpanderSize(AbstractControl control);

        /// <summary>
        /// Returns the default height of a header button in dips, either a fixed platform
        /// height if available, or a generic height based on the window's font.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <returns></returns>
        Coord GetHeaderButtonHeight(AbstractControl control);

        /// <summary>
        /// Returns the margin on left and right sides of header button's label in dips.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <returns></returns>
        Coord GetHeaderButtonMargin(AbstractControl control);

        /// <summary>
        /// Returns the default size of a collapse button in dips.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <returns></returns>
        SizeD GetCollapseButtonSize(AbstractControl control, Graphics dc);

        /// <summary>
        /// Draws checkbox.
        /// </summary>
        /// <param name="control">Control where checkbox will be painted.</param>
        /// <param name="checkState">Check state.</param>
        /// <param name="controlState">Control part state.</param>
        /// <param name="canvas"><see cref="Graphics"/> used for painting the checkbox.</param>
        /// <param name="rect">Rectangle where checkbox is painted.</param>
        void DrawCheckBox(
            AbstractControl control,
            Graphics canvas,
            RectD rect,
            CheckState checkState,
            VisualControlState controlState);

        /// <summary>
        /// Draws a push button control within the specified bounds and visual state.
        /// </summary>
        /// <param name="control">The <see cref="AbstractControl"/> instance
        /// where button will be painted. Used for scale factor and dark mode determination.</param>
        /// <param name="canvas">The <see cref="Graphics"/> surface used for rendering.</param>
        /// <param name="rect">The bounding <see cref="RectD"/> that defines
        /// the button’s layout region.</param>
        /// <param name="controlState">The current <see cref="VisualControlState"/>
        /// used to render visual feedback (e.g., hover, pressed, disabled).</param>
        void DrawPushButton(
            AbstractControl control,
            Graphics canvas,
            RectD rect,
            VisualControlState controlState);

        /// <summary>
        /// Draws a radio button control within the specified bounds, reflecting
        /// its checked state and visual appearance.
        /// </summary>
        /// <param name="control">The <see cref="AbstractControl"/> instance
        /// where radio button will be painted. Used for scale factor and dark mode
        /// determination.</param>
        /// <param name="canvas">The <see cref="Graphics"/> surface used for rendering.</param>
        /// <param name="rect">The bounding <see cref="RectD"/> that
        /// defines the button’s layout region.</param>
        /// <param name="isChecked">Indicates whether the radio
        /// button is currently checked.</param>
        /// <param name="controlState">The current <see cref="VisualControlState"/>
        /// used to render visual feedback (e.g., selected, inactive).</param>
        void DrawRadioButton(
            AbstractControl control,
            Graphics canvas,
            RectD rect,
            bool isChecked,
            VisualControlState controlState);
    }
}
