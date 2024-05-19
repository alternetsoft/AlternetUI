using System;
using System.Collections.Generic;
using System.ComponentModel;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that manages a related set of tab pages.
    /// </summary>
    [DefaultProperty("Pages")]
    [DefaultEvent("SelectedPageChanged")]
    [ControlCategory("Containers")]
    internal partial class WxTabControl : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WxTabControl"/> class.
        /// </summary>
        public WxTabControl()
        {
            if (Application.IsWindowsOS)
                UserPaint = true;
            Children.ItemInserted += Children_ItemInserted;
        }

        /// <summary>
        /// Occurs when the <see cref="SelectedPage"/> property has changed.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? SelectedPageChanged;

        /// <summary>
        /// Occurs when new page was added.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? PageAdded;

        /// <summary>
        /// Gets or sets the area of the control (for example, along the top) where
        /// the tabs are aligned.
        /// </summary>
        /// <value>One of the <see cref="TabAlignment"/> values. The default is
        /// <see cref="TabAlignment.Top"/>.</value>
        public TabAlignment TabAlignment
        {
            get => Handler.TabAlignment;
            set
            {
               // Disabled Left and Right as under Windows they are buggy
                if (value == TabAlignment.Left || value == TabAlignment.Right)
                    return;
                Handler.TabAlignment = value;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.TabControl;

        /// <summary>
        /// Gets the collection of tab pages in this tab control.
        /// </summary>
        /// <value>A <see cref="Collection{TabPage}"/> that contains the <see cref="TabPage"/>
        /// objects in this <see cref="WxTabControl"/>.</value>
        /// <remarks>The order of tab pages in this collection reflects the order the tabs appear
        /// in the control.</remarks>
        [Content]
        public Collection<TabPage> Pages { get; } = new();

        /// <inheritdoc/>
        public override IReadOnlyList<FrameworkElement> ContentElements => Pages;

        /// <summary>
        ///  Gets or sets the currently selected tab page.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public TabPage? SelectedPage
        {
            get
            {
                var pageIndex = SelectedPageIndex;
                if (pageIndex is null)
                    return null;
                return Pages[pageIndex.Value];
            }

            set
            {
                var selectedPage = SelectedPage;
                if (selectedPage == value)
                    return;
                var index = selectedPage == null
                    ? -1 : Pages.IndexOf(selectedPage);
                SelectedPageIndex = index;
            }
        }

        /// <summary>
        ///  Gets the currently selected tab page index.
        /// </summary>
        [Browsable(false)]
        public int? SelectedPageIndex
        {
            get
            {
                var result = NativeControl.SelectedPageIndex;
                if (result < 0 || result >= Pages.Count)
                    return null;
                return result;
            }

            set
            {
                value ??= -1;
                NativeControl.SelectedPageIndex = value.Value;
            }
        }

        /// <summary>
        /// Gets a <see cref="WxTabControlHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        internal new WxTabControlHandler Handler
        {
            get
            {
                CheckDisposed();
                return (WxTabControlHandler)base.Handler;
            }
        }

        internal Native.TabControl NativeControl => (Native.TabControl)Handler.NativeControl;

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection => Pages;

        /// <inheritdoc />
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            return NativeControl.GetTotalPreferredSizeFromPageSize(
                GetSpecifiedOrChildrenPreferredSize(availableSize));
        }

        /// <inheritdoc />
        public override void OnLayout()
        {
            Handler.LayoutSelectedPage();
        }

        internal void RaiseSelectedPageChanged(EventArgs e)
        {
            OnSelectedPageChanged(e);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return new WxTabControlHandler();
        }

        /// <summary>
        /// A virtual function that is called when the selection is changed. Default behavior
        /// is to raise a SelectedPageChangedEvent
        /// </summary>
        /// <param name="e">The inputs for this event. Can be raised (default behavior) or
        /// processed in some other way.</param>
        protected virtual void OnSelectedPageChanged(EventArgs e)
        {
            SelectedPageChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
        }

        private void Children_ItemInserted(object? sender, int index, Control item)
        {
            PageAdded?.Invoke(this, EventArgs.Empty);
        }
    }
}