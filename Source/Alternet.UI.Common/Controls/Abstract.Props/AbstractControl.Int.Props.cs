using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <inheritdoc/>
        public override FrameworkElement? LogicalParent
        {
            get => Parent;
            internal set => Parent = value as AbstractControl;
        }
    }
}
