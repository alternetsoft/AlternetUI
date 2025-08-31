using System;

using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defined in order to make library more compatible with the legacy code.
    /// </summary>
    public partial class ToolStripMenuItem : MenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='MenuItem'/> class.
        /// </summary>
        public ToolStripMenuItem()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem" /> class that displays the
        /// specified text and image and that does the specified action when
        /// the <see cref="MenuItem" /> is clicked.</summary>
        /// <param name="text">The text to display on the menu item.</param>
        /// <param name="image">The <see cref="Image" /> to display on the control.</param>
        /// <param name="onClick">An event handler that raises the <see cref="AbstractControl.Click" />
        /// event when the control is clicked.</param>
        public ToolStripMenuItem(string text, Image? image, EventHandler onClick)
            : base(text, image, onClick)
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

        /// <summary>
        /// Alias to <see cref="ItemContainerElement{T}.Items"/>.
        /// </summary>
        public BaseCollection<MenuItem> DropDownItems => Items;
    }
}
