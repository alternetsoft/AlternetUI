using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with tooltips.
    /// </summary>
    public interface IToolTipProvider
    {
        /// <summary>
        /// Gets <see cref="IRichToolTip"/> provider which allows to show, hide and customize
        /// tooltips.
        /// </summary>
        /// <param name="sender">Object which requires the tooltip.
        /// It can be <see cref="Control"/> descendant or any other object.
        /// </param>
        /// <returns></returns>
        IRichToolTip? Get(object? sender);
    }
}