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
        private ListBox? listBox;
        private int? resultIndex;

        /// <summary>
        /// Gets or sets <see cref="ListBox"/> control used in the popup window.
        /// </summary>
        public ListBox ListBox
        {
            get
            {
                if(listBox == null)
                {
                    listBox = (ListBox)CreateMainControl();
                    listBox.HasBorder = false;
                    listBox.Parent = this.Border;
                    BindEvents(listBox);
                }

                return listBox;
            }

            set
            {
                if (listBox == value || listBox is null)
                    return;
                UnbindEvents(listBox);
                listBox = value;
                BindEvents(listBox);
                listBox.Parent = Border;
            }
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
        /// Gets index of the selected item when popup window is closed.
        /// </summary>
        /// <remarks>
        /// Use this property instead of <see cref="ListBox.SelectedIndex"/> after popup is closed.
        /// In cases when <see cref="PopupResult"/> is <see cref="ModalResult.Accepted"/>
        /// it contains selected item index. This item index is more correct than
        /// <see cref="ListBox.SelectedIndex"/>.
        /// </remarks>
        public int? ResultIndex
        {
            get
            {
                if (resultIndex is null)
                    return ListBox.SelectedIndex;
                return resultIndex;
            }

            set
            {
                resultIndex = value;
            }
        }

        /// <inheritdoc/>
        protected override Control CreateMainControl() => new ListBox();

        /// <inheritdoc/>
        protected override void OnMainControlMouseDoubleClick(object? sender, MouseButtonEventArgs e)
        {
            UpdateResultIndex(e);
            if(resultIndex is not null)
                base.OnMainControlMouseDoubleClick(sender, e);
        }

        /// <inheritdoc/>
        protected override void OnMainControlMouseLeftButtonUp(object? sender, MouseButtonEventArgs e)
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

        private void UpdateResultIndex(MouseButtonEventArgs e)
        {
            resultIndex = ListBox.HitTest(e.GetPosition(ListBox));
        }
    }
}
