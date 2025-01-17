using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which help to implement custom control handler.
    /// </summary>
    public abstract class BaseControlHandler : DisposableObject
    {
        private Control? control;

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for
        /// or Null if control is disposed or not assigned.
        /// </summary>
        public Control? ControlOrNull
        {
            get
            {
                return control;
            }
        }

        /// <inheritdoc cref="AbstractControl.HasBorder"/>
        public virtual bool HasBorder
        {
            get => false;

            set
            {
            }
        }

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// If control is disposed or not attached to the handler, returns dummy control.
        /// </summary>
        public Control? Control
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this object is attached
        /// to a <see cref="Control"/>.
        /// </summary>
        public bool IsAttached => ControlOrNull != null;

        /// <summary>
        /// Attaches this handler to the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to attach this
        /// handler to.</param>
        public void Attach(Control control)
        {
            if (DisposingOrDisposed)
                return;
            this.control = control;
            OnAttach();
        }

        /// <summary>
        /// Detaches this handler from the <see cref="Control"/> it is attached to.
        /// </summary>
        public virtual void Detach()
        {
            OnDetach();

            control = null;
        }

        /// <summary>
        /// This methods is called when the layout of the control changes.
        /// </summary>
        public virtual void OnLayoutChanged()
        {
        }

        /// <summary>
        /// Called after this handler has been detached from the <see cref="Control"/>.
        /// </summary>
        protected virtual void OnDetach()
        {
        }

        /// <summary>
        /// Called after this handler has been attached to a <see cref="Control"/>.
        /// </summary>
        protected virtual void OnAttach()
        {
        }
    }
}
