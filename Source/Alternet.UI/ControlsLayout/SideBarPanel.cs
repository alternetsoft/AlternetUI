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
    public partial class SideBarPanel : VerticalStackPanel
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
            header.VerticalAlignment = VerticalAlignment.Top;
            header.Parent = this;
        }

        /// <summary>
        /// Gets attached header control.
        /// </summary>
        public CardPanelHeader Header => header;

        /// <summary>
        /// Gets or sets the area of the control (for example, along the top) where
        /// the tabs are aligned.
        /// </summary>
        /// <value>One of the <see cref="TabAlignment"/> values. The default is
        /// <see cref="TabAlignment.Top"/>.</value>
        /// <remarks>
        /// Currently only <see cref="TabAlignment.Top"/> and <see cref="TabAlignment.Bottom"/>
        /// alignment is supported.
        /// </remarks>
        public TabAlignment TabAlignment
        {
            get
            {
                if (header.VerticalAlignment == VerticalAlignment.Bottom)
                    return TabAlignment.Bottom;
                else
                    return TabAlignment.Top;
            }

            set
            {
                if (TabAlignment == value)
                    return;
                if (value == TabAlignment.Bottom)
                    header.VerticalAlignment = VerticalAlignment.Bottom;
                else
                    header.VerticalAlignment = VerticalAlignment.Top;
            }
        }
    }
}
