using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a generic list view control implemented inside the library.
    /// </summary>
    public partial class XListView : XTreeView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XListView"/> class.
        /// </summary>
        public XListView()
        {
            ListBox.GridLinesDisplayMode = ListViewGridLinesDisplayMode.Vertical;
            ListBox.CheckBoxVisible = false;
            IsHeaderVisible = true;
            TreeButtons = TreeViewButtonsKind.Null;
        }

        /// <summary>
        /// Gets or sets the selection mode (none, single or multiple).
        /// </summary>
        /// <remarks>The selection mode determines whether multiple items can be selected
        /// at once and how the selection behaves.
        /// For example, <see cref="ListViewSelectionMode.Single"/> allows only one item to be
        /// selected, while  <see cref="ListViewSelectionMode.Multiple"/> allows multiple
        /// items to be selected.</remarks>
        public new ListViewSelectionMode SelectionMode
        {
            get
            {
                return (ListViewSelectionMode)ListBox.SelectionMode;
            }

            set
            {
                ListBox.SelectionMode = (ListBoxSelectionMode)value;
            }
        }
    }
}
