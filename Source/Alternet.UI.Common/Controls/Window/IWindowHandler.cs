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
    public interface IWindowHandler : IControlHandler
    {
        /// <summary>
        /// Gets or sets action which is called when window state is changed.
        /// </summary>
        Action? StateChanged { get; set; }

        /// <summary>
        /// Gets or sets action which is called when command specified in the input bindings
        /// is executed.
        /// </summary>
        Action<HandledEventArgs<string>>? InputBindingCommandExecuted { get; set; }

        /// <summary>
        /// Gets or sets action which is called when window is closing.
        /// </summary>
        Action<CancelEventArgs>? Closing { get; set; }

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

        /// <inheritdoc cref="Window.HasBorder"/>
        bool HasBorder { get; set; }

        /// <inheritdoc cref="Window.HasTitleBar"/>
        bool HasTitleBar { get; set; }

        /// <inheritdoc cref="Window.HasSystemMenu"/>
        bool HasSystemMenu { get; set; }

        /// <inheritdoc cref="AbstractControl.Title"/>
        string Title { get; set; }

        /// <summary>
        /// Gets whether or not window is modal dialog.
        /// </summary>
        bool IsModal { get; }

        /// <inheritdoc cref="Window.IsPopupWindow"/>
        bool IsPopupWindow { get; set; }

        /// <inheritdoc cref="Window.StartLocation"/>
        WindowStartLocation StartLocation { get; set; }

        /// <inheritdoc cref="Window.IsActive"/>
        bool IsActive { get; }

        /// <inheritdoc cref="Window.State"/>
        WindowState State { get; set; }

        /// <inheritdoc cref="Window.OwnedWindows"/>
        Window[] OwnedWindows { get; }

        /// <inheritdoc cref="DialogWindow.ModalResult"/>
        ModalResult ModalResult { get; set; }

        /// <summary>
        /// Gets or sets window status bar.
        /// </summary>
        object? StatusBar { get; set; }

        /// <summary>
        /// Gets a <see cref="Window"/> this handler provides the implementation for.
        /// </summary>
        new Window Control { get; }

        /// <summary>
        /// Sets window owner.
        /// </summary>
        /// <param name="owner">Window owner.</param>
        void SetOwner(Window? owner);

        /// <summary>
        /// Shows window as a modal dialog.
        /// </summary>
        /// <param name="owner">Window owner.</param>
        /// <returns></returns>
        ModalResult ShowModal(IWindow? owner);

        /// <inheritdoc cref="Window.Close"/>
        void Close();

        /// <inheritdoc cref="Window.Activate"/>
        void Activate();

        /// <summary>
        /// Adds input binding.
        /// </summary>
        /// <param name="value"></param>
        void AddInputBinding(InputBinding value);

        /// <summary>
        /// Removes input binding.
        /// </summary>
        /// <param name="item"></param>
        void RemoveInputBinding(InputBinding item);

        /// <summary>
        /// Sets window icon.
        /// </summary>
        /// <param name="value"></param>
        void SetIcon(IconSet? value);

        /// <summary>
        /// Sets window menu.
        /// </summary>
        /// <param name="value"></param>
        void SetMenu(object? value);
    }
}
