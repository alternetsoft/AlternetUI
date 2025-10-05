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
    public partial class TabControl : HiddenBorder
    {
        /// <summary>
        /// Gets or sets default svg size for normal-dpi displays
        /// used in <see cref="SetTabSvg"/>.
        /// </summary>
        public static int DefaultSmallSvgSize = 16;

        /// <summary>
        /// Gets or sets default svg size for high-dpi displays
        /// used in <see cref="SetTabSvg"/>.
        /// </summary>
        public static int DefaultLargeSvgSize = 32;

        /// <summary>
        /// Gets or sets default minimal tab size in the header.
        /// </summary>
        public static SizeD DefaultMinTabSize = (0, 0);

        private readonly TabControlCardPanel cardPanel = new()
        {
            ParentBackColor = true,
            ParentForeColor = true,
            ParentFont = true,
        };

        private readonly CardPanelHeader cardPanelHeader = new()
        {
            ParentBackColor = true,
            ParentForeColor = true,
            ParentFont = true,
            HasInteriorBorder = false,
        };

        private bool hasInteriorBorder = true;
        private TabSizeMode sizeMode = TabSizeMode.Normal;
        private TabAppearance tabAppearance = TabAppearance.Normal;
        private int addSuspended;
        private TabAlignment? tabPaintAlignment;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public TabControl(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        public TabControl()
        {
            ParentBackColor = true;
            ParentForeColor = true;

            cardPanelHeader.UserPaint = true;
            cardPanelHeader.Paint += OnHeaderPaint;
            UserPaint = true;

            base.Layout = LayoutStyle.Vertical;
            cardPanelHeader.TabHasBorder = false;
            cardPanelHeader.BeforeTabClick += OnCardPanelHeaderBeforeTabClick;
            cardPanelHeader.TabClick += OnCardPanelHeaderTabClick;
            cardPanelHeader.ButtonSizeChanged += OnCardPanelHeaderButtonSizeChanged;
            cardPanelHeader.VerticalAlignment = UI.VerticalAlignment.Top;
            cardPanelHeader.UpdateCardsMode = WindowSizeToContentMode.None;
            cardPanelHeader.Parent = this;

            cardPanel.Margin = 1;
            cardPanel.Parent = this;
            cardPanel.VerticalAlignment = UI.VerticalAlignment.Fill;
            cardPanel.HorizontalAlignment = UI.HorizontalAlignment.Fill;

            cardPanelHeader.CardPanel = cardPanel;

            cardPanel.Children.ItemInserted += OnPagesItemInserted;
            cardPanel.Children.ItemRemoved += OnPagesItemRemoved;
        }

        /// <summary>
        /// Occurs when the close button is clicked.
        /// </summary>
        /// <remarks>This event is triggered when the user clicks the close button in the header.
        /// </remarks>
        public event EventHandler? CloseButtonClick
        {
            add => Header.CloseButtonClick += value;

            remove => Header.CloseButtonClick -= value;
        }

        /// <summary>
        /// Occurs when the size of the tab has changed.
        /// </summary>
        public event EventHandler<BaseEventArgs<AbstractControl>>? TabSizeChanged;

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

        /// <summary>
        /// Specifies flags that control the behavior of ensuring a sidebar child element.
        /// </summary>
        /// <remarks>This enumeration supports a bitwise combination of its member values.</remarks>
        [Flags]
        public enum EnsureSideBarChildFlags
        {
            /// <summary>
            /// Represents a state where no specific option or value is selected.
            /// </summary>
            None = 0,

            /// <summary>
            /// Represents an option to make an element visible.
            /// </summary>
            MakeVisible = 1 << 0,

            /// <summary>
            /// Represents an option to check the title of the element.
            /// </summary>
            CheckTitle = 1 << 1,
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.TabControl;

        /// <summary>
        /// Gets an enumerable collection of header buttons associated with the tabs in the control.
        /// </summary>
        /// <remarks>
        /// Each header button corresponds to a tab in the <see cref="TabControl"/>.
        /// </remarks>
        [Browsable(false)]
        public IEnumerable<SpeedButton> HeaderButtons
        {
            get
            {
                foreach (var item in Header.Tabs)
                {
                    yield return item.HeaderButton;
                }
            }
        }

        /// <summary>
        /// Gets the count of header buttons based on the number of tabs in the header.
        /// </summary>
        [Browsable(false)]
        public int HeaderButtonsCount
        {
            get
            {
                return Header.Tabs.Count;
            }
        }

        /// <summary>
        /// Gets the collection of tab pages in this tab control.
        /// </summary>
        /// <value>A <see cref="BaseCollection{Control}"/> that contains pages
        /// in this <see cref="TabControl"/>.</value>
        [Content]
        public BaseCollection<AbstractControl> Pages
        {
            get
            {
                return cardPanel.Children;
            }
        }

        /// <summary>
        /// Gets the collection of the loaded pages.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<AbstractControl> LoadedPages
        {
            get
            {
                return cardPanel.LoadedCards.Select(x => x.Control);
            }
        }

        /// <summary>
        /// Gets or sets colors and styles theme of the tabs.
        /// </summary>
        public virtual SpeedButton.KnownTheme TabTheme
        {
            get
            {
                if (DisposingOrDisposed)
                    return SpeedButton.KnownTheme.None;
                return Header.TabTheme;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (TabTheme == value)
                    return;
                Header.TabTheme = value;
                Invalidate();
            }
        }

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
                if (DisposingOrDisposed)
                    return -1;
                var result = Header.SelectedTabIndex;
                if (result is null)
                    return -1;
                return result.Value;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Header.SelectedTabIndex = value;
                Invalidate();
                RaiseSelectedIndexChanged();
            }
        }

        /// <summary>
        /// Gets or sets the layout relationship between the image and text content
        /// within the tab (vertical or horizontal align).
        /// </summary>
        public virtual ImageToText ImageToText
        {
            get => Header.ImageToText;

            set
            {
                if (ImageToText == value)
                    return;
                DoInsideLayout(() =>
                {
                    Header.ImageToText = value;
                });
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text should be rendered vertically.
        /// </summary>
        /// <remarks>
        /// When this property is set, the layout is refreshed to reflect the vertical text orientation.
        /// </remarks>
        public virtual bool IsVerticalText
        {
            get => Header.IsVerticalText;

            set
            {
                if (IsVerticalText == value)
                    return;

                DoInsideLayout(() =>
                {
                    Header.IsVerticalText = value;
                });
            }
        }

        /// <summary>
        /// Gets or sets whether contents of the control is visible.
        /// If contents is hidden only tab headers are shown.
        /// </summary>
        [Browsable(true)]
        public virtual bool ContentVisible
        {
            get
            {
                return cardPanel.Visible;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (ContentVisible == value)
                    return;
                cardPanel.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether tab titles are visible.
        /// </summary>
        public virtual bool TabsVisible
        {
            get
            {
                return Header.Visible;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Header.Visible = value;
            }
        }

        /// <summary>
        /// Gets selected tab page.
        /// </summary>
        [Browsable(false)]
        public virtual AbstractControl? SelectedControl
        {
            get
            {
                if (DisposingOrDisposed)
                    return null;
                return GetControlAt(SelectedIndex);
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                SelectedPage = value;
            }
        }

        /// <summary>
        ///  Gets or sets the currently selected tab page.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public virtual AbstractControl? SelectedPage
        {
            get
            {
                if (DisposingOrDisposed)
                    return null;
                var index = Header.SelectedTabIndex;
                var result = GetControlAt(index);
                return result;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
                PerformLayoutAndInvalidate();
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
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets padding of contents.
        /// </summary>
        public virtual Thickness ContentPadding
        {
            get
            {
                return Contents.Padding;
            }

            set
            {
                Contents.Padding = value;
            }
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
        /// Gets or sets alignment override used when tabs are painted.
        /// </summary>
        public virtual TabAlignment? TabPaintAlignment
        {
            get
            {
                return tabPaintAlignment;
            }

            set
            {
                if (tabPaintAlignment == value)
                    return;
                tabPaintAlignment = value;
                Header.Invalidate();
            }
        }

        /// <summary>
        /// Gets the button with X image that is shown in the header and can be configured
        /// to close (or hide) tab page (or tab control itself).
        /// </summary>
        public SpeedButton CloseButton
        {
            get
            {
                return Header.CloseButton;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the header includes a close button.
        /// </summary>
        public virtual bool HasCloseButton
        {
            get
            {
                return Header.HasCloseButton;
            }

            set
            {
                Header.HasCloseButton = value;
            }
        }

        /// <summary>
        /// Gets or sets the area of the control (for example, along the top) where
        /// the tabs are aligned.
        /// </summary>
        /// <value>One of the <see cref="TabAlignment"/> values. The default is
        /// <see cref="TabAlignment.Top"/>.</value>
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
                if (DisposingOrDisposed)
                    return;
                if (TabAlignment == value)
                    return;
                DoInsideLayout(() =>
                {
                    var isVertical = value == TabAlignment.Top || value == TabAlignment.Bottom;

                    Header.TabsAlignment = value;

                    if (isVertical)
                    {
                        Header.HorizontalAlignment = UI.HorizontalAlignment.Stretch;

                        base.Layout = LayoutStyle.Vertical;
                        Header.Layout = LayoutStyle.Horizontal;
                        if (value == TabAlignment.Bottom)
                        {
                            Header.VerticalAlignment = UI.VerticalAlignment.Bottom;
                        }
                        else
                        {
                            Header.VerticalAlignment = UI.VerticalAlignment.Top;
                        }
                    }
                    else
                    {
                        Header.VerticalAlignment = UI.VerticalAlignment.Stretch;

                        base.Layout = LayoutStyle.Horizontal;
                        Header.Layout = LayoutStyle.Vertical;
                        if (value == TabAlignment.Right)
                        {
                            Header.HorizontalAlignment = UI.HorizontalAlignment.Right;
                        }
                        else
                        {
                            Header.HorizontalAlignment = UI.HorizontalAlignment.Left;
                        }
                    }
                });
                Invalidate();
                Header.Invalidate();
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
        [Browsable(false)]
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

        /// <inheritdoc/>
        [Browsable(false)]
        public override IReadOnlyList<FrameworkElement> ContentElements => Pages;

        /// <inheritdoc/>
        [Browsable(false)]
        public override Thickness Padding
        {
            get => base.Padding;
            set
            {
            }
        }

        /// <summary>
        /// Gets inner content control with tab pages.
        /// </summary>
        [Browsable(false)]
        public AbstractControl ContentsControl => cardPanel;

        /// <summary>
        /// Gets inner header control with tab labels.
        /// </summary>
        [Browsable(false)]
        public AbstractControl HeaderControl => cardPanelHeader;

        /// <summary>
        /// Gets inner content control with tab pages.
        /// </summary>
        [Browsable(false)]
        internal CardPanel Contents => cardPanel;

        /// <summary>
        /// Gets inner header control with tab labels.
        /// </summary>
        [Browsable(false)]
        internal CardPanelHeader Header => cardPanelHeader;

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set
            {
            }
        }

        [Browsable(false)]
        internal new string? ToolTip
        {
            get => base.ToolTip;
            set
            {
            }
        }

        [Browsable(false)]
        internal new bool IsBold
        {
            get => base.IsBold;
            set
            {
            }
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        [Browsable(false)]
        internal new Font? Font
        {
            get => base.Font;
            set
            {
            }
        }

        [Browsable(false)]
        internal new Color? ForegroundColor
        {
            get => base.ForegroundColor;
            set
            {
            }
        }

        /// <summary>
        /// Gets default interior border color as light/dark color pair.
        /// </summary>
        /// <returns></returns>
        public static Color GetDefaultInteriorBorderColor()
        {
            return Color.LightDark(
                light: ColorUtils.GetTabControlInteriorBorderColor(false),
                dark: ColorUtils.GetTabControlInteriorBorderColor(true));
        }

        /// <summary>
        /// Ensures that a child control of the specified type exists within one of the tabs.
        /// </summary>
        /// <remarks>This method checks the existing collection of child controls and
        /// returns the first
        /// instance of the specified type <typeparamref name="T"/> if found.
        /// If no such instance exists, a new instance
        /// is created, optionally associated with the provided title, and added
        /// to the collection.</remarks>
        /// <typeparam name="T">The type of the child control to ensure, which must derive
        /// from <see cref="AbstractControl"/> and have a
        /// parameterless constructor.</typeparam>
        /// <param name="title">An optional title to associate with the child control
        /// if a new instance is created.</param>
        /// <param name="onCreate">The action to execute when creating the child control.</param>
        /// <param name="onUpdate">The action to execute when updating the child control.</param>
        /// <param name="flags">The flags for additional options.</param>
        /// <returns>An instance of the specified type <typeparamref name="T"/>.
        /// If a child of this type already exists, it is
        /// returned; otherwise, a new instance is created, added
        /// to the collection, and returned.</returns>
        public virtual T EnsureChild<T>(
            string? title = null,
            Action<T>? onCreate = null,
            Action<T>? onUpdate = null,
            EnsureSideBarChildFlags flags = EnsureSideBarChildFlags.None)
            where T : AbstractControl, new()
        {
            var makeVisible = flags.HasFlag(EnsureSideBarChildFlags.MakeVisible);

            foreach (var child in Pages)
            {
                if (child is not T typedChild)
                    continue;

                if (flags.HasFlag(EnsureSideBarChildFlags.CheckTitle)
                    && title is not null && child.Title != title)
                    continue;

                onUpdate?.Invoke(typedChild);
                if (makeVisible)
                    SelectedControl = typedChild;
                return typedChild;
            }

            var result = new T();
            onCreate?.Invoke(result);
            Add(title, result);
            if(title is not null)
                result.Title = title;
            if (makeVisible)
                SelectedControl = result;
            return result;
        }

        /// <summary>
        /// Adds new page.
        /// </summary>
        /// <param name="page">Page title and control.</param>
        /// <returns>
        /// Created page index.
        /// </returns>
        public virtual int Add(NameValue<AbstractControl> page)
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
        public virtual int Add(NameValue<Func<AbstractControl>> page)
        {
            return Add(page.Name, page.Value);
        }

        /// <summary>
        /// Adds new pages.
        /// </summary>
        /// <param name="pages">Collection of pages.</param>
        public virtual void AddRange(IEnumerable<NameValue<AbstractControl>> pages)
        {
            foreach(var page in pages)
                Add(page);
        }

        /// <summary>
        /// Adds new pages.
        /// </summary>
        /// <param name="pages">Collection of pages.</param>
        public virtual void AddRange(IEnumerable<NameValue<Func<AbstractControl>>?> pages)
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
        public virtual int Add(AbstractControl control)
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
        public virtual int Insert(int? index, AbstractControl control)
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
        public virtual int Insert(int? index, string? title, AbstractControl? control = null)
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
        public virtual int Add(string title, Func<AbstractControl> fnCreate)
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
        public virtual int Add(string? title, AbstractControl? control = null)
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
        /// Retrieves the header button associated with the specified tab page.
        /// </summary>
        /// <param name="control">The control for which to retrieve the header button.</param>
        /// <returns>The <see cref="SpeedButton"/> associated with the specified control,
        /// or <c>null</c> if no button is found.</returns>
        public virtual SpeedButton? GetHeaderButton(AbstractControl? control)
        {
            var index = GetTabIndex(control);
            var tab = Header.GetTab(index);
            if (tab is null)
                return null;
            return tab.HeaderButton;
        }

        /// <summary>
        /// Gets index of the tab page or <c>null</c> if control is not found in tabs.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public virtual int? GetTabIndex(AbstractControl? control)
        {
            if (control is null || DisposingOrDisposed)
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
        public virtual bool Remove(AbstractControl control)
        {
            var index = GetTabIndex(control);
            return RemoveAt(index);
        }

        /// <summary>
        /// Resets images for the tab.
        /// </summary>
        /// <param name="index">The index of the tab page.</param>
        public virtual void ResetTabImage(int? index)
        {
            var item = Header.GetTab(index);
            if (item is null)
                return;
            item.Image = null;
            item.DisabledImage = null;
            item.ImageSet = null;
            item.DisabledImageSet = null;
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
        /// Sets svg image for the tab.
        /// </summary>
        /// <param name="index">The index of the tab page.</param>
        /// <param name="svg">Tab image.</param>
        /// <param name="size">Size of the image. Optional. If not specified,
        /// defaults will be used.</param>
        /// <param name="color">Color of the svg image. Optional. If not specified,
        /// default svg color is used.</param>
        public virtual void SetTabSvg(int? index, SvgImage? svg, int? size = null, Color? color = null)
        {
            if(index is null)
                return;
            if(svg is null)
            {
                ResetTabImage(index);
                return;
            }

            size ??= UseSmallImages ? DefaultSmallSvgSize : DefaultLargeSvgSize;

            ImageSet? image;

            if (color is null)
                image = svg.AsNormal(size.Value, IsDarkBackground);
            else
                image = svg.ImageSetWithColor(size.Value, color);

            SetTabImage(index, image);
        }

        /// <summary>
        /// Retrieves the header button associated with the specified tab index.
        /// </summary>
        /// <param name="index">The zero-based index of the tab whose header button
        /// is to be retrieved.</param>
        /// <returns>The <see cref="SpeedButton"/> associated with the specified tab
        /// index, or <c>null</c> if no button is found.</returns>
        public SpeedButton? GetHeaderButtonAt(int? index)
        {
            return Header.GetTab(index)?.HeaderButton;
        }

        /// <summary>
        /// Raises the <see cref="SelectedIndexChanged"/> and <see cref="SelectedPageChanged"/>
        /// events and updates the minimum size of the control based on the current settings.
        /// </summary>
        public void RaiseSelectedIndexChanged()
        {
            if (DisposingOrDisposed)
                return;
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            SelectedPageChanged?.Invoke(this, EventArgs.Empty);
            GrowMinSize(MinSizeGrowMode);
        }

        /// <summary>
        /// Gets tab page at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual AbstractControl? GetControlAt(int? index)
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
            AbstractControl child,
            string propName,
            bool directChild = true)
        {
            if (DisposingOrDisposed)
                return;
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
            var color = Borders?.GetObjectOrNull(VisualControlState.Normal)?.Color;
            color ??= ColorUtils.GetTabControlInteriorBorderColor(IsDarkBackground);
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
            if (DisposingOrDisposed)
                return;
            base.OnPaint(e);
            if (!hasInteriorBorder || TabCount == 0 || !TabsVisible)
                return;
            var r = Header.Bounds;
            r.Size += Header.Margin.Size;
            DrawTabControlInterior(
                e.Graphics,
                ClientRectangle,
                r,
                GetInteriorBorderColor().AsBrush,
                tabPaintAlignment ?? TabAlignment);
        }

        /// <summary>
        /// Called when the size of the header button changes.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments containing information about the size change.</param>
        protected virtual void OnCardPanelHeaderButtonSizeChanged(
            object? sender,
            BaseEventArgs<AbstractControl> e)
        {
            if (DisposingOrDisposed)
                return;
            TabSizeChanged?.Invoke(this, e);
            Invalidate();
        }

        /// <summary>
        /// Called before a tab header is clicked.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments containing information about the click event.</param>
        protected virtual void OnCardPanelHeaderBeforeTabClick(object sender, BaseCancelEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            GrowMinSize(MinSizeGrowMode);
        }

        /// <summary>
        /// Called when a tab header is clicked.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments containing information about the click event.</param>
        protected virtual void OnCardPanelHeaderTabClick(object? sender, EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            RaiseSelectedIndexChanged();
        }

        /// <summary>
        /// Called when item is removed from the pages collection.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="index">The index of the item.</param>
        /// <param name="item">The item that was removed.</param>
        protected virtual void OnPagesItemRemoved(object? sender, int index, AbstractControl item)
        {
            Remove(item);
        }

        /// <summary>
        /// Called when item is inserted into the pages collection.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="index">The index of the item.</param>
        /// <param name="item">The item that was removed.</param>
        protected virtual void OnPagesItemInserted(object? sender, int index, AbstractControl item)
        {
            if (DisposingOrDisposed)
                return;
            if (addSuspended > 0)
                return;
            if (Contents.Find(item) is not null)
                return;
            Add(item);
        }

        /// <summary>
        /// Called when the header is painted.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event arguments containing information about the paint event.</param>
        protected virtual void OnHeaderPaint(object? sender, PaintEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (!hasInteriorBorder || TabCount == 0 || !TabsVisible)
                return;
            var r = e.ClientRectangle;
            if(r.Width > ClientSize.Width)
                r.Width = ClientSize.Width;
            if (r.Height > ClientSize.Height)
                r.Height = ClientSize.Height;

            DrawTabHeaderInterior(
                Header,
                e.Graphics,
                r,
                GetInteriorBorderColor().AsBrush,
                tabPaintAlignment ?? TabAlignment);
        }

        private class TabControlCardPanel : CardPanel
        {
            public bool IsVisibleInParent
            {
                get
                {
                    return ((TabControl?)Parent)?.ContentVisible ?? false;
                }
            }

            /// <inheritdoc/>
            public override bool Visible
            {
                get => base.Visible;
                set => base.Visible = value && IsVisibleInParent;
            }

            /// <inheritdoc/>
            public override void OnChildPropertyChanged(
                AbstractControl child,
                string propName,
                bool directChild = true)
            {
                if (DisposingOrDisposed)
                    return;
                Parent?.OnChildPropertyChanged(child, propName, false);
            }
        }
    }
}