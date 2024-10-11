using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Parent class for all template controls.
    /// </summary>
    public class TemplateControl : Border
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateControl"/> class.
        /// </summary>
        public TemplateControl()
        {
            Visible = false;
            HasBorder = false;
        }
    }
}
