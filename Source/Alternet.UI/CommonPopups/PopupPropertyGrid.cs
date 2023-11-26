using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="PropertyGrid"/> control.
    /// </summary>
    public class PopupPropertyGrid : PopupWindow
    {
        /// <summary>
        /// Gets or sets <see cref="ListBox"/> control used in the popup window.
        /// </summary>
        [Browsable(false)]
        public new PropertyGrid MainControl
        {
            get => (PropertyGrid)base.MainControl;
            set => base.MainControl = value;
        }

        /// <inheritdoc/>
        public override void HidePopup(ModalResult result)
        {
            if (!MainControl.ClearSelection(true))
                return;
            base.HidePopup(result);
        }

        /// <inheritdoc/>
        protected override Control CreateMainControl()
        {
            return new PropertyGrid()
            {
                HasBorder = false,
            };
        }
    }
}
