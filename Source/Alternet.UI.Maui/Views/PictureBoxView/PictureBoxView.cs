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
    /// Implements picture box view on the MAUI platform using <see cref="PictureBox"/> control.
    /// </summary>
    [Experimental("MAUI0001")]
    public partial class PictureBoxView : ControlView<PictureBox>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PictureBoxView"/> class.
        /// </summary>
        public PictureBoxView()
        {
        }
    }
}