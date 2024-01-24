using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements side bar panel with header.
    /// </summary>
    /// <remarks>
    /// This control can be used in <see cref="SplittedPanel"/> side bars.
    /// </remarks>
    public class SideBarPanel : VerticalStackPanel
    {
        private readonly CardPanelHeader header = new()
        {
            Margin = (0, 5, 0, 0),
            BorderWidth = 0,
            UpdateCardsMode = WindowSizeToContentMode.None,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="SideBarPanel"/> class.
        /// </summary>
        public SideBarPanel()
        {
            AllowStretch = true;
            header.Parent = this;
        }

        /// <summary>
        /// Gets attached header control.
        /// </summary>
        public CardPanelHeader Header => header;
    }
}
