using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implenents horizontal line in the menu which separate items.
    /// </summary>
    public class ToolStripSeparator : ToolStripMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolStripSeparator"/> class.
        /// </summary>
        public ToolStripSeparator()
            : base("-")
        {
        }
    }
}
