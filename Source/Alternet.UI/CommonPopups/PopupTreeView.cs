using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="TreeView"/> control.
    /// </summary>
    public class PopupTreeView : PopupWindow<TreeView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupTreeView"/> class.
        /// </summary>
        public PopupTreeView()
        {
            HideOnClick = false;
        }

        /// <inheritdoc/>
        protected override TreeView CreateMainControl()
        {
            return new TreeView()
            {
                HasBorder = false,
            };
        }
    }
}
