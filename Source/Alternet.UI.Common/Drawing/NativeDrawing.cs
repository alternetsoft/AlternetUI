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
    public class NativeDrawing : BaseObject
    {
        /// <summary>
        /// Gets default native drawing implementation.
        /// </summary>
        public static NativeDrawing Default = new();

        /// <summary>
        /// Creates native pen.
        /// </summary>
        /// <returns></returns>
        public virtual object CreatePen() => throw new NotImplementedException();

        /// <summary>
        /// Creates native transparent brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateTransparentBrush() => throw new NotImplementedException();

        /// <summary>
        /// Creates native hatch brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateHatchBrush() => throw new NotImplementedException();

        /// <summary>
        /// Creates native linear gradient brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateLinearGradientBrush() => throw new NotImplementedException();

        /// <summary>
        /// Creates native radial gradient brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateRadialGradientBrush() => throw new NotImplementedException();

        /// <summary>
        /// Creates native solid brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateSolidBrush() => throw new NotImplementedException();

        /// <summary>
        /// Creates native texture brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateTextureBrush() => throw new NotImplementedException();
    }
}
