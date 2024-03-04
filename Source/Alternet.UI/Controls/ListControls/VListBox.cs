using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Implements virtual ListBox control. This is experimental and will be changed at any time.
    /// </summary>
    public class VListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VListBox"/> class.
        /// </summary>
        public VListBox()
        {
            SuggestedSize = 200;
        }

        /// <inheritdoc/>
        public override bool CanUserPaint
        {
            get => false;
        }

        /// <summary>
        /// Gets or sets number of items in the control.
        /// </summary>
        public new int Count
        {
            get
            {
                return base.Count;
            }

            set
            {
                ((VListBoxHandler)Handler).NativeControl.ItemsCount = value;
            }
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return new VListBoxHandler();
        }
    }
}
