using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements properties and methods related to focus change.
    /// </summary>
    public interface IFocusable
    {
        /// <inheritdoc cref="Control.CanAcceptFocus"/>
        bool CanAcceptFocus { get; }

        /// <inheritdoc cref="Control.SetFocus"/>
        bool SetFocus();
    }
}
