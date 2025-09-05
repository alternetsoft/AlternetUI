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
        public override bool HasBorder { get; set; }

        /// <inheritdoc/>
        public virtual bool HasTitleBar { get; set; }

        /// <inheritdoc/>
        public virtual bool HasSystemMenu { get; set; }

        /// <inheritdoc/>
        public virtual string Title { get; set; } = string.Empty;

        /// <inheritdoc/>
        public virtual bool IsPopupWindow { get; set; }

        /// <inheritdoc/>
        public virtual bool IsActive { get; }

        /// <inheritdoc/>
        public virtual WindowState State { get; set; }

        /// <inheritdoc/>
        public virtual DisposableObject? StatusBar { get; set; }

        Window? IWindowHandler.Control => (Window?)Control;

        /// <inheritdoc/>
        public virtual void Activate()
        {
        }

        /// <inheritdoc/>
        public virtual void Close()
        {
        }

        /// <inheritdoc/>
        public virtual void SetIcon(IconSet? value)
        {
        }

        /// <inheritdoc/>
        public virtual void SetMaxSize(SizeD size)
        {
        }

        /// <inheritdoc/>
        public virtual void SetMenu(DisposableObject? value)
        {
        }

        /// <inheritdoc/>
        public virtual void SetMinSize(SizeD size)
        {
        }
    }
}
