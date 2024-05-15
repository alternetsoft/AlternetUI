using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal abstract class BaseControlHandler : DisposableObject
    {
        private Control? control;

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// </summary>
        public Control Control
        {
            get => control ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="WxControlHandler"/> is attached
        /// to a <see cref="Control"/>.
        /// </summary>
        public bool IsAttached => control != null;

        public void RaiseChildInserted(Control childControl)
        {
            Control.RaiseChildInserted(childControl);
            OnChildInserted(childControl);
        }

        public void RaiseChildRemoved(Control childControl)
        {
            Control.RaiseChildRemoved(childControl);
            OnChildRemoved(childControl);
        }

        /// <summary>
        /// Attaches this handler to the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to attach this
        /// handler to.</param>
        public void Attach(Control control)
        {
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
        /// Called when a <see cref="Control"/> is inserted into the
        /// <see cref="Control.Children"/> collection.
        /// </summary>
        protected virtual void OnChildInserted(Control childControl)
        {
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is removed from the
        /// <see cref="Control.Children"/> collections.
        /// </summary>
        protected virtual void OnChildRemoved(Control childControl)
        {
        }

        /// <summary>
        /// Called after this handler has been detached from the <see cref="Control"/>.
        /// </summary>
        protected abstract void OnDetach();

        /// <summary>
        /// Called after this handler has been attached to a <see cref="Control"/>.
        /// </summary>
        protected abstract void OnAttach();
    }
}
