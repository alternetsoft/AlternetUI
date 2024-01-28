﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="PanelAuiManager"/> control.
    /// </summary>
    internal class PopupAuiManager : PopupWindow
    {
        /// <summary>
        /// Gets or sets <see cref="PanelAuiManager"/> control used in the popup window.
        /// </summary>
        [Browsable(false)]
        public new PanelAuiManager MainControl
        {
            get => (PanelAuiManager)base.MainControl;
            set => base.MainControl = value;
        }

        /// <inheritdoc/>
        protected override Control CreateMainControl() => new PanelAuiManager();
    }
}
