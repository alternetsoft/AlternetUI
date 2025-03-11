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

        public SimpleToolBarView Header => tabs;

        public IList<IView> Tabs => Header.Buttons;

        public new View Content
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

        public virtual void SelectTab(int index)
        {
            if (Header.Buttons.Count > index)
                ((SimpleToolBarView.IToolBarItem)Header.Buttons[index]).IsSticky = true;
        }

        public virtual void SelectFirstTab()
        {
            if(Header.Buttons.Count > 0)
                ((SimpleToolBarView.IToolBarItem)Header.Buttons[0]).IsSticky = true;
        }

        public virtual SimpleToolBarView.IToolBarItem? GetTab(int index)
        {
            if (Header.Buttons.Count > index)
                return (SimpleToolBarView.IToolBarItem)Header.Buttons[index];
            return null;
        }
    }
}
