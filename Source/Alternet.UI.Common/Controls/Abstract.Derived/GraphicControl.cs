using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Descendant of the <see cref="UserControl"/> which doesn't need to have focus.
    /// An example of <see cref="GraphicControl"/> is <see cref="Label"/>.
    /// </summary>
    public partial class GraphicControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicControl"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public GraphicControl(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicControl"/> class.
        /// </summary>
        public GraphicControl()
        {
            IsGraphicControl = true;
        }
    }
}
