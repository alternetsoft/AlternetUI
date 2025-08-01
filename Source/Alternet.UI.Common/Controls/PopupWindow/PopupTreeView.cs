using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="VirtualTreeControl"/>.
    /// </summary>
    public partial class PopupTreeView : PopupWindow<VirtualTreeControl>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupTreeView"/> class.
        /// </summary>
        public PopupTreeView()
        {
            HideOnClick = false;
            Title = CommonStrings.Default.WindowTitleSelectValue;
        }

        /// <inheritdoc/>
        protected override VirtualTreeControl CreateMainControl()
        {
            return new VirtualTreeControl()
            {
                HasBorder = false,
            };
        }
    }
}
