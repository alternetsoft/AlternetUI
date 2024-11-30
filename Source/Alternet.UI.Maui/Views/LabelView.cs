﻿using System;
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
    /// Implements label view on the MAUI platform using <see cref="GenericLabel"/> control.
    /// </summary>
    /// <remarks>
    /// This controls is implemented for testing purposes. It is better to use native MAUI control
    /// instead of <see cref="LabelView"/>.
    /// </remarks>
    public partial class LabelView : ControlView<GenericLabel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelView"/> class.
        /// </summary>
        public LabelView()
        {
        }
    }
}