using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements transparent panel which is handled inside the library.
    /// This control doesn't have native handle, border, or background.
    /// It can be used to layout other generic controls.
    /// Do not add <see cref="Control"/> descendants to this panel, only generic control are allowed.
    /// </summary>
    public partial class TransparentPanel : GenericControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransparentPanel"/> class.
        /// </summary>
        public TransparentPanel()
        {
            UserPaint = true;
        }

        /// <summary>
        /// Adds the specified child controls to the container.
        /// </summary>
        /// <param name="children">The child controls to add.</param>
        /// <returns>The current instance of <see cref="TransparentPanel"/>.</returns>
        public virtual TransparentPanel WithChildren(params GenericControl[] children)
        {
            DoInsideLayout(() =>
            {
                Children.AddRange(children);
            });

            return this;
        }

        /// <inheritdoc/>
        public sealed override void DefaultPaint(PaintEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected sealed override void OnPaint(PaintEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected sealed override void OnPaintBackground(PaintEventArgs e)
        {
        }
    }
}
