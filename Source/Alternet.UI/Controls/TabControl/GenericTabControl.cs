using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

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
    public partial class GenericTabControl : Control
    {
        private readonly CardPanel cardPanel = new();
        private readonly CardPanelHeader cardPanelHeader = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericTabControl"/> class.
        /// </summary>
        public GenericTabControl()
            : base()
        {
            cardPanelHeader.UserPaint = true;
            cardPanelHeader.Paint += Header_Paint;
            UserPaint = true;

            Layout = LayoutStyle.Vertical;
            cardPanelHeader.TabHasBorder = false;
            cardPanelHeader.VerticalAlignment = UI.VerticalAlignment.Top;
            cardPanelHeader.UpdateCardsMode = WindowSizeToContentMode.None;
            cardPanelHeader.Parent = this;
            cardPanel.Margin = 1;
            cardPanel.Parent = this;
            cardPanel.VerticalAlignment = UI.VerticalAlignment.Fill;
            cardPanel.HorizontalAlignment = UI.HorizontalAlignment.Fill;
            Padding = 0;
            cardPanelHeader.CardPanel = cardPanel;
        }

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
                if(Layout == LayoutStyle.Vertical)
                {
                    if (Header.VerticalAlignment == UI.VerticalAlignment.Bottom)
                        return TabAlignment.Bottom;
                    else
                        return TabAlignment.Top;
                }
                else
                {
                    if (Header.HorizontalAlignment == UI.HorizontalAlignment.Right)
                        return TabAlignment.Right;
                    else
                        return TabAlignment.Left;
                }
            }

            set
            {
                if (TabAlignment == value)
                    return;
                DoInsideLayout(() =>
                {
                    var isVertical = value == TabAlignment.Top || value == TabAlignment.Bottom;

                    if (isVertical)
                    {
                        cardPanelHeader.HorizontalAlignment = UI.HorizontalAlignment.Stretch;

                        Layout = LayoutStyle.Vertical;
                        Header.Layout = LayoutStyle.Horizontal;
                        if (value == TabAlignment.Bottom)
                            Header.VerticalAlignment = UI.VerticalAlignment.Bottom;
                        else
                            Header.VerticalAlignment = UI.VerticalAlignment.Top;
                    }
                    else
                    {
                        cardPanelHeader.VerticalAlignment = UI.VerticalAlignment.Stretch;

                        Layout = LayoutStyle.Horizontal;
                        Header.Layout = LayoutStyle.Vertical;
                        if (value == TabAlignment.Right)
                            Header.HorizontalAlignment = UI.HorizontalAlignment.Right;
                        else
                            Header.HorizontalAlignment = UI.HorizontalAlignment.Left;
                    }
                });
                Invalidate();
                Header.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets how the tabs and content are aligned.
        /// </summary>
        public bool IsVertical
        {
            get
            {
                return Layout == LayoutStyle.Vertical;
            }

            set
            {
                if(value)
                    Layout = LayoutStyle.Vertical;
                else
                    Layout = LayoutStyle.Horizontal;
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
        /// Selects tab with the specified index.
        /// </summary>
        public void SelectTab(int index)
        {
            Header.SelectedTabIndex = index;
        }

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
            var headerTabIndex = Header.Add(title, cardPanel[cardIndex].UniqueId);
            if (headerTabIndex == 0)
                SelectFirstTab();
            return cardIndex;
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
            var headerTabIndex = Header.Add(title, cardPanel[cardIndex].UniqueId);
            if(headerTabIndex == 0)
                SelectFirstTab();
            return cardIndex;
        }

        /// <summary>
        /// Gets interior border color.
        /// </summary>
        /// <returns></returns>
        protected virtual Color GetInteriorBorderColor()
        {
            var color = Borders?.GetObjectOrNull(GenericControlState.Normal)?.Color;

            if(color is null)
            {
                if (IsDarkBackground)
                {
                    color = Color.FromRgb(61, 61, 61);
                }
                else
                {
                    color = Color.FromRgb(204, 206, 219);
                }
            }

            return color;
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            var r = Header.Bounds;
            r.Size += Header.Margin.Size;
            DrawTabControlInterior(
                e.Graphics,
                ClientRectangle,
                r,
                GetInteriorBorderColor(),
                TabAlignment);
        }

        private void Header_Paint(object? sender, PaintEventArgs e)
        {
            DrawTabsInterior(
                Header,
                e.DrawingContext,
                e.Bounds,
                GetInteriorBorderColor(),
                TabAlignment);
        }
    }
}