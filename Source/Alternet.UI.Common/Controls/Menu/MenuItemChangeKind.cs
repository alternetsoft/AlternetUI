using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the kind of change that can occur in a <see cref="MenuItem"/>.
    /// </summary>
    public enum MenuItemChangeKind
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
        /// Some other property or state has changed.
        /// </summary>
        Other,
    }
}
