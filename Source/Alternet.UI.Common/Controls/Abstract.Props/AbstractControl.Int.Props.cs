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
        internal static int ScreenShotCounter { get; set; } = 0;

        internal override FrameworkElement? LogicalParent
        {
            get => Parent;
            set => Parent = value as AbstractControl;
        }

        internal bool HasExtendedProps => extendedProps != null;

        internal ControlExtendedProps ExtendedProps
        {
            get
            {
                extendedProps ??= new();
                return extendedProps;
            }
        }
    }
}
