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

        public virtual void SelectTab(int index)
        {
            var tab = GetTab(index);
            tab?.ClickedAction?.Invoke();
        }

        public virtual void SelectFirstTab() => SelectTab(0);

        public virtual SimpleToolBarView.IToolBarItem? GetTab(int index)
        {
            if (Header.Buttons.Count > index)
                return (SimpleToolBarView.IToolBarItem)Header.Buttons[index];
            return null;
        }

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
