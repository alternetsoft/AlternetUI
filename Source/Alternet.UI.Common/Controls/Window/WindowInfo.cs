using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains window properties.
    /// </summary>
    public class WindowInfo
    {
        /// <summary>
        /// <inheritdoc cref="Window.MinimizeEnabled"/>
        /// </summary>
        public bool MinimizeEnabled = true;

        /// <summary>
        /// <inheritdoc cref="Window.MaximizeEnabled"/>
        /// </summary>
        public bool MaximizeEnabled = true;

        /// <summary>
        /// <inheritdoc cref="Window.ShowInTaskbar"/>
        /// </summary>
        public bool ShowInTaskbar = true;

        /// <summary>
        /// <inheritdoc cref="Window.CloseEnabled"/>
        /// </summary>
        public bool CloseEnabled = true;

        /// <summary>
        /// <inheritdoc cref="Window.HasSystemMenu"/>
        /// </summary>
        public bool HasSystemMenu = true;

        /// <summary>
        /// <inheritdoc cref="Window.HasBorder"/>
        /// </summary>
        public bool HasBorder = true;

        /// <summary>
        /// <inheritdoc cref="Window.TopMost"/>
        /// </summary>
        public bool AlwaysOnTop = false;

        /// <summary>
        /// <inheritdoc cref="Window.IsToolWindow"/>
        /// </summary>
        public bool IsToolWindow = false;

        /// <summary>
        /// <inheritdoc cref="Window.IsPopupWindow"/>
        /// </summary>
        public bool IsPopupWindow = false;

        /// <summary>
        /// <inheritdoc cref="Window.Resizable"/>
        /// </summary>
        public bool Resizable = true;

        /// <summary>
        /// <inheritdoc cref="Window.HasTitleBar"/>
        /// </summary>
        public bool HasTitleBar = true;

        /// <summary>
        /// <inheritdoc cref="Window.StartLocation"/>
        /// </summary>
        public WindowStartLocation StartLocation = WindowStartLocation.Default;
    }
}
