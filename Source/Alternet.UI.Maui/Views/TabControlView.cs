using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Alternet.UI;

using Microsoft.Maui.Dispatching;

namespace Alternet.UI
{
    /// <summary>
    /// Implements tab control view on the MAUI platform using <see cref="TabControl"/> control.
    /// </summary>
    /// <remarks>
    /// This controls is implemented for testing purposes. It is better to use native MAUI control
    /// instead of <see cref="TabControlView"/>.
    /// </remarks>
    public partial class TabControlView : ControlView<TabControl>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabControlView"/> class.
        /// </summary>
        public TabControlView()
        {
        }
    }
}