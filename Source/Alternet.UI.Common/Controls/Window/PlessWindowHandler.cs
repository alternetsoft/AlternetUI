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
    /// Implements dummy <see cref="IWindowHandler"/> provider.
    /// </summary>
    public class PlessWindowHandler : PlessControlHandler, IWindowHandler
    {
        /// <inheritdoc/>
        public Action<HandledEventArgs<string>>? InputBindingCommandExecuted { get; set; }

        /// <inheritdoc/>
        public Action<CancelEventArgs>? Closing { get; set; }

        /// <inheritdoc/>
        public virtual bool ShowInTaskbar { get; set; }

        /// <inheritdoc/>
        public virtual bool MaximizeEnabled { get; set; }

        /// <inheritdoc/>
        public virtual bool MinimizeEnabled { get; set; }

        /// <inheritdoc/>
        public virtual bool CloseEnabled { get; set; }

        /// <inheritdoc/>
        public virtual bool AlwaysOnTop { get; set; }

        /// <inheritdoc/>
        public virtual bool IsToolWindow { get; set; }

        /// <inheritdoc/>
        public virtual bool Resizable { get; set; }

        /// <inheritdoc/>
        public virtual bool HasBorder { get; set; }

        /// <inheritdoc/>
        public virtual bool HasTitleBar { get; set; }

        /// <inheritdoc/>
        public virtual bool HasSystemMenu { get; set; }

        /// <inheritdoc/>
        public Action? StateChanged { get; set; }

        /// <inheritdoc/>
        public virtual string Title { get; set; } = string.Empty;

        /// <inheritdoc/>
        public virtual bool IsModal { get; }

        /// <inheritdoc/>
        public virtual bool IsPopupWindow { get; set; }

        /// <inheritdoc/>
        public virtual WindowStartLocation StartLocation { get; set; }

        /// <inheritdoc/>
        public virtual bool IsActive { get; }

        /// <inheritdoc/>
        public virtual WindowState State { get; set; }

        /// <inheritdoc/>
        public virtual Window[] OwnedWindows { get; } = Array.Empty<Window>();

        /// <inheritdoc/>
        public virtual ModalResult ModalResult { get; set; }

        /// <inheritdoc/>
        public virtual object? StatusBar { get; set; }

        /// <inheritdoc/>
        public virtual void Activate()
        {
        }

        /// <inheritdoc/>
        public virtual void AddInputBinding(InputBinding value)
        {
        }

        /// <inheritdoc/>
        public virtual void Close()
        {
        }

        /// <inheritdoc/>
        public virtual void RemoveInputBinding(InputBinding item)
        {
        }

        /// <inheritdoc/>
        public virtual void SetIcon(IconSet? value)
        {
        }

        /// <inheritdoc/>
        public virtual void SetMenu(object? value)
        {
        }

        /// <inheritdoc/>
        public virtual void SetOwner(Window? owner)
        {
        }

        /// <inheritdoc/>
        public virtual ModalResult ShowModal(IWindow? owner)
        {
            return ModalResult.Canceled;
        }
    }
}
