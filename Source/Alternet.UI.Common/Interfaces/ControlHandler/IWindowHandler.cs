using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="IControlHandler"/> with properties and methods
    /// specific to the window.
    /// </summary>
    public interface IWindowHandler
    {
        /// <inheritdoc cref="Window.ShowInTaskbar"/>
        bool ShowInTaskbar { get; set; }

        /// <inheritdoc cref="Window.MaximizeEnabled"/>
        bool MaximizeEnabled { get; set; }

        /// <inheritdoc cref="Window.MinimizeEnabled"/>
        bool MinimizeEnabled { get; set; }

        /// <inheritdoc cref="Window.CloseEnabled"/>
        bool CloseEnabled { get; set; }

        /// <inheritdoc cref="Window.TopMost"/>
        bool AlwaysOnTop { get; set; }

        /// <inheritdoc cref="Window.IsToolWindow"/>
        bool IsToolWindow { get; set; }

        /// <inheritdoc cref="Window.Resizable"/>
        bool Resizable { get; set; }

        /// <inheritdoc cref="Window.HasTitleBar"/>
        bool HasTitleBar { get; set; }

        /// <inheritdoc cref="Window.HasSystemMenu"/>
        bool HasSystemMenu { get; set; }

        /// <inheritdoc cref="AbstractControl.Title"/>
        string Title { get; set; }

        /// <inheritdoc cref="Window.IsPopupWindow"/>
        bool IsPopupWindow { get; set; }

        /// <inheritdoc cref="Window.IsActive"/>
        bool IsActive { get; }

        /// <inheritdoc cref="Window.State"/>
        WindowState State { get; set; }

        /// <summary>
        /// Gets or sets window status bar.
        /// </summary>
        DisposableObject? StatusBar { get; set; }

        /// <summary>
        /// Gets a <see cref="Window"/> this handler provides the implementation for.
        /// </summary>
        Window? Control { get; }

        /// <inheritdoc cref="Window.Close()"/>
        void Close();

        /// <inheritdoc cref="Window.Activate"/>
        void Activate();

        /// <summary>
        /// Sets window icon.
        /// </summary>
        /// <param name="value"></param>
        void SetIcon(IconSet? value);

        /// <summary>
        /// Sets window menu.
        /// </summary>
        /// <param name="value"></param>
        void SetMenu(DisposableObject? value);

        /// <summary>
        /// Sets minimal window size.
        /// </summary>
        /// <param name="size">The minimal window size.</param>
        void SetMinSize(SizeD size);

        /// <summary>
        /// Sets maximal window size.
        /// </summary>
        /// <param name="size">The maximal window size.</param>
        void SetMaxSize(SizeD size);
    }
}
