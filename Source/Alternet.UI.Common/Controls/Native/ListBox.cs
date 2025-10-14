using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that displays a list of items and allows the user to select one or more items.
    /// This control encapsulates the native list box implemented by the operating system. It has limited functionality.
    /// For the full featured list control, use the <see cref="VirtualListBox"/> or <see cref="StdListBox"/> controls.
    /// </summary>
    /// <remarks>The <see cref="ListBox"/> control is commonly used to present a collection of items in a
    /// scrollable list.  It supports single or multiple selection modes, depending on the configuration.
    /// This control is a part of the user interface framework and inherits from <see cref="Control"/>.</remarks>
    public partial class ListBox : Control
    {
        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateListBoxHandler(this);
        }
    }
}
