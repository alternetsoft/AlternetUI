using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines abstract methods related to native drawing.
    /// </summary>
    /// <remarks>
    /// Do not use <see cref="Default"/> property until native drawing
    /// is initialized.
    /// </remarks>
    public abstract partial class NativeDrawing : BaseObject
    {
        /// <summary>
        /// Gets default native drawing implementation.
        /// </summary>
        public static NativeDrawing Default = new NotImplementedDrawing();

        public abstract Graphics CreateGraphicsFromScreen();

        public abstract Graphics CreateGraphicsFromImage(Image image);

        /// <summary>
        /// Gets a standard system color.
        /// </summary>
        /// <param name="index">System color identifier.</param>
        public abstract Color GetColor(KnownSystemColor index);
    }
}
