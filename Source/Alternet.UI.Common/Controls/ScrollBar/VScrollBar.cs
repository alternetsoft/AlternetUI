using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a standard vertical scroll bar.
    /// </summary>
    [ControlCategory("Common")]
    public partial class VScrollBar : ScrollBar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VScrollBar"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public VScrollBar(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HScrollBar"/> class.
        /// </summary>
        public VScrollBar()
        {
            IsVertical = true;
        }
    }
}
