using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a panel in a side-bar control. A side bar can contain multiple panels,
    /// </summary>
    public partial class SideBarPanel : TabControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SideBarPanel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public SideBarPanel(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SideBarPanel"/> class.
        /// </summary>
        public SideBarPanel()
        {
            BackgroundColor = ThemedColors.Window;
            Header.BackgroundColor = ThemedColors.ButtonFace;
        }
    }
}
