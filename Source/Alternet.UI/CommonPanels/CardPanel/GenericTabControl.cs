using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that manages a related set of tab pages.
    /// </summary>
    /// <remarks>
    /// This control is implemented inside the Alternet.UI and doesn't
    /// use native tab control.
    /// </remarks>
    [ControlCategory("Containers")]
    public class GenericTabControl : Control
    {
        private readonly CardPanel cardPanel = new();
        private readonly CardPanelHeader cardPanelHeader = new();
        private readonly StackPanel stackPanel = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericTabControl"/> class.
        /// </summary>
        public GenericTabControl()
            : base()
        {
            stackPanel.Orientation = StackPanelOrientation.Vertical;
            stackPanel.Parent = this;
            cardPanelHeader.Parent = stackPanel;
            cardPanel.Parent = stackPanel;
            cardPanel.VerticalAlignment = VerticalAlignment.Stretch;
            cardPanelHeader.CardPanel = cardPanel;
        }

        /// <summary>
        /// Gets or sets how the tabs are aligned.
        /// </summary>
        public bool IsVertical
        {
            get
            {
                return stackPanel.Orientation == StackPanelOrientation.Vertical;
            }

            set
            {
                if(value)
                    stackPanel.Orientation = StackPanelOrientation.Vertical;
                else
                    stackPanel.Orientation = StackPanelOrientation.Horizontal;
            }
        }

        /// <summary>
        /// Gets internal control with tab pages.
        /// </summary>
        [Browsable(false)]
        public CardPanel Contents => cardPanel;

        /// <summary>
        /// Gets internal control with tab labels.
        /// </summary>
        [Browsable(false)]
        public CardPanelHeader Header => cardPanelHeader;

        /// <summary>
        /// Adds new page.
        /// </summary>
        /// <param name="title">Page title.</param>
        /// <param name="fnCreate">Function which creates the control.</param>
        /// <returns>
        /// Created page index.
        /// </returns>
        public int Add(string title, Func<Control> fnCreate)
        {
            var cardIndex = cardPanel.Add(title, fnCreate);
            Header.Add(title, cardPanel[cardIndex].UniqueId);
            return cardIndex;
        }

        /// <summary>
        /// Adds new page.
        /// </summary>
        /// <param name="page">Page title and control.</param>
        /// <returns>
        /// Created page index.
        /// </returns>
        public int Add(NameValue<Control> page)
        {
            return Add(page.Name, page.Value);
        }

        /// <summary>
        /// Adds new page.
        /// </summary>
        /// <param name="page">Page title and control creation function.</param>
        /// <returns>
        /// Created page index.
        /// </returns>
        public int Add(NameValue<Func<Control>> page)
        {
            return Add(page.Name, page.Value);
        }

        /// <summary>
        /// Adds new pages.
        /// </summary>
        /// <param name="pages">Collection of pages.</param>
        public void AddRange(IEnumerable<NameValue<Control>> pages)
        {
            foreach(var page in pages)
                Add(page);
        }

        /// <summary>
        /// Adds new pages.
        /// </summary>
        /// <param name="pages">Collection of pages.</param>
        public void AddRange(IEnumerable<NameValue<Func<Control>>?> pages)
        {
            foreach (var page in pages)
            {
                if(page is not null)
                    Add(page);
            }
        }

        /// <summary>
        /// Selects the first tab if it exists.
        /// </summary>
        public virtual void SelectFirstTab()
        {
            Header.SelectFirstTab();
        }

        /// <summary>
        /// Adds new page.
        /// </summary>
        /// <param name="title">Page title.</param>
        /// <param name="control">Control.</param>
        /// <returns>
        /// Created page index.
        /// </returns>
        public virtual int Add(string title, Control? control = null)
        {
            control ??= new();
            var cardIndex = cardPanel.Add(title, control);
            Header.Add(title, cardPanel[cardIndex].UniqueId);
            return cardIndex;
        }
    }
}
