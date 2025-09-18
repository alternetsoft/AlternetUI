using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with notify icon.
    /// </summary>
    public interface INotifyIconHandler : IDisposable
    {
        /// <inheritdoc cref="NotifyIcon.Text"/>
        string? Text { get; set; }

        /// <summary>
        /// Gets or sets action which is called when notify icon is clicked.
        /// </summary>
        Action? Click { get; set; }

        /// <summary>
        /// Gets or sets the action which is called when the
        /// left mouse button is released on the notify icon.
        /// </summary>
        Action? LeftMouseButtonUp { get; set; }

        /// <summary>
        /// Gets or sets the action which is called when the left
        /// mouse button is pressed on the notify icon.
        /// </summary>
        Action? LeftMouseButtonDown { get; set; }

        /// <summary>
        /// Gets or sets the action which is called when the left mouse button
        /// is double-clicked on the notify icon.
        /// </summary>
        Action? LeftMouseButtonDoubleClick { get; set; }

        /// <summary>
        /// Gets or sets the action which is called when the right mouse button
        /// is released on the notify icon.
        /// </summary>
        Action? RightMouseButtonUp { get; set; }

        /// <summary>
        /// Gets or sets the action which is called when the right mouse button
        /// is pressed on the notify icon.
        /// </summary>
        Action? RightMouseButtonDown { get; set; }

        /// <summary>
        /// Gets or sets the action which is called when the right mouse button
        /// is double-clicked on the notify icon.
        /// </summary>
        Action? RightMouseButtonDoubleClick { get; set; }

        /// <summary>
        /// Gets or sets the action to be executed when the notify icon is created.
        /// </summary>
        Action? Created { get; set; }

        /// <inheritdoc cref="NotifyIcon.Icon"/>
        Image? Icon { set; }

        /// <inheritdoc cref="NotifyIcon.Visible"/>
        bool Visible { get; set; }

        /// <inheritdoc cref="NotifyIcon.IsIconInstalled"/>
        bool IsIconInstalled { get; }

        /// <inheritdoc cref="NotifyIcon.IsOk"/>
        bool IsOk { get; }

        /// <summary>
        /// Sets the context menu to be displayed when the notification icon is right-clicked.
        /// </summary>
        /// <param name="menu">The <see cref="ContextMenu"/> to show.
        /// If <see langword="null"/>, the context menu
        /// will be removed.</param>
        void SetContextMenu(ContextMenu? menu);
    }
}