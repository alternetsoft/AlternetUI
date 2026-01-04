using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Maui
{
    /// <summary>
    /// Provides a menu factory implementation for use with .NET MAUI applications.
    /// </summary>
    /// <remarks>This class extends the base functionality of <see cref="Alternet.UI.InnerMenuFactory"/> to
    /// support menu creation and management within the .NET MAUI framework. Typically, developers do not need to
    /// instantiate this class directly; it is used internally by the UI framework to provide platform-specific menu
    /// behavior.</remarks>
    public partial class MauiMenuFactory : Alternet.UI.InnerMenuFactory
    {
        static MauiMenuFactory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MauiMenuFactory"/> class.
        /// </summary>
        public MauiMenuFactory()
        {
        }

        /// <inheritdoc/>
        public override void Show(
            UI.ContextMenu menu,
            UI.AbstractControl control,
            Drawing.PointD? position = null,
            Action? onClose = null)
        {
            var pos = CoercePosition(position, ref control);
            if (pos is null)
                return;

            menu?.ShowInsideControl(control, menu.RelatedControl, pos, onClose);
        }
    }
}
