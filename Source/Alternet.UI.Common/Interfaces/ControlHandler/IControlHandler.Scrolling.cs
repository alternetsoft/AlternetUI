using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial interface IControlHandler
    {
        /// <inheritdoc cref="AbstractControl.VertScrollBarInfo"/>
        ScrollBarInfo VertScrollBarInfo { get; set; }

        /// <inheritdoc cref="AbstractControl.HorzScrollBarInfo"/>
        ScrollBarInfo HorzScrollBarInfo { get; set; }

        /// <inheritdoc cref="AbstractControl.IsScrollable"/>
        bool IsScrollable { get; set; }
    }
}
