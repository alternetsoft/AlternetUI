using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class MauiSystemSettingsHandler : PlessSystemSettingsHandler, ISystemSettingsHandler
    {
        public override IDisplayFactoryHandler CreateDisplayFactoryHandler()
        {
            return new MauiDisplayFactoryHandler();
        }

        public override UIPlatformKind GetPlatformKind()
        {
            return UIPlatformKind.Maui;
        }
    }
}
