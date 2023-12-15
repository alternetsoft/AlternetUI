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
    public class HScrollBar : ScrollBar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HScrollBar"/> class.
        /// </summary>
        public HScrollBar()
            : base()
        {
            IsVertical = false;
        }
    }
}
