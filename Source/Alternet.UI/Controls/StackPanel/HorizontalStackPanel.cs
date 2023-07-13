using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Arranges child controls into a single line that can be oriented horizontally.
    /// </summary>
    public class HorizontalStackPanel : StackPanel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalStackPanel"/> class.
        /// </summary>
        public HorizontalStackPanel()
            : base()
        {
        }

        /// <inheritdoc/>
        public override StackPanelOrientation Orientation
        {
            get => StackPanelOrientation.Horizontal;

            set
            {
            }
        }
    }
}
