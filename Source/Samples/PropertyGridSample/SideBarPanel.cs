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
    public class SideBarPanel : VerticalStackPanel
    {
        private readonly CardPanelHeader header = new()
        {
            Margin = (0, 5, 0, 0),
            BorderWidth = 0,
            UpdateCardsMode = WindowSizeToContentMode.None,
        };

        public SideBarPanel()
        {
            AllowStretch = true;
            header.Parent = this;
        }

        public CardPanelHeader Header => header;
    }
}
