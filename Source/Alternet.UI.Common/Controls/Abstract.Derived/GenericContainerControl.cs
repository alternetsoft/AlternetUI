using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for all generic container controls.
    /// </summary>
    public partial class GenericContainerControl : HiddenGenericBorder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericContainerControl"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public GenericContainerControl(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericContainerControl"/> class.
        /// </summary>
        public GenericContainerControl()
        {
            TabStop = false;
            CanSelect = false;
            ParentBackColor = true;
            ParentForeColor = true;
        }
    }
}
