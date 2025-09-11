using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the kind of change that can occur in a <see cref="MenuItem"/>, <see cref="Menu"/>
    /// or <see cref="MainMenu"/>.
    /// </summary>
    public enum MenuChangeKind
    {
        /// <summary>
        /// No change.
        /// </summary>
        None,

        /// <summary>
        /// The <see cref="MenuItem.Text"/> property has changed.
        /// </summary>
        Text,

        /// <summary>
        /// The <see cref="MenuItem.Enabled"/> property has changed.
        /// </summary>
        Enabled,

        /// <summary>
        /// The <see cref="MenuItem.Visible"/> property has changed.
        /// </summary>
        Visible,

        /// <summary>
        /// The <see cref="MenuItem.Image"/> property has changed.
        /// </summary>
        Image,

        /// <summary>
        /// The <see cref="MenuItem.DisabledImage"/> property has changed.
        /// </summary>
        DisabledImage,

        /// <summary>
        /// The <see cref="MenuItem.Shortcut"/>
        /// or <see cref="MenuItem.ShortcutKeys"/> property has changed.
        /// </summary>
        Shortcut,

        /// <summary>
        /// The <see cref="MenuItem.Role"/> property has changed.
        /// </summary>
        Role,

        /// <summary>
        /// The <see cref="MenuItem.Checked"/> property has changed.
        /// </summary>
        Checked,

        /// <summary>
        /// The <see cref="MenuItem.ClickAction"/> property has changed.
        /// </summary>
        ClickAction,

        /// <summary>
        /// The command source or its state has changed.
        /// </summary>
        CommandSource,

        /// <summary>
        /// Child item has been inserted.
        /// </summary>
        ItemInserted,

        /// <summary>
        /// Child item has been removed.
        /// </summary>
        ItemRemoved,

        /// <summary>
        /// Some other property or state has changed.
        /// </summary>
        Other,

        /// <summary>
        /// This value is typically used to indicate that a bulk update has occurred,
        /// and the specific changes are not being tracked individually.
        /// </summary>
        Any,

        /// <summary>
        /// The menu item or menu has been opened.
        /// </summary>
        Opened,

        /// <summary>
        /// The menu item or menu has been closed.
        /// </summary>
        Closed,

        /// <summary>
        /// The menu item has been highlighted or focused.
        /// </summary>
        Highlighted,
    }
}
