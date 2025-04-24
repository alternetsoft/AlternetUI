﻿using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="TreeControlItem"/> with horizontal separator painting.
    /// </summary>
    public class TreeControlSeparatorItem : TreeControlItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlSeparatorItem"/> class.
        /// </summary>
        public TreeControlSeparatorItem()
        {
            this.HideSelection = true;
            ForegroundColor = ListControlSeparatorItem.DefaultSeparatorColor ?? DefaultColors.BorderColor;
            DrawForegroundAction = ListControlSeparatorItem.DefaultDrawSeparator;
            DrawBackgroundAction = (s, e) => { };
        }
    }
}
