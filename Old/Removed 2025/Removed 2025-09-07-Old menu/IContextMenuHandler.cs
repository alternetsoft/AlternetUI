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
    public interface IContextMenuHandler : IDisposable
    {
        /// <summary>
        /// Show menu on screen.
        /// </summary>
        /// <param name="control">The target control.</param>
        /// <param name="position">The position in local coordinates.</param>
        /// <param name="onClose">The action to be invoked when the menu is closed.</param>
        void Show(
            AbstractControl control,
            PointD? position = null,
            Action? onClose = null);
    }
}
