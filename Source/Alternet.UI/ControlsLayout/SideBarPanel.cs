using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements side bar panel with header.
    /// </summary>
    /// <remarks>
    /// This control can be used in <see cref="SplittedPanel"/> side bars
    /// or any other places.
    /// </remarks>
    public partial class SideBarPanel : GenericTabControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SideBarPanel"/> class.
        /// </summary>
        public SideBarPanel()
        {
            Header.Padding = (0, 5, 0, 0);
            Header.BorderWidth = 0;
            BackgroundColor = SystemColors.Window;
            Header.BackgroundColor = SystemColors.ButtonFace;
        }
    }
}
