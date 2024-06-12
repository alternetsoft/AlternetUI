using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.Devices;

namespace Alternet.UI
{
    public class MauiDisplayFactoryHandler : PlessDisplayFactoryHandler, IDisplayFactoryHandler
    {
        private IDisplayHandler? primaryDisplay;

        public override IDisplayHandler CreateDisplay()
        {
            return primaryDisplay ??= new MauiDisplayHandler();
        }
    }
}
