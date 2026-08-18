using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with list box control.
    /// </summary>
    /// <typeparam name="T">Type of the list box control</typeparam>
    public partial class PopupListBox<T> : PopupWindow<T>
        where T : VirtualListBox, new()
    {
        private int? resultIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupListBox{T}"/> class.
        /// </summary>
        public PopupListBox()
        {
            InitPopup();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupListBox{T}"/> class with specified parameters.
        /// </summary>
        public PopupListBox(object? initialSettings)
            : base(initialSettings)
        {
            InitPopup();
        }

        /// <summary>
        /// Gets or sets default minimum size of the list box popup main control.
        /// </summary>
        public static SizeD DefaultMinimumSize { get; set; } = (200, 300);

        /// <summary>
        /// Gets or sets a value indicating whether the list box popup should
        /// automatically scroll to the selected item when resized.
        /// </summary>
        public virtual bool AutoScrollOnResize { get; set; } = false;

        /// <inheritdoc/>
        [Browsable(false)]
        public override ModalResult PopupResult
        {
            get
            {
                return base.PopupResult;
            }

            set
            {
                if (value == ModalResult.None)
                    resultIndex = null;
                base.PopupResult = value;
            }
        }

        /// <summary>
        /// Gets selected item when popup window is closed.
        /// </summary>
        /// <remarks>
        /// Use this property instead of <see cref="VirtualListControl.SelectedItem"/>
        /// after popup is closed.
        /// In cases when <see cref="PopupResult"/> is <see cref="ModalResult.Accepted"/>
        /// it contains selected item.
        /// </remarks>
        public virtual ListControlItem? ResultItem
        {
            get
            {
                if (ResultIndex is null || ResultIndex >= MainControl.Count)
                    return null;
                return MainControl[ResultIndex.Value];
            }
        }

        /// <inheritdoc/>
        protected override void AfterShowPopup()
        {
            base.AfterShowPopup();
            ScrollToSelectedRow();
        }

        /// <summary>
        /// Scrolls list box to the selected item
        /// </summary>
        /// <param name="postAction">Whether to post the action to the message queue.</param>
        public virtual void ScrollToSelectedRow(bool postAction = true)
        {
            if(postAction)
                Post(() => Internal());
            else
                Internal();

            void Internal()
            {
                if (MainControl.SelectedIndex is not null)
                    MainControl.ScrollToRow(MainControl.SelectedIndex.Value);
            }
        }

        /// <summary>
        /// Gets index of the selected item when popup window is closed.
        /// </summary>
        /// <remarks>
        /// Use this property instead of <see cref="VirtualListControl.SelectedIndex"/>
        /// after popup is closed.
        /// In cases when <see cref="PopupResult"/> is <see cref="ModalResult.Accepted"/>
        /// it contains selected item index. This item index is more correct than
        /// <see cref="VirtualListControl.SelectedIndex"/>.
        /// </remarks>
        [Browsable(false)]
        public virtual int? ResultIndex
        {
            get
            {
                if (resultIndex is null)
                    return MainControl.SelectedIndex;
                return resultIndex;
            }

            set
            {
                resultIndex = value;
            }
        }

        /// <inheritdoc/>
        protected override T CreateMainControl()
        {
            return new T()
            {
                HasBorder = false,
            };
        }

        /// <inheritdoc/>
        protected override void OnMainControlMouseDoubleClick(object? sender, MouseEventArgs e)
        {
            UpdateResultIndex(e);
            if(resultIndex is not null)
                base.OnMainControlMouseDoubleClick(sender, e);
        }

        /// <inheritdoc/>
        protected override void OnMainControlMouseLeftButtonUp(object? sender, MouseEventArgs e)
        {
            UpdateResultIndex(e);
            if (resultIndex is not null)
                base.OnMainControlMouseLeftButtonUp(sender, e);
        }

        /// <summary>
        /// Called from the constructor to initialize the popup window.
        /// </summary>
        protected virtual void InitPopup()
        {
            MinimumSize = DefaultMinimumSize;
            Title = CommonStrings.Default.WindowTitleSelectValue;

            MainControl.SizeChanged += (s, e) =>
            {
                if (AutoScrollOnResize)
                    ScrollToSelectedRow();
            };
        }

        /// <inheritdoc/>
        protected override void BindEvents(AbstractControl? control)
        {
            base.BindEvents(control);
        }

        /// <inheritdoc/>
        protected override void UnbindEvents(AbstractControl? control)
        {
            base.UnbindEvents(control);
        }

        private void UpdateResultIndex(MouseEventArgs e)
        {
            resultIndex = MainControl.HitTest(Mouse.GetPosition(MainControl));
        }
    }
}
