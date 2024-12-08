using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with displays.
    /// </summary>
    public interface IDisplayFactoryHandler : IDisposableObject
    {
        /// <inheritdoc cref="Display.GetFromControl(AbstractControl)"/>
        int GetFromControl(AbstractControl control);

        /// <summary>
        /// Creates <see cref="IDisplayHandler"/> interface provider for the default display.
        /// </summary>
        /// <returns></returns>
        IDisplayHandler CreateDisplay();

        /// <summary>
        /// Creates <see cref="IDisplayHandler"/> interface provider for the display with the
        /// specified index.
        /// </summary>
        /// <returns></returns>
        IDisplayHandler CreateDisplay(int index);

        /// <inheritdoc cref="Display.Count"/>
        int GetCount();

        /// <inheritdoc cref="Display.BaseDPI"/>
        SizeI GetDefaultDPI();

        /// <inheritdoc cref="Display.GetFromPoint(PointI)"/>
        int GetFromPoint(PointI pt);
    }
}
