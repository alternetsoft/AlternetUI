using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Connects <see cref="AbstractControl"/> and visual designer.
    /// </summary>
    public interface IComponentDesigner
    {
        /// <summary>
        /// Occurs when the property value changes.
        /// </summary>
        event EventHandler<ObjectPropertyChangedEventArgs>? ObjectPropertyChanged;

        /// <summary>
        /// Occurs when the left mouse button was pressed on the control
        /// </summary>
        event EventHandler<MouseEventArgs>? MouseLeftButtonDown;

        /// <summary>
        /// Occurs when the control receives focus.
        /// </summary>
        event EventHandler? ControlGotFocus;

        /// <summary>
        /// Occurs when the control created.
        /// </summary>
        event EventHandler? ControlCreated;

        /// <summary>
        /// Occurs when the control disposed.
        /// </summary>
        event EventHandler? ControlDisposed;

        /// <summary>
        /// Occurs when the control's parent was changed.
        /// </summary>
        event EventHandler? ControlParentChanged;

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
        /// <param name="e">Event arguments.</param>
        void RaiseGotFocus(object control, GotFocusEventArgs e);

        /// <summary>
        /// Notifiers designer when control was created.
        /// </summary>
        /// <param name="control">Control which was created.</param>
        /// <param name="e">Event arguments.</param>
        void RaiseCreated(object control, EventArgs e);

        /// <summary>
        /// Notifiers designer when control was disposed.
        /// </summary>
        /// <param name="control">Control which was disposed.</param>
        /// <param name="e">Event arguments.</param>
        void RaiseDisposed(object control, EventArgs e);

        /// <summary>
        /// Notifiers designer when control's parent was changed.
        /// </summary>
        /// <param name="control">Affected control.</param>
        /// <param name="e">Event arguments.</param>
        void RaiseParentChanged(object control, EventArgs e);

        /// <summary>
        /// Notifiers designer when the left mouse button was pressed on the control.
        /// </summary>
        /// <param name="control">Affected control.</param>
        /// <param name="e">Event arguments.</param>
        void RaiseMouseLeftButtonDown(object control, MouseEventArgs e);
    }
}
