using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for all non visual controls like <see cref="Menu"/>,
    /// <see cref="StatusBar"/> and other.
    /// </summary>
    public class NonVisualControl : Control
    {
        /// <inheritdoc cref="Control.ToolTip"/>
        [Browsable(false)]
        public new string? ToolTip { get => default; }
    }
}
