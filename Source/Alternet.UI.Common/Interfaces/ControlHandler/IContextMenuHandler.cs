using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with context menu.
    /// </summary>
    public interface IContextMenuHandler
    {
        /// <summary>
        /// Show menu on screen.
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <param name="position">Position in local coordinates.</param>
        void Show(AbstractControl control, PointD? position = null);
    }
}
