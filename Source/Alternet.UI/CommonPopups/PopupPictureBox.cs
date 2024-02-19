﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="PictureBox"/> control.
    /// </summary>
    public class PopupPictureBox : PopupWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupPictureBox"/> class.
        /// </summary>
        public PopupPictureBox()
        {
            ShowOkButton = false;
        }

        /// <summary>
        /// Gets or sets <see cref="PictureBox"/> control used in the popup window.
        /// </summary>
        [Browsable(false)]
        public new PictureBox MainControl
        {
            get => (PictureBox)base.MainControl;
            set => base.MainControl = value;
        }

        /// <inheritdoc/>
        protected override Control CreateMainControl() => new PictureBox();
    }
}
