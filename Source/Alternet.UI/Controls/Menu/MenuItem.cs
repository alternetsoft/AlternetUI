using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an individual item that is displayed within a menu.
    /// </summary>
    public class MenuItem : Menu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class.
        /// </summary>
        public MenuItem() :
            this("")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with the specified text for the menu item.
        /// </summary>
        public MenuItem(string text) :
            this(text, null, Key.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with the specified text for the menu item.
        /// </summary>
        public MenuItem(string text, Key shortcut) :
            this(text, null, shortcut)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with the specified text and action for the menu item.
        /// </summary>
        public MenuItem(string text, EventHandler onClick) :
            this(text, onClick, Key.None)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with a specified properties for the menu item.
        /// </summary>
        public MenuItem(string text, EventHandler? onClick, Key shortcut)
        {
            Text = text;
            Shortcut = shortcut;
            if (onClick != null)
                Click += onClick;
        }

        string text = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating the caption of the menu item.
        /// </summary>
        /// <remarks>
        /// Setting this property to "-" makes the item a separator.
        /// Use underscore (<c>_</c>) symbol before a character to make it an access key.
        /// </remarks>
        public string Text
        {
            get
            {
                CheckDisposed();
                return text;
            }

            set
            {
                CheckDisposed();

                if (value == text)
                    return;

                text = value;
                TextChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Text"/> property changes.
        /// </summary>
        public event EventHandler? TextChanged;

        Key shortcut;

        /// <summary>
        /// Gets or sets a value indicating the shortcut key associated with the menu item.
        /// </summary>
        public Key Shortcut
        {
            get
            {
                CheckDisposed();
                return shortcut;
            }

            set
            {
                CheckDisposed();

                if (shortcut == value)
                    return;

                shortcut = value;
                ShortcutChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Shortcut"/> property changes.
        /// </summary>
        public event EventHandler? ShortcutChanged;

        bool @checked;

        /// <summary>
        /// Gets or sets a value indicating whether a check mark appears next to the text of the menu item.
        /// </summary>
        public bool Checked
        {
            get
            {
                CheckDisposed();
                return @checked;
            }

            set
            {
                CheckDisposed();
                @checked = value;
                CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Checked"/> property changes.
        /// </summary>
        public event EventHandler? CheckedChanged;
    }
}