using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="IDisplayHandler"/> interface provider which does nothing.
    /// </summary>
    public class PlessDisplayHandler : DisposableObject, IDisplayHandler
    {
        /// <inheritdoc/>
        public virtual bool IsOk => true;

        /// <inheritdoc/>
        public virtual RectI GetClientArea()
        {
            return (0, 0, 1920, 1024);
        }

        /// <inheritdoc/>
        public virtual RectI GetGeometry()
        {
            return (0, 0, 1920, 1024);
        }

        /// <inheritdoc/>
        public virtual string GetName()
        {
            return "Primary Display";
        }

        /// <inheritdoc/>
        public virtual Coord GetScaleFactor()
        {
            return 1;
        }

        /// <inheritdoc/>
        public virtual bool IsPrimary()
        {
            return true;
        }
    }
}
