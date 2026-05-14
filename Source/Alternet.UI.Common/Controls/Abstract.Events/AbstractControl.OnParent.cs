using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Called after the <see cref="MouseWheel" /> event of the <see cref="Parent"/> is raised.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">The source of the event.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnAfterParentMouseWheel(object? sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called before the <see cref="KeyDown" /> event of the <see cref="Parent"/> is raised.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="KeyEventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnBeforeParentKeyDown(object? sender, KeyEventArgs e)
        {
        }

        /// <summary>
        /// Called after the <see cref="SizeChanged" /> event of the <see cref="Parent"/> is raised.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="KeyEventArgs" /> that contains the event data.</param>
        protected virtual void OnAfterParentSizeChanged(object? sender, HandledEventArgs e)
        {
        }

        /// <summary>
        /// Called before the <see cref="KeyPress" /> event of the <see cref="Parent"/> is raised.
        /// </summary>
        /// <param name="e">A <see cref="KeyPressEventArgs" /> that contains the event data.</param>
        /// <param name="sender">The source of the event.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnBeforeParentKeyPress(object? sender, KeyPressEventArgs e)
        {
        }

        /// <summary>
        /// Called after the <see cref="KeyPress" /> event of the <see cref="Parent"/> is raised.
        /// </summary>
        /// <param name="e">A <see cref="KeyPressEventArgs" /> that contains the event data.</param>
        /// <param name="sender">The source of the event.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnAfterParentKeyPress(object? sender, KeyPressEventArgs e)
        {
        }

        /// <summary>
        /// Called after the <see cref="KeyDown" /> event of the <see cref="Parent"/> is raised.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="KeyEventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnAfterParentKeyDown(object? sender, KeyEventArgs e)
        {
        }

        /// <summary>
        /// Called before the <see cref="MouseDown" /> event of the <see cref="Parent"/> is raised.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnBeforeParentMouseDown(object? sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called after the <see cref="MouseDown" /> event of the <see cref="Parent"/> is raised.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnAfterParentMouseDown(object? sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called before the <see cref="MouseUp" /> event of the <see cref="Parent"/> is raised.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnBeforeParentMouseUp(object? sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called after the <see cref="MouseUp" /> event of the <see cref="Parent"/> is raised.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnAfterParentMouseUp(object? sender, MouseEventArgs e)
        {
        }
    }
}
