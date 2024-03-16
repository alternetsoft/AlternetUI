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
        /// <summary>
        /// Gets or sets current control painter.
        /// See also <see cref="NativeControlPainter.Default"/>.
        /// </summary>
        public static CustomControlPainter Current;

        static CustomControlPainter()
        {
            NativeControlPainter.Default = new();
            Current = NativeControlPainter.Default;
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
            CheckState checkState,
            GenericControlState controlState);

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
    }
}
