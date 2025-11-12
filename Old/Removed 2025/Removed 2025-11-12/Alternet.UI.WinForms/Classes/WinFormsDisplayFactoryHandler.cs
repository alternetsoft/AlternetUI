using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI.WinForms
{
    internal partial class WinFormsDisplayFactoryHandler
        : PlessDisplayFactoryHandler, IDisplayFactoryHandler
    {
        private IDisplayHandler? primaryDisplay;

        public override IDisplayHandler CreateDisplay()
        {
            return primaryDisplay ??= new WinFormsDisplayHandler();
        }
    }
}
