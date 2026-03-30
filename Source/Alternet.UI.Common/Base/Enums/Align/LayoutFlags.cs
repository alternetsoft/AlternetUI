using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies layout customization flags for controls and other elements.
    /// These flags can be used to modify the behavior of layout processes, such as how child controls are arranged within a container
    /// or how docked controls are layered. The <see cref="LayoutFlags"/> enumeration provides options for customizing layout behavior,
    /// allowing developers to control aspects of the layout process to achieve desired visual arrangements and interactions within the user interface.
    /// </summary>
    [Flags]
    public enum LayoutFlags
    {
        /// <summary>
        /// Specifies that no layout customization flags are set. This is the default value, indicating that the control should use
        /// the standard layout behavior without any special modifications.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies to use the backward direction in the child collection iterator
        /// when layout is performed. Currently it is used only when docked controls are laid out. This flag is used to change
        /// the visual stacking of docked controls. When this flag is set, the layout process iterates through the child
        /// controls in reverse order, which can affect how docked controls are layered and displayed within their container.
        /// </summary>
        IterateBackward = 1,

        /// <summary>
        /// Specifies that the layout process should consider the margins of child controls when arranging them within a container.
        /// When this flag is set, the layout process takes into account the margins of each child control, which can affect
        /// the positioning and spacing of controls within the container. By default margins
        /// are not considered when docking controls with <see cref="DockStyle.Top"/>, <see cref="DockStyle.Bottom"/>,
        /// <see cref="DockStyle.Left"/>, or <see cref="DockStyle.Right"/>.
        /// </summary>
        UseMarginsWhenDock = 2,
    }
}
