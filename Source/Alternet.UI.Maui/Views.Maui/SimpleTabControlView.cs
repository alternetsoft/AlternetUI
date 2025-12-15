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
    public partial class SimpleTabControlView : BaseContentView
    {
        /// <summary>
        /// Gets or sets the default style applied to the tabs.
        /// </summary>
        public static SimpleToolBarView.StickyButtonStyle DefaultTabStyle
            = SimpleToolBarView.StickyButtonStyle.Border;

        /// <summary>
        /// Indicates whether the tab is displayed in bold when selected.
        /// </summary>
        public static bool DefaultTabIsBoldWhenSelected = true;

        private static Microsoft.Maui.Graphics.Color? altHeaderBackColor;

        private readonly SimpleToolBarView tabs = new();
        private readonly Grid grid = new();
        private readonly ContentView content = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTabControlView"/> class.
        /// </summary>
        public SimpleTabControlView()
        {
            tabs.AllowMultipleSticky = false;
            tabs.StickyStyle = DefaultTabStyle;
            tabs.IsBoldWhenSticky = DefaultTabIsBoldWhenSelected;

            grid.RowDefinitions =
            [
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Star },
            ];

            grid.Add(tabs, 0, 0);
            grid.Add(content, 0, 1);
            base.Content = grid;
        }

        /// <summary>
        /// Occurs when the selected tab changes.
        /// </summary>
        /// <remarks>This event is triggered whenever the currently selected
        /// tab is changed by the user or
        /// programmatically. Subscribers can use this event to perform
        /// actions based on the newly selected tab.</remarks>
        public event EventHandler? SelectedTabChanged;

        /// <summary>
        /// Gets or sets the alternative header background color.
        /// </summary>
        public static Microsoft.Maui.Graphics.Color AltHeaderBackColor
        {
            get
            {
                if(altHeaderBackColor is not null)
                    return altHeaderBackColor;

                Microsoft.Maui.Graphics.Color headerColor;

                if (Alternet.UI.SystemSettings.AppearanceIsDark)
                {
                    headerColor = Microsoft.Maui.Graphics.Color.FromRgb(37, 37, 38);
                }
                else
                {
                    headerColor = Microsoft.Maui.Graphics.Color.FromRgb(245, 245, 245);
                }

                return headerColor;
            }

            set
            {
                altHeaderBackColor = value;
            }
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
        /// Gets the number of tabs in the tab control view.
        /// </summary>
        public int TabCount => Tabs.Count;

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
        /// Gets or sets the selected tab control item.
        /// </summary>
        public virtual TabControlItem? SelectedTab
        {
            get
            {
                return GetTab(SelectedIndex);
            }

            set
            {
                SelectTab(value);
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected tab.
        /// </summary>
        public virtual int SelectedIndex
        {
            get
            {
                for(int i = 0; i < TabCount; i++)
                {
                    if (GetTabButton(i)?.IsSticky ?? false)
                        return i;
                }

                return -1;
            }

            set
            {
                var item = GetTab(value);
                SelectTab(item);
            }
        }

        /// <summary>
        /// Removes the specified tab control item.
        /// </summary>
        /// <param name="item">The tab control item to remove.</param>
        public virtual void RemoveTab(TabControlItem? item)
        {
            if (item is null)
                return;

            var isSelected = item == SelectedTab;

            tabs.Remove(item.Button);

            if (isSelected)
                SelectFirstTab();
        }

        /// <summary>
        /// Selects the specified tab control item.
        /// </summary>
        /// <param name="item">The tab control item to select.</param>
        public virtual void SelectTab(TabControlItem? item)
        {
            var btn = item?.Button;
            if (btn is null)
                return;
            btn.IsSticky = true;
            Content = item!.PageResolver?.Invoke();
            SelectedTabChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Selects the tab at the specified index.
        /// </summary>
        /// <param name="index">The index of the tab to select.</param>
        public virtual void SelectTab(int index)
        {
            var tab = GetTabButton(index);

            if(tab is null)
            {
                Content = null;
            }
            else
            {
                tab.ClickedAction?.Invoke();
            }
        }

        /// <summary>
        /// Selects the first tab.
        /// </summary>
        public virtual void SelectFirstTab() => SelectTab(0);

        /// <summary>
        /// Sets the font for the tabs.
        /// </summary>
        /// <param name="fontFamily">The font family to use for the tabs.</param>
        /// <param name="fontsize">The font size to use for the tabs.</param>
        public virtual void SetTabFont(string? fontFamily, double fontsize)
        {
            Header.TabFontFamily = fontFamily;
            Header.TabFontSize = fontsize;
        }

        /// <summary>
        /// Gets the tab button at the specified index.
        /// </summary>
        /// <param name="index">The index of the tab button to get.</param>
        /// <returns>The tab button at the specified index,
        /// or null if the index is out of range.</returns>
        public virtual SimpleToolBarView.IToolBarItem? GetTabButton(int index)
        {
            if (Header.Buttons.Count > index && index >= 0)
            {
                var result = (SimpleToolBarView.IToolBarItem)Header.Buttons[index];
                return result;
            }

            return null;
        }

        /// <summary>
        /// Gets the tab at the specified index.
        /// </summary>
        /// <param name="index">The index of the tab to get.</param>
        /// <returns>The tab at the specified index, or null if the index is out of range.</returns>
        public virtual TabControlItem? GetTab(int index)
        {
            var button = GetTabButton(index);
            if (button is null)
                return null;
            return new TabControlItem(button);
        }

        /// <summary>
        /// Adds a new tab to the tab control view.
        /// </summary>
        /// <param name="text">The text to display on the tab.</param>
        /// <param name="getView">A function that returns the content view for the tab.</param>
        /// <param name="toolTip">The tooltip text for the tab.</param>
        /// <param name="image">The image to display on the tab.</param>
        public virtual TabControlItem Add(
            string? text,
            Func<View>? getView = null,
            string? toolTip = null,
            Drawing.SvgImage? image = null)
        {
            var btn = Header.AddButton(text, toolTip, image);
            var result = new TabControlItem(btn);
            result.PageResolver = getView;

            btn.ClickedAction = () =>
            {
                SelectedTab = result;
            };

            if (TabCount == 1)
                SelectFirstTab();

            return result;
        }

        /// <summary>
        /// Represents an item in the tab control view.
        /// </summary>
        public class TabControlItem
        {
            private const string resolverPropName = "CDF756F1-D3A3-4077-A2E4-13199C821EB9";

            private readonly UI.ObjectUniqueId uniqueId;
            private Alternet.UI.WeakReferenceValue<SimpleToolBarView.IToolBarItem> buttonRef
                = new();

            /// <summary>
            /// Initializes a new instance of the <see cref="TabControlItem"/> class.
            /// </summary>
            /// <param name="button">The button associated with the tab control item.</param>
            internal TabControlItem(SimpleToolBarView.IToolBarItem button)
            {
                this.buttonRef.Value = button;
                uniqueId = button.AttributesProvider.UniqueId;
            }

            /// <summary>
            /// Gets the button associated with the tab control item.
            /// </summary>
            public SimpleToolBarView.IToolBarItem? Button => buttonRef.Value;

            /// <summary>
            /// Gets the custom attributes for the tab control item.
            /// </summary>
            public UI.ICustomAttributes<string, object>? CustomAttr
                => Button?.AttributesProvider?.CustomAttr;

            /// <summary>
            /// Gets or sets the page resolver function for the tab control item.
            /// </summary>
            public Func<View>? PageResolver
            {
                get => (Func<View>?)CustomAttr?[resolverPropName];

                set
                {
                    if (CustomAttr is not null)
                        CustomAttr[resolverPropName] = value;
                }
            }

            /// <summary>
            /// Gets the page associated with the tab control item.
            /// </summary>
            public View? Page => PageResolver?.Invoke();

            /// <summary>
            /// Gets the attributes provider for the item.
            /// </summary>
            public UI.IBaseObjectWithAttr? AttributesProvider => Button?.AttributesProvider;

            /// <summary>
            /// Gets unique id of this object.
            /// </summary>
            public UI.ObjectUniqueId UniqueId => uniqueId;

            /// <summary>
            /// Determines whether two specified instances of <see cref="TabControlItem"/> are equal.
            /// </summary>
            /// <param name="left">The first <see cref="TabControlItem"/> to compare.</param>
            /// <param name="right">The second <see cref="TabControlItem"/> to compare.</param>
            /// <returns>true if the two <see cref="TabControlItem"/> instances
            /// are equal; otherwise, false.</returns>
            public static bool operator ==(TabControlItem? left, TabControlItem? right)
            {
                if (ReferenceEquals(left, right))
                {
                    return true;
                }

                if (left is null || right is null)
                {
                    return false;
                }

                return left.UniqueId == right.UniqueId;
            }

            /// <summary>
            /// Determines whether two specified instances of <see cref="TabControlItem"/> are not equal.
            /// </summary>
            /// <param name="left">The first <see cref="TabControlItem"/> to compare.</param>
            /// <param name="right">The second <see cref="TabControlItem"/> to compare.</param>
            /// <returns>true if the two <see cref="TabControlItem"/> instances are not equal;
            /// otherwise, false.</returns>
            public static bool operator !=(TabControlItem? left, TabControlItem? right)
            {
                return !(left == right);
            }

            /// <summary>
            /// Determines whether the specified object is equal to the current
            /// <see cref="TabControlItem"/>.
            /// </summary>
            /// <param name="obj">The object to compare with the current
            /// <see cref="TabControlItem"/>.</param>
            /// <returns>true if the specified object is equal to the current
            /// <see cref="TabControlItem"/>; otherwise, false.</returns>
            public override bool Equals(object? obj)
            {
                if (obj is TabControlItem item)
                {
                    return this == item;
                }

                return false;
            }

            /// <summary>
            /// Serves as the default hash function.
            /// </summary>
            /// <returns>A hash code for the current <see cref="TabControlItem"/>.</returns>
            public override int GetHashCode()
            {
                return UniqueId.GetHashCode();
            }
        }
    }
}