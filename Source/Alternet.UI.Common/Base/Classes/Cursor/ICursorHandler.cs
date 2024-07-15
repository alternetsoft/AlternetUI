using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to control cursor behavior.
    /// </summary>
    public interface ICursorHandler : IDisposable
    {
        /// <summary>
        /// Gets whether this object is ok.
        /// </summary>
        bool IsOk { get; }

        /// <summary>
        /// Gets the coordinates of the cursor hot spot.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The hot spot is the point at which the mouse is actually considered to be when
        /// this cursor is used. This method is currently only implemented on Windows and Linux
        /// and simply returns (-1, -1) in the other ports.
        /// </remarks>
        PointI GetHotSpot();
    }
}