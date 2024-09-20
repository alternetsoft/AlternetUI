using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to get display related information.
    /// </summary>
    public interface IDisplayHandler : IDisposable
    {
        /// <inheritdoc cref="Display.IsOk"/>
        public bool IsOk { get; }

        /// <inheritdoc cref="Display.Name"/>
        public string GetName();

        /// <inheritdoc cref="Display.ScaleFactor"/>
        public Coord GetScaleFactor();

        /// <inheritdoc cref="Display.IsPrimary"/>
        public bool IsPrimary();

        /// <inheritdoc cref="Display.ClientArea"/>
        public RectI GetClientArea();

        /// <inheritdoc cref="Display.Geometry"/>
        public RectI GetGeometry();
    }
}
