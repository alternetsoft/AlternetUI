using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Root control for the templates.
    /// </summary>
    public class TemplateControl : HiddenBorder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateControl"/> class.
        /// </summary>
        public TemplateControl()
        {
            Visible = false;
        }

        /// <inheritdoc/>
        public override AbstractControl? Parent
        {
            get => base.Parent;

            set
            {
                base.Parent = value;
            }
        }

        /// <inheritdoc/>
        protected override Thickness MinPadding => Thickness.One;
    }
}
