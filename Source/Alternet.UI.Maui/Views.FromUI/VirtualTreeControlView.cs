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

namespace Alternet.Maui
{
    /// <summary>
    /// Implements tree view on the MAUI platform using internal
    /// <see cref="StdTreeView"/> control.
    /// </summary>
    public partial class VirtualTreeControlView : ControlView<StdTreeView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeControlView"/> class.
        /// </summary>
        public VirtualTreeControlView()
        {
        }
    }
}