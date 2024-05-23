using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Base class for the control painters.
    /// </summary>
    public abstract class CustomControlPainter : DisposableObject
    {
        private static CustomControlPainter? current;

        /// <summary>
        /// Gets or sets current control painter.
        /// </summary>
        public static CustomControlPainter Current
        {
            get => current ??= NativePlatform.Default.GetPainter();
            set => current = value;
        }

        /// <summary>
        /// Gets default checkbox size.
        /// </summary>
        /// <param name="control">Control where checkbox will be painted.</param>
        /// <param name="checkState">Check state.</param>
        /// <param name="controlState">Control part state.</param>
        /// <returns></returns>
        public abstract SizeD GetCheckBoxSize(
            Control control,
            CheckState checkState = CheckState.Unchecked,
            GenericControlState controlState = GenericControlState.Normal);

        /// <summary>
        /// Returns the default size of a check mark in dips.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <returns></returns>
        public abstract SizeD GetCheckMarkSize(Control control);

        /// <summary>
        /// Returns the default size of a expander in dips.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <returns></returns>
        public abstract SizeD GetExpanderSize(Control control);

        /// <summary>
        /// Returns the default height of a header button in dips, either a fixed platform
        /// height if available, or a generic height based on the window's font.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <returns></returns>
        public abstract double GetHeaderButtonHeight(Control control);

        /// <summary>
        /// Returns the margin on left and right sides of header button's label in dips.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <returns></returns>
        public abstract double GetHeaderButtonMargin(Control control);

        /// <summary>
        /// Returns the default size of a collapse button in dips.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <returns></returns>
        public abstract SizeD GetCollapseButtonSize(Control control, Graphics dc);

        /// <summary>
        /// Draws checkbox.
        /// </summary>
        /// <param name="control">Control where checkbox will be painted.</param>
        /// <param name="checkState">Check state.</param>
        /// <param name="controlState">Control part state.</param>
        /// <param name="canvas"><see cref="Graphics"/> used for painting the checkbox.</param>
        /// <param name="rect">Rectangle where checkbox is painted.</param>
        public abstract void DrawCheckBox(
            Control control,
            Graphics canvas,
            RectD rect,
            CheckState checkState,
            GenericControlState controlState);

        internal void LogPartSize(Control control)
        {
            Log($"CheckMarkSize: {GetCheckMarkSize(control)}");
            Log($"CheckBoxSize(0): {GetCheckBoxSize(control)}");
            /*Log($"CheckBoxSize(Cell): {GetCheckBoxSize(control, DrawFlags.Cell)}");*/
            Log($"GetExpanderSize: {GetExpanderSize(control)}");
            Log($"GetHeaderButtonHeight: {GetHeaderButtonHeight(control)}");
            Log($"GetHeaderButtonMargin: {GetHeaderButtonMargin(control)}");
        }
    }
}
