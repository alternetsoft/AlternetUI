using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="VirtualListBox"/> control.
    /// </summary>
    public partial class PopupListBox : PopupListBox<VirtualListBox>
    {
        private static PopupListBox? defaultInstance;

        /// <summary>
        /// Gets or sets the default instance of <see cref="PopupListBox"/>.
        /// </summary>
        public static new PopupListBox Default
        {
            get => defaultInstance ??= new PopupListBox();
            set => defaultInstance = value;
        }
    }
}
