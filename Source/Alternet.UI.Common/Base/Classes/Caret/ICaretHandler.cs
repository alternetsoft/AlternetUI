using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines properties which allows to control caret.
    /// </summary>
    public interface ICaretHandler : IDisposable
    {
        /// <summary>
        /// <inheritdoc cref="Caret.BlinkTime"/>
        /// </summary>
        int BlinkTime { get; set; }

        /// <summary>
        /// <inheritdoc cref="Caret.Size"/>
        /// </summary>
        SizeI Size { get; set; }

        /// <summary>
        /// <inheritdoc cref="Caret.Position"/>
        /// </summary>
        PointI Position { get; set; }

        /// <summary>
        /// <inheritdoc cref="Caret.IsOk"/>
        /// </summary>
        bool IsOk { get; }

        /// <summary>
        /// <inheritdoc cref="Caret.Visible"/>
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Control to which this caret is attached.
        /// </summary>
        AbstractControl? Control { get; }
    }
}
