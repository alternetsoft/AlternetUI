using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Defined in order to make library more compatible with the legacy code.
    /// </summary>
    [ControlCategory(KnownControlCategory.Hidden)]
    public partial class ContextMenuStrip : ContextMenu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenuStrip"/> class.
        /// </summary>
        public ContextMenuStrip()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenuStrip"/> class with the specified container.
        /// </summary>
        /// <param name="container">The container to add the ContextMenuStrip to.</param>
        public ContextMenuStrip(IContainer container) : base(container)
        {
        }

        /// <summary>
        /// Added for the compatibility with the legacy code. In the current implementation, this method does nothing.
        /// </summary>
        public void SuspendLayout()
        {
        }

        /// <summary>
        /// Added for the compatibility with the legacy code. In the current implementation, this method does nothing.
        /// </summary>
        public void ResumeLayout(bool performLayout = true)
        {
        }
    }
}
