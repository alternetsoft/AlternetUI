using System;

namespace Alternet.UI
{
    /// <summary>
    /// Defined in order to make library more compatible with the legacy code.
    /// </summary>
    public class ToolStripMenuItem : MenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class.
        /// </summary>
        public ToolStripMenuItem()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/> and <paramref name="shortcut"/> for the menu item.
        /// </summary>
        public ToolStripMenuItem(string text, KeyGesture shortcut)
            : base(text, shortcut)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/> for the menu item.
        /// </summary>
        public ToolStripMenuItem(string text)
            : base(text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/>, <paramref name="onClick"/> and
        /// <paramref name="shortcut"/> for the menu item.
        /// </summary>
        public ToolStripMenuItem(string text, EventHandler? onClick = null, KeyGesture? shortcut = null)
            : base(text, onClick, shortcut)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/> and <paramref name="onClick"/>
        /// for the menu item.
        /// </summary>
        public ToolStripMenuItem(string text, Action? onClick)
            : base(text, onClick)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class with
        /// the specified <paramref name="text"/>, <paramref name="onClick"/> and
        /// <paramref name="shortcut"/> for the menu item.
        /// </summary>
        public ToolStripMenuItem(string text, Action? onClick, KeyGesture? shortcut)
            : base(text, onClick, shortcut)
        {
        }
    }
}
