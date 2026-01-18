using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a standard horizontal scroll bar.
    /// </summary>
    [ControlCategory("Common")]
    public partial class HScrollBar : ScrollBar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HScrollBar"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public HScrollBar(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HScrollBar"/> class.
        /// </summary>
        public HScrollBar()
        {
            IsVertical = false;
        }
    }
}
