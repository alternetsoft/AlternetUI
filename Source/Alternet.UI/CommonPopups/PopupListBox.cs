using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="ListBox"/> control.
    /// </summary>
    public class PopupListBox : PopupWindow
    {
        private int? resultIndex;

        /// <summary>
        /// Gets or sets <see cref="ListBox"/> control used in the popup window.
        /// </summary>
        [Browsable(false)]
        public new ListBox MainControl
        {
            get => (ListBox)base.MainControl;
            set => base.MainControl = value;
        }

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
        /// Use this property instead of <see cref="ListBox.SelectedItem"/> after popup is closed.
        /// In cases when <see cref="PopupResult"/> is <see cref="ModalResult.Accepted"/>
        /// it contains selected item.
        /// </remarks>
        public object? ResultItem
        {
            get
            {
                if (ResultIndex is null || ResultIndex >= MainControl.Count)
                    return null;
                return MainControl[ResultIndex.Value];
            }
        }

        /// <summary>
        /// Gets index of the selected item when popup window is closed.
        /// </summary>
        /// <remarks>
        /// Use this property instead of <see cref="ListBox.SelectedIndex"/> after popup is closed.
        /// In cases when <see cref="PopupResult"/> is <see cref="ModalResult.Accepted"/>
        /// it contains selected item index. This item index is more correct than
        /// <see cref="ListBox.SelectedIndex"/>.
        /// </remarks>
        [Browsable(false)]
        public int? ResultIndex
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
        protected override Control CreateMainControl()
        {
            return new ListBox()
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

        /// <inheritdoc/>
        protected override void BindEvents(Control control)
        {
            base.BindEvents(control);
        }

        /// <inheritdoc/>
        protected override void UnbindEvents(Control control)
        {
            base.UnbindEvents(control);
        }

        private void UpdateResultIndex(MouseEventArgs e)
        {
            resultIndex = MainControl.HitTest(e.GetPosition(MainControl));
        }
    }
}
