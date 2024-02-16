using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="Control.GlobalOnLayout"/> event.
    /// </summary>
    public class DefaultLayoutEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLayoutEventArgs"/> class.
        /// </summary>
        /// <param name="container">Container control which childs need to be processed.</param>
        /// <param name="layout">Layout style to use.</param>
        /// <param name="bounds">Rectangle in which layout is performed.</param>
        /// <param name="children">List of controls to layout.</param>
        public DefaultLayoutEventArgs(
            Control container,
            LayoutStyle layout,
            RectD bounds,
            IReadOnlyList<Control> children)
        {
            Container = container;
            Layout = layout;
            Bounds = bounds;
            Children = children;
        }

        /// <summary>
        /// Container control which childs need to be processed.
        /// </summary>
        public Control Container { get; }

        /// <summary>
        /// Layout style to use.
        /// </summary>
        public LayoutStyle Layout { get; set; }

        /// <summary>
        /// Rectangle in which layout is performed.
        /// </summary>
        public RectD Bounds { get; set; }

        /// <summary>
        /// List of controls to layout.
        /// </summary>
        public IReadOnlyList<Control> Children { get; set; }
    }
}