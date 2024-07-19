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
        /// Gets or sets action which is called when notify icon is double-clicked.
        /// </summary>
        Action? DoubleClick { get; set; }

        /// <inheritdoc cref="NotifyIcon.Icon"/>
        Image? Icon { set; }

        /// <inheritdoc cref="NotifyIcon.Menu"/>
        ContextMenu? Menu { set; }

        /// <inheritdoc cref="NotifyIcon.Visible"/>
        bool Visible { get; set; }

        /// <inheritdoc cref="NotifyIcon.IsIconInstalled"/>
        bool IsIconInstalled { get; }

        /// <inheritdoc cref="NotifyIcon.IsOk"/>
        bool IsOk { get; }
    }
}