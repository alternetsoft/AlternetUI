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
    }
}
