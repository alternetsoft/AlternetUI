using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Connects <see cref="Control"/> and visual designer.
    /// </summary>
    public interface IComponentDesigner
    {
        /// <summary>
        /// Occurs when the property value changes.
        /// </summary>
        event EventHandler<PropertyChangeEventArgs>? PropertyChanged;

        /// <summary>
        /// Occurs when the control receives focus.
        /// </summary>
        event EventHandler ControlGotFocus;

        /// <summary>
        /// Occurs when the control created.
        /// </summary>
        event EventHandler ControlCreated;

        /// <summary>
        /// Occurs when the control disposed.
        /// </summary>
        event EventHandler ControlDisposed;

        /// <summary>
        /// Notifies designer about property change.
        /// </summary>
        /// <param name="instance">Object instance which property was changed.</param>
        /// <param name="propName">Property name. Optional. If <c>null</c>, more than one
        /// property were changed.</param>
        void RaisePropertyChanged(object? instance, string? propName);

        /// <summary>
        /// Notifiers designer when control got focus.
        /// </summary>
        /// <param name="control">Control which received focus.</param>
        void RaiseGotFocus(Control control);

        /// <summary>
        /// Notifiers designer when control was created.
        /// </summary>
        /// <param name="control">Control which was created.</param>
        void RaiseCreated(Control control);

        /// <summary>
        /// Notifiers designer when control was disposed.
        /// </summary>
        /// <param name="control">Control which was disposed.</param>
        void RaiseDisposed(Control control);
    }
}
