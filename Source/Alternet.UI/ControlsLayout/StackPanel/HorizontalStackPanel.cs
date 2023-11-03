using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Arranges child controls into a single line that can be oriented horizontally.
    /// </summary>
    [ControlCategory("Containers")]
    public class HorizontalStackPanel : StackPanel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalStackPanel"/> class.
        /// </summary>
        public HorizontalStackPanel(params Control[] children)
        {
            foreach (var child in children)
                child.Parent = this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalStackPanel"/> class.
        /// </summary>
        public HorizontalStackPanel()
            : base()
        {
        }

        /// <summary>
        /// Gets a value that indicates the dimension by which child
        /// controls are stacked.
        /// </summary>
        [Browsable(false)]
        public override StackPanelOrientation Orientation
        {
            get => StackPanelOrientation.Horizontal;

            set
            {
            }
        }
    }
}
