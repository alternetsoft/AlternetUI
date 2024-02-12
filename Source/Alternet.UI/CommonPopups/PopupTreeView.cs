﻿using System;
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
    public class PopupTreeView : PopupWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupTreeView"/> class.
        /// </summary>
        public PopupTreeView()
        {
            HideOnClick = false;
        }

        /// <summary>
        /// Gets or sets <see cref="TreeView"/> control used in the popup window.
        /// </summary>
        [Browsable(false)]
        public new TreeView MainControl
        {
            get => (TreeView)base.MainControl;
            set => base.MainControl = value;
        }

        /// <inheritdoc/>
        protected override Control CreateMainControl()
        {
            return new TreeView()
            {
                HasBorder = false,
            };
        }
    }
}
