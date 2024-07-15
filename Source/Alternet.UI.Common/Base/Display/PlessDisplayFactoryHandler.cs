using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements dummy <see cref="IDisplayFactoryHandler"/> provider.
    /// </summary>
    public abstract class PlessDisplayFactoryHandler : DisposableObject, IDisplayFactoryHandler
    {
        /// <inheritdoc/>
        public abstract IDisplayHandler CreateDisplay();

        /// <inheritdoc/>
        public virtual IDisplayHandler CreateDisplay(int index)
        {
            return CreateDisplay();
        }

        /// <inheritdoc/>
        public virtual int GetCount()
        {
            return 1;
        }

        /// <inheritdoc/>
        public virtual SizeI GetDefaultDPI()
        {
            if (App.IsIOS || App.IsMacOS)
                return 72;
            return 96;
        }

        /// <inheritdoc/>
        public virtual int GetFromControl(Control control)
        {
            return 0;
        }

        /// <inheritdoc/>
        public virtual int GetFromPoint(PointI pt)
        {
            return 0;
        }
    }
}
