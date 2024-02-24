using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;
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
    [DefaultProperty("Pages")]
    [DefaultEvent("SelectedIndexChanged")]
    public partial class TabControl : Control
    {
        /// <summary>
        /// Gets or sets default minimal tab size in the header.
        /// </summary>
        public static SizeD DefaultMinTabSize = (0, 0);

        private readonly TabControlCardPanel cardPanel = new();
        private readonly CardPanelHeader cardPanelHeader = new();
        private bool hasInteriorBorder = true;
        private TabSizeMode sizeMode = TabSizeMode.Normal;
        private TabAppearance tabAppearance = TabAppearance.Normal;
        private int addSuspended;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        public TabControl()
            : base()
        {
            cardPanelHeader.UserPaint = true;
            cardPanelHeader.Paint += Header_Paint;
            UserPaint = true;

            base.Layout = LayoutStyle.Vertical;
            cardPanelHeader.TabHasBorder = false;
            cardPanelHeader.TabClick += CardPanelHeader_TabClick;
            cardPanelHeader.ButtonSizeChanged += CardPanelHeader_ButtonSizeChanged;
            cardPanelHeader.VerticalAlignment = UI.VerticalAlignment.Top;
            cardPanelHeader.UpdateCardsMode = WindowSizeToContentMode.None;
            cardPanelHeader.Parent = this;
            cardPanel.Margin = 1;
            cardPanel.Parent = this;
            cardPanel.VerticalAlignment = UI.VerticalAlignment.Fill;
            cardPanel.HorizontalAlignment = UI.HorizontalAlignment.Fill;
            cardPanelHeader.CardPanel = cardPanel;
            cardPanel.Children.ItemInserted += Pages_ItemInserted;
            cardPanel.Children.ItemRemoved += Pages_ItemRemoved;
        }

        /// <summary>
        /// Occurs when the <see cref="SelectedIndex" /> property has changed.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? SelectedIndexChanged;

        /// <summary>
        /// Occurs when the selected page has changed.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? SelectedPageChanged;

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.TabControl;

        /// <summary>
        /// Gets the collection of tab pages in this tab control.
        /// </summary>
        /// <value>A <see cref="Collection{Control}"/> that contains pages
        /// in this <see cref="TabControl"/>.</value>
        [Content]
        public Collection<Control> Pages
        {
            get
            {
                return cardPanel.Children;
            }
        }

        /// <summary>
        /// Gets or sets colors and styles theme of the tabs.
        /// </summary>
        public virtual SpeedButton.KnownTheme TabTheme
        {
            get => Header.TabTheme;
            set
            {
                if (TabTheme == value)
                    return;
                Header.TabTheme = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override IReadOnlyList<FrameworkElement> ContentElements => Pages;

        /// <summary>
        /// Gets or sets the index of the currently selected tab page.
        /// </summary>
        /// <returns>
        /// The zero-based index of the currently selected tab page.
        /// The default is -1, which is also the value if no tab page is selected.
        /// </returns>
        [Browsable(false)]
        [Category("Behavior")]
        [DefaultValue(-1)]
        public virtual int SelectedIndex
        {
            get
            {
                var result = Header.SelectedTabIndex;
                if (result is null)
                    return -1;
                return result.Value;
            }

            set
            {
                Header.SelectedTabIndex = value;
                Invalidate();
                RaiseSelectedIndexChanged();
            }
        }

        /// <summary>
        ///  Gets or sets the currently selected tab page.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Control? SelectedPage
        {
            get
            {
                var index = Header.SelectedTabIndex;
                var result = GetControlAt(index);
                return result;
            }

            set
            {
                if (TabCount == 0)
                    return;
                var selectedPage = SelectedPage;
                if (selectedPage == value)
                    return;
                var index = GetTabIndex(value);
                SelectedIndex = index ?? -1;
            }
        }

        /// <summary>
        /// Gets or sets the way that the control's tabs are sized.
        /// </summary>
        /// <returns>One of the <see cref="TabSizeMode" /> values.
        /// The default is <see langword="Normal" />.</returns>
        /// <remarks>
        /// This property was added for compatibility and currently doesn't change
        /// control behavior.
        /// </remarks>
        [Category("Behavior")]
        [DefaultValue(TabSizeMode.Normal)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Browsable(false)]
        public virtual TabSizeMode SizeMode
        {
            get
            {
                return sizeMode;
            }

            set
            {
                if (sizeMode == value)
                    return;
                sizeMode = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the visual appearance of the control's tabs.
        /// </summary>
        /// <returns>One of the <see cref="TabAppearance" /> values.
        /// The default is <see langword="Normal" />.</returns>
        /// <remarks>
        /// This property was added for compatibility and currently doesn't change
        /// control behavior.
        /// </remarks>
        [Category("Behavior")]
        [Localizable(true)]
        [DefaultValue(TabAppearance.Normal)]
        [Browsable(false)]
        public virtual TabAppearance Appearance
        {
            get
            {
                return tabAppearance;
            }

            set
            {
                if (tabAppearance == value)
                    return;
                tabAppearance = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets padding of contents.
        /// </summary>
        public virtual Thickness ContentPadding
        {
            get => Contents.Padding;

            set => Contents.Padding = value;
        }

        /// <summary>
        /// Gets or sets whether tab interior border is visible.
        /// </summary>
        public virtual bool HasInteriorBorder
        {
            get => hasInteriorBorder;

            set
            {
                if (hasInteriorBorder == value)
                    return;
                hasInteriorBorder = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets the number of tabs in the tab strip.
        /// </summary>
        /// <returns>The number of tabs in the tab strip.</returns>
        [Category("Appearance")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int TabCount
        {
            get
            {
                return Header.Tabs.Count;
            }
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
        public virtual TabAlignment TabAlignment
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

                        base.Layout = LayoutStyle.Vertical;
                        Header.Layout = LayoutStyle.Horizontal;
                        if (value == TabAlignment.Bottom)
                            Header.VerticalAlignment = UI.VerticalAlignment.Bottom;
                        else
                            Header.VerticalAlignment = UI.VerticalAlignment.Top;
                    }
                    else
                    {
                        cardPanelHeader.VerticalAlignment = UI.VerticalAlignment.Stretch;

                        base.Layout = LayoutStyle.Horizontal;
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
        /// Gets selected tab page.
        /// </summary>
        [Browsable(false)]
        public virtual Control? SelectedControl
        {
            get
            {
                return GetControlAt(SelectedIndex);
            }
        }

        /// <summary>
        /// Gets the display area of the control's tab pages.
        /// </summary>
        /// <returns>
        /// A <see cref="RectD" /> that represents the display area
        /// of the tab pages.
        /// </returns>
        /// <remarks>
        /// <see cref="ContentPadding"/> is not included in the result. This function
        /// returns client area of the control, except tab header buttons area.
        /// </remarks>
        public virtual RectD DisplayRectangle
        {
            get
            {
                var result = Contents.Bounds;
                return result;
            }
        }

        /// <summary>
        /// Gets or sets how the tabs and content are aligned.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsVertical
        {
            get
            {
                return TabAlignment == TabAlignment.Top || TabAlignment == TabAlignment.Bottom;
            }

            set
            {
                if(value)
                    TabAlignment = TabAlignment.Top;
                else
                    TabAlignment = TabAlignment.Left;
            }
        }

        /// <summary>
        /// Gets internal control with tab pages.
        /// </summary>
        [Browsable(false)]
        internal CardPanel Contents => cardPanel;

        /// <summary>
        /// Gets internal control with tab labels.
        /// </summary>
        [Browsable(false)]
        internal CardPanelHeader Header => cardPanelHeader;

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection => Pages;

        /// <summary>
        /// Adds new page.
        /// </summary>
        /// <param name="page">Page title and control.</param>
        /// <returns>
        /// Created page index.
        /// </returns>
        public virtual int Add(NameValue<Control> page)
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
        public virtual int Add(NameValue<Func<Control>> page)
        {
            return Add(page.Name, page.Value);
        }

        /// <summary>
        /// Adds new pages.
        /// </summary>
        /// <param name="pages">Collection of pages.</param>
        public virtual void AddRange(IEnumerable<NameValue<Control>> pages)
        {
            foreach(var page in pages)
                Add(page);
        }

        /// <summary>
        /// Adds new pages.
        /// </summary>
        /// <param name="pages">Collection of pages.</param>
        public virtual void AddRange(IEnumerable<NameValue<Func<Control>>?> pages)
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
            if (TabCount > 0)
                SelectedIndex = 0;
        }

        /// <summary>
        /// Selects tab with the specified index.
        /// </summary>
        public virtual void SelectTab(int? index)
        {
            if (index is null)
            {
                SelectFirstTab();
                return;
            }

            SelectedIndex = index.Value;
        }

        /// <summary>
        /// Adds new page.
        /// </summary>
        /// <param name="control">Control.</param>
        /// <returns>
        /// Created page index.
        /// </returns>
        public virtual int Add(Control control)
        {
            return Add(control.Title, control);
        }

        /// <summary>
        /// Inserts new page at the specified index.
        /// </summary>
        /// <param name="index">The position at which to insert the tab.</param>
        /// <param name="control">Control.</param>
        /// <returns>
        /// Created page index.
        /// </returns>
        public virtual int Insert(int? index, Control control)
        {
            return Insert(index, control.Title, control);
        }

        /// <summary>
        /// Inserts new page at the specified index.
        /// </summary>
        /// <param name="index">The position at which to insert the tab.</param>
        /// <param name="title">Page title.</param>
        /// <param name="control">Control.</param>
        /// <returns>
        /// Created page index.
        /// </returns>
        public virtual int Insert(int? index, string title, Control? control = null)
        {
            addSuspended++;

            try
            {
                control ??= new TabPage();
                var cardIndex = cardPanel.Add(title, control);
                var headerTabIndex = Header.Insert(index, title, cardPanel[cardIndex].UniqueId);
                if (headerTabIndex == 0)
                    SelectFirstTab();
                else
                {
                    control.Visible = false;
                    control.Parent = Contents;
                }

                Invalidate();
                return cardIndex;
            }
            finally
            {
                addSuspended--;
            }
        }

        /// <summary>
        /// Adds new page.
        /// </summary>
        /// <param name="title">Page title.</param>
        /// <param name="fnCreate">Function which creates the control.</param>
        /// <returns>
        /// Created page index.
        /// </returns>
        public virtual int Add(string title, Func<Control> fnCreate)
        {
            addSuspended++;

            try
            {
                var cardIndex = cardPanel.Add(title, fnCreate);
                var headerTabIndex = Header.Add(title, cardPanel[cardIndex].UniqueId);
                if (headerTabIndex == 0)
                    SelectFirstTab();
                Invalidate();
                return cardIndex;
            }
            finally
            {
                addSuspended--;
            }
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
            return Insert(Header.Tabs.Count, title, control);
        }

        /// <summary>
        /// Gets title of the specified tab page.
        /// </summary>
        /// <param name="index">Index of the tab page.</param>
        /// <returns></returns>
        public virtual string? GetTitle(int? index)
        {
            var item = Header.GetTab(index);
            if (item is null)
                return null;
            return item.Text;
        }

        /// <summary>
        /// Sets title of the specified tab page.
        /// </summary>
        /// <param name="index">Index of the tab page.</param>
        /// <param name="value">New title of the tab page.</param>
        /// <returns></returns>
        public virtual bool SetTitle(int? index, string? value)
        {
            var item = Header.GetTab(index);
            if (item is null)
                return false;
            item.Text = value;
            return true;
        }

        /// <summary>
        /// Removed all items from the control.
        /// </summary>
        public virtual void RemoveAll()
        {
            if (TabCount == 0)
                return;

            DoInsideLayout(() =>
            {
                for (int i = TabCount - 1; i >= 0; i--)
                {
                    RemoveAt(i);
                }
            });
            Invalidate();
        }

        /// <summary>
        /// Removes tab page with the specified index.
        /// </summary>
        /// <param name="index">The index of the tab page.</param>
        /// <returns><c>true</c> if tab page was removed, <c>false</c> otherwise.</returns>
        public virtual bool RemoveAt(int? index)
        {
            var tabPage = GetControlAt(index);
            if (tabPage is null)
                return false;

            DoInsideLayout(() =>
            {
                Header.RemoveAt(index);
                SelectTab(index);
                tabPage.Parent = null;
            });

            Invalidate();
            return true;
        }

        /// <summary>
        /// Gets index of the tab page or <c>null</c> if control is not found in tabs.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public virtual int? GetTabIndex(Control? control)
        {
            if (control is null)
                return null;

            var tabs = Header.Tabs;

            for(int i = 0; i < tabs.Count; i++)
            {
                if (tabs[i].CardControl == control)
                    return i;
                var card = cardPanel.Find(tabs[i].CardUniqueId);
                if (card?.Control == control)
                    return i;
            }

            return null;
        }

        /// <summary>
        /// Removes tab page from the control.
        /// </summary>
        /// <param name="control">Control to remove</param>
        /// <returns></returns>
        public virtual bool Remove(Control control)
        {
            var index = GetTabIndex(control);
            return RemoveAt(index);
        }

        /// <summary>
        /// Sets images for the tab.
        /// </summary>
        /// <param name="index">The index of the tab page.</param>
        /// <param name="image">Tab image.</param>
        /// <param name="disabledImage">Tab image in the disabled state.</param>
        public virtual void SetTabImage(int? index, Image? image, Image? disabledImage = null)
        {
            var item = Header.GetTab(index);
            if (item is null)
                return;
            item.Image = image;
            item.DisabledImage = disabledImage;
        }

        /// <summary>
        /// Sets images for the tab.
        /// </summary>
        /// <param name="index">The index of the tab page.</param>
        /// <param name="image">Tab image.</param>
        /// <param name="disabledImage">Tab image in the disabled state.</param>
        public virtual void SetTabImage(int? index, ImageSet? image, ImageSet? disabledImage = null)
        {
            var item = Header.GetTab(index);
            if (item is null)
                return;
            item.ImageSet = image;
            item.DisabledImageSet = disabledImage;
        }

        /// <summary>
        /// Gets tab page at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual Control? GetControlAt(int? index)
        {
            var headerTab = Header.GetTab(index);
            if (headerTab is null)
                return null;
            var tabPage = headerTab.CardControl;
            if (tabPage is null)
            {
                var cardPanelItem = Contents.Find(headerTab.CardUniqueId);
                if (cardPanelItem is null)
                    return null;
                if (cardPanelItem.ControlCreated)
                    tabPage = cardPanelItem.Control;
            }

            return tabPage;
        }

        /// <inheritdoc/>
        public override void OnChildPropertyChanged(
            Control child,
            string propName,
            bool directChild = true)
        {
            if (propName == nameof(Title))
            {
                var index = GetTabIndex(child);
                if (index is null)
                    return;
                Header.Tabs[index.Value].HeaderButton.Text = child.Title;
            }
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
                    color = Color.FromRgb(41, 41, 41);
                }
                else
                {
                    color = Color.FromRgb(204, 206, 219);
                }
            }

            return color;
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!hasInteriorBorder || TabCount == 0)
                return;
            var r = Header.Bounds;
            r.Size += Header.Margin.Size;
            DrawTabControlInterior(
                e.Graphics,
                ClientRectangle,
                r,
                GetInteriorBorderColor(),
                TabAlignment);
        }

        private void CardPanelHeader_ButtonSizeChanged(object? sender, EventArgs e)
        {
            Invalidate();
        }

        private void CardPanelHeader_TabClick(object? sender, EventArgs e)
        {
            RaiseSelectedIndexChanged();
        }

        private void RaiseSelectedIndexChanged()
        {
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            SelectedPageChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Pages_ItemRemoved(object? sender, int index, Control item)
        {
            Remove(item);
        }

        private void Pages_ItemInserted(object? sender, int index, Control item)
        {
            if (addSuspended > 0)
                return;
            if (Contents.Find(item) is not null)
                return;
            Add(item);
        }

        private void Header_Paint(object? sender, PaintEventArgs e)
        {
            if (!hasInteriorBorder || TabCount == 0)
                return;
            var r = e.Bounds;
            if(r.Width > ClientSize.Width)
                r.Width = ClientSize.Width;
            if (r.Height > ClientSize.Height)
                r.Height = ClientSize.Height;

            DrawTabsInterior(
                Header,
                e.DrawingContext,
                r,
                GetInteriorBorderColor(),
                TabAlignment);
        }

        private class TabControlCardPanel : CardPanel
        {
            /// <inheritdoc/>
            public override void OnChildPropertyChanged(
                Control child,
                string propName,
                bool directChild = true)
            {
                Parent?.OnChildPropertyChanged(child, propName, false);
            }
        }
    }
}