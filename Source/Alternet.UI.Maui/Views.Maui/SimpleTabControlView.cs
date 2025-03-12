using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a simple tab control view.
    /// </summary>
    public partial class SimpleTabControlView : ContentView
    {
        private readonly SimpleToolBarView tabs = new();
        private readonly Grid grid = new();
        private readonly ContentView content = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTabControlView"/> class.
        /// </summary>
        public SimpleTabControlView()
        {
            tabs.AllowMultipleSticky = false;
            tabs.StickyStyle = SimpleToolBarView.StickyButtonStyle.UnderlineFull;

            grid.Add(tabs, 0, 0);
            grid.Add(content, 0, 1);
            base.Content = grid;
        }

        /// <summary>
        /// Gets the header of the tab control view.
        /// </summary>
        public SimpleToolBarView Header => tabs;

        /// <summary>
        /// Gets the collection of tabs in the tab control view.
        /// </summary>
        public IList<IView> Tabs => Header.Buttons;

        /// <summary>
        /// Gets or sets the content of the tab control view.
        /// You can use this property to set the active tab contents.
        /// Normally, it is updated automatically when a new tab is selected
        /// with the control that was specified when the tab was added.
        /// </summary>
        public new View? Content
        {
            get
            {
                return content.Content;
            }

            set
            {
                content.Content = value;
            }
        }

        /// <summary>
        /// Selects the tab at the specified index.
        /// </summary>
        /// <param name="index">The index of the tab to select.</param>
        public virtual void SelectTab(int index)
        {
            var tab = GetTab(index);
            tab?.ClickedAction?.Invoke();
        }

        /// <summary>
        /// Selects the first tab.
        /// </summary>
        public virtual void SelectFirstTab() => SelectTab(0);

        /// <summary>
        /// Gets the tab at the specified index.
        /// </summary>
        /// <param name="index">The index of the tab to get.</param>
        /// <returns>The tab at the specified index, or null if the index is out of range.</returns>
        public virtual SimpleToolBarView.IToolBarItem? GetTab(int index)
        {
            if (Header.Buttons.Count > index)
                return (SimpleToolBarView.IToolBarItem)Header.Buttons[index];
            return null;
        }

        /// <summary>
        /// Adds a new tab to the tab control view.
        /// </summary>
        /// <param name="text">The text to display on the tab.</param>
        /// <param name="getView">A function that returns the content view for the tab.</param>
        /// <param name="toolTip">The tooltip text for the tab.</param>
        /// <param name="image">The image to display on the tab.</param>
        public virtual void Add(
            string? text,
            Func<View>? getView = null,
            string? toolTip = null,
            Drawing.SvgImage? image = null)
        {
            var btn = Header.AddButton(text, toolTip, image);

            btn.ClickedAction = () =>
            {
                btn.IsSticky = true;
                Content = getView?.Invoke();
            };
        }
    }
}
