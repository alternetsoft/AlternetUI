using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Called when the mouse pointer leaves child control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnChildMouseLeave(object? sender, EventArgs e)
        {
        }

        /// <summary>
        /// Called before the <see cref="KeyDown" /> event of the child control is raised.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="KeyEventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnBeforeChildKeyDown(object? sender, KeyEventArgs e)
        {
        }

        /// <summary>
        /// This method is invoked when the control's child lost focus.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnChildLostFocus(object? sender, LostFocusEventArgs e)
        {
        }

        /// <summary>
        /// Called when the layout of the child control is updated.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnChildLayoutUpdated(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the size of the child control is changed.
        /// </summary>
        /// <param name="childControl">The child control whose size has changed.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnChildSizeChanged(AbstractControl childControl)
        {
        }

        /// <summary>
        /// Called after the <see cref="KeyDown" /> event of the child control is raised.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="KeyEventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnAfterChildKeyDown(object? sender, KeyEventArgs e)
        {
        }

        /// <summary>
        /// Called before the <see cref="MouseWheel" /> event of the child control is raised.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">The source of the event.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnBeforeChildMouseWheel(object? sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called after the <see cref="MouseWheel" /> event of the child control is raised.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">The source of the event.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnAfterChildMouseWheel(object? sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when a <see cref="AbstractControl"/> is removed from the
        /// <see cref="AbstractControl.Children"/> collections.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnChildRemoved(AbstractControl childControl)
        {
        }

        /// <summary>
        /// Called when a <see cref="AbstractControl"/> is inserted into
        /// the <see cref="AbstractControl.Children"/>.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnChildInserted(int index, AbstractControl childControl)
        {
        }


    }
}
