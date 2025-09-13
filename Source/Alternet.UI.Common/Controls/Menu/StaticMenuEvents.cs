using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a collection of static events and methods for managing and
    /// interacting with menu-related actions.
    /// </summary>
    /// <remarks>The <see cref="StaticMenuEvents"/> class defines a set of events
    /// that are triggered in
    /// response to various menu-related actions, such as opening, closing,
    /// or interacting with menu items. These events
    /// are primarily intended for use with menus and their associated menu items.
    /// Additionally, the class
    /// provides static methods to raise these events programmatically.
    /// <para> This class is useful for scenarios where
    /// centralized handling of menu events is required, such as in applications
    /// with dynamic or context-sensitive
    /// menus. </para></remarks>
    public static class StaticMenuEvents
    {
        /// <summary>
        /// Occurs when a new item is inserted into the items of <see cref="MenuItem"/>.
        /// </summary>
        /// <remarks>This event is triggered whenever an item is added.
        /// Subscribers can use
        /// this event to respond to changes in the menu's structure.</remarks>
        public static event EventHandler<MenuChangeEventArgs>? ItemInserted;

        /// <summary>
        /// Occurs when an item is removed from the items of <see cref="MenuItem"/>.
        /// </summary>
        /// <remarks>This event is triggered whenever an item is removed, providing details about the
        /// removed item through the <see cref="MenuChangeEventArgs"/> parameter.
        /// Subscribers can use this event to
        /// perform actions such as updating the UI or logging changes.</remarks>
        public static event EventHandler<MenuChangeEventArgs>? ItemRemoved;

        /// <summary>
        /// Occurs when an item is removed from the main menu.
        /// </summary>
        /// <remarks>This event is triggered whenever an item is removed, providing details about the
        /// removed item  through the <see cref="MenuChangeEventArgs"/> parameter.
        /// Subscribers can use this event to
        /// perform actions such as updating the UI or logging changes.</remarks>
        public static event EventHandler<MenuChangeEventArgs>? MainMenuItemRemoved;

        /// <summary>
        /// Occurs when an item is removed from the context menu.
        /// </summary>
        /// <remarks>This event is triggered whenever an item is removed, providing details about the
        /// removed item  through the <see cref="MenuChangeEventArgs"/> parameter.
        /// Subscribers can use this event to
        /// perform actions such as updating the UI or logging changes.</remarks>
        public static event EventHandler<MenuChangeEventArgs>? ContextMenuItemRemoved;

        /// <summary>
        /// Occurs when the menu is opening. This event is usually raised only for top level menus.
        /// You can also handle <see cref="MenuItem.Opened"/> to be notified when a menu item is opened.
        /// </summary>
        public static event CancelEventHandler? MenuOpening;

        /// <summary>
        /// Occurs when the menu is closing. This event is usually raised only for top level menus.
        /// </summary>
        public static event EventHandler? MenuClosing;

        /// <summary>
        /// Occurs when the value of the <see cref="MenuItem.Enabled"/> property changes.
        /// </summary>
        public static event EventHandler? ItemEnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MenuItem.Visible"/> property changes.
        /// </summary>
        public static event EventHandler? ItemVisibleChanged;

        /// <summary>
        /// Occurs when the menu item is clicked.
        /// </summary>
        public static event EventHandler? ItemClick;

        /// <summary>
        /// Occurs when the <see cref="MenuItem.Text" /> property value changes.
        /// </summary>
        public static event EventHandler? ItemTextChanged;

        /// <summary>
        /// Occurs when a change in the menu item is detected, providing details about the type of change.
        /// </summary>
        /// <remarks>This event is triggered whenever a change occurs, and
        /// it provides information about
        /// the change through the <see cref="BaseEventArgs{T}"/> parameter.
        /// The <see cref="MenuChangeKind"/> value
        /// indicates the specific type of change that occurred.</remarks>
        public static event EventHandler<BaseEventArgs<MenuChangeKind>>? ItemChanged;

        /// <summary>
        /// Occurs when menu item is opened.
        /// </summary>
        public static event EventHandler? ItemOpened;

        /// <summary>
        /// Occurs when menu item is closed.
        /// </summary>
        public static event EventHandler? ItemClosed;

        /// <summary>
        /// Occurs when menu item is highlighted.
        /// </summary>
        public static event EventHandler? ItemHighlighted;

        /// <summary>
        /// Occurs when the <see cref="MenuItem.Shortcut"/> property changes.
        /// </summary>
        public static event EventHandler? ItemShortcutChanged;

        /// <summary>
        /// Occurs when the <see cref="MenuItem.Checked"/> property changes.
        /// </summary>
        public static event EventHandler? ItemCheckedChanged;

        /// <summary>
        /// Occurs when the <see cref="MenuItem.Role"/> property changes.
        /// </summary>
        public static event EventHandler? ItemRoleChanged;

        /// <summary>
        /// Occurs when the <see cref="MenuItem.Image"/> property changes.
        /// </summary>
        public static event EventHandler? ItemImageChanged;

        /// <summary>
        /// Occurs when the <see cref="MenuItem.DisabledImage"/> property changes.
        /// </summary>
        public static event EventHandler? ItemDisabledImageChanged;

        /// <summary>
        /// Occurs when the click action associated with the menu item has changed.
        /// </summary>
        /// <remarks>This event is triggered whenever the click action is updated,
        /// allowing subscribers to respond to the change.</remarks>
        public static event EventHandler? ItemClickActionChanged;

        /// <summary>
        /// Raises the <see cref="MenuOpening"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="CancelEventArgs"/> that contains the event data.</param>
        public static void RaiseMenuOpening(ContextMenu sender, CancelEventArgs e)
        {
            MenuOpening?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="MenuClosing"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseMenuClosing(ContextMenu sender, EventArgs e)
        {
            MenuClosing?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemEnabledChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemEnabledChanged(MenuItem sender, EventArgs e)
        {
            ItemEnabledChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemVisibleChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemVisibleChanged(MenuItem sender, EventArgs e)
        {
            ItemVisibleChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemClick"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemClick(MenuItem sender, EventArgs e)
        {
            ItemClick?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemTextChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemTextChanged(MenuItem sender, EventArgs e)
        {
            ItemTextChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="BaseEventArgs{MenuChangeKind}"/>
        /// that contains the event data.</param>
        public static void RaiseItemChanged(MenuItem sender, BaseEventArgs<MenuChangeKind> e)
        {
            ItemChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemOpened"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemOpened(MenuItem sender, EventArgs e)
        {
            ItemOpened?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemClosed"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemClosed(MenuItem sender, EventArgs e)
        {
            ItemClosed?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemHighlighted"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemHighlighted(MenuItem sender, EventArgs e)
        {
            ItemHighlighted?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemShortcutChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemShortcutChanged(MenuItem sender, EventArgs e)
        {
            ItemShortcutChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemCheckedChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemCheckedChanged(MenuItem sender, EventArgs e)
        {
            ItemCheckedChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemRoleChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemRoleChanged(MenuItem sender, EventArgs e)
        {
            ItemRoleChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemImageChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemImageChanged(MenuItem sender, EventArgs e)
        {
            ItemImageChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemDisabledImageChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemDisabledImageChanged(MenuItem sender, EventArgs e)
        {
            ItemDisabledImageChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ItemClickActionChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseItemClickActionChanged(MenuItem sender, EventArgs e)
        {
            ItemClickActionChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the "item inserted" events to notify subscribers
        /// that a new menu item has been inserted.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> that triggered the event.</param>
        /// <param name="e">The event data associated with the menu item insertion.</param>
        public static void RaiseItemInserted(Menu sender, MenuChangeEventArgs e)
        {
            ItemInserted?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the "item removed" events to notify subscribers that
        /// a menu item has been removed.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> that was removed.</param>
        /// <param name="e">The event data associated with the removal,
        /// containing additional details.</param>
        public static void RaiseItemRemoved(Menu sender, MenuChangeEventArgs e)
        {
            if (sender is MainMenu)
                MainMenuItemRemoved?.Invoke(sender, e);
            else
            if (sender is ContextMenu)
                ContextMenuItemRemoved?.Invoke(sender, e);
            else
            if (sender is MenuItem)
                ItemRemoved?.Invoke(sender, e);
        }
    }
}
