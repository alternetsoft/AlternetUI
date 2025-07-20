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
        internal override FrameworkElement? LogicalParent
        {
            get => Parent;
            set => Parent = value as AbstractControl;
        }
    }
}
