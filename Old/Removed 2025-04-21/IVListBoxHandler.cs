using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with virtual list box control.
    /// </summary>
    public interface IVListBoxHandler
    {
        /// <summary>
        /// Sets items count.
        /// </summary>
        int ItemsCount { set; }

        /// <inheritdoc cref="VirtualListBox.ScrollToRow(int)"/>
        bool ScrollToRow(int row);

        /// <inheritdoc cref="VirtualListBox.GetVisibleBegin"/>
        int GetVisibleBegin();
    }
}
