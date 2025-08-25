using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Handles the display of context menus for the control in a Maui application.
    /// </summary>
    /// <remarks>This class extends <see cref="PlessContextMenuHandler"/> to provide
    /// platform-specific functionality for showing context menus.
    /// It overrides the <see cref="Show"/> method to customize the behavior
    /// for Maui applications.</remarks>
    public partial class MauiContextMenuHandler : PlessContextMenuHandler
    {
        /// <inheritdoc/>
        public override void Show(AbstractControl container, PointD? position = null)
        {
            Control?.ShowInsideControl(container, position);
        }
    }
}
