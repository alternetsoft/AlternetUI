using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Default <see cref="IComponentDesigner"/> implementation.
    /// </summary>
    public class ComponentDesigner : IComponentDesigner
    {
        /// <summary>
        /// Occurs when the property value changes.
        /// </summary>
        public event EventHandler<ObjectPropertyChangedEventArgs>? PropertyChanged;

        /// <summary>
        /// Occurs when the control receives focus.
        /// </summary>
        public event EventHandler? ControlGotFocus;

        /// <see cref="IComponentDesigner.ControlCreated"/>
        public event EventHandler? ControlCreated;

        /// <see cref="IComponentDesigner.ControlDisposed"/>
        public event EventHandler? ControlDisposed;

        /// <summary>
        /// Occurs when the control's parent was changed.
        /// </summary>
        public event EventHandler? ControlParentChanged;

        /// <summary>
        /// Gets or sets default <see cref="IComponentDesigner"/>.
        /// </summary>
        public static IComponentDesigner? Default { get; set; }

        /// <summary>
        /// Gets default <see cref="IComponentDesigner"/>. Initializes <see cref="Default"/>
        /// property if it is not already assigned.
        /// </summary>
        public static IComponentDesigner SafeDefault
        {
            get
            {
                if (Default is null)
                    InitDefault();
                return Default!;
            }
        }

        /// <summary>
        /// Initializes <see cref="Default"/> if it wasn't assigned previously.
        /// </summary>
        public static void InitDefault()
        {
            Default ??= new ComponentDesigner();
        }

        /// <inheritdoc/>
        public virtual void RaisePropertyChanged(object? instance, string? propName)
        {
            PropertyChanged?.Invoke(this, new ObjectPropertyChangedEventArgs(instance, propName));
        }

        /// <summary>
        /// Notifiers designer when control got focus.
        /// </summary>
        /// <param name="control">Control which received focus.</param>
        public void RaiseGotFocus(Control control)
        {
            ControlGotFocus?.Invoke(control, EventArgs.Empty);
        }

        /// <see cref="IComponentDesigner.RaiseCreated"/>
        public void RaiseCreated(Control control)
        {
            ControlCreated?.Invoke(control, EventArgs.Empty);
        }

        /// <see cref="IComponentDesigner.RaiseDisposed"/>
        public void RaiseDisposed(Control control)
        {
            ControlDisposed?.Invoke(control, EventArgs.Empty);
        }

        /// <see cref="IComponentDesigner.RaiseParentChanged"/>
        public void RaiseParentChanged(Control control)
        {
            ControlParentChanged?.Invoke(control, EventArgs.Empty);
        }
    }
}
