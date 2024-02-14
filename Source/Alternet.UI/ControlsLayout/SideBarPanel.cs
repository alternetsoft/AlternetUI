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
    /// This control can be used in <see cref="SplittedPanel"/> side bars.
    /// </remarks>
    public partial class SideBarPanel : Control
    {
        private readonly CardPanelHeader header = new()
        {
            Padding = (0, 5, 0, 0),
            BorderWidth = 0,
            UpdateCardsMode = WindowSizeToContentMode.None,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="SideBarPanel"/> class.
        /// </summary>
        public SideBarPanel()
        {
            Layout = LayoutStyle.Vertical;
            BackgroundColor = SystemColors.Window;
            header.VerticalAlignment = UI.VerticalAlignment.Top;
            header.Parent = this;
            header.BackgroundColor = SystemColors.ButtonFace;
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
                if (header.VerticalAlignment == UI.VerticalAlignment.Bottom)
                    return TabAlignment.Bottom;
                else
                    return TabAlignment.Top;
            }

            set
            {
                if (TabAlignment == value)
                    return;
                if (value == TabAlignment.Bottom)
                    header.VerticalAlignment = UI.VerticalAlignment.Bottom;
                else
                    header.VerticalAlignment = UI.VerticalAlignment.Top;
            }
        }

        /// <summary>
        /// Adds new tab.
        /// </summary>
        /// <param name="title">Tab title.</param>
        /// <param name="control">Tab control.</param>
        /// <returns>Index of the added tab.</returns>
        public int Add(string title, Control? control)
        {
            if (control is not null)
            {
                control.Visible = false;
                control.VerticalAlignment = VerticalAlignment.Fill;
                control.HorizontalAlignment = HorizontalAlignment.Fill;
                control.Parent = this;
            }

            var result = Header.Add(title, control);
            return result;
        }

        /// <summary>
        /// Selects first tab.
        /// </summary>
        public void SelectFirstTab()
        {
            Header.SelectFirstTab();
        }

        /// <summary>
        /// Selects first tab.
        /// </summary>
        public void SelectTab(int index)
        {
            Header.SelectedTabIndex = index;
        }
    }
}
