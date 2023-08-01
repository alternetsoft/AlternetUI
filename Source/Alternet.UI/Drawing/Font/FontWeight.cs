using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    [Flags]
    public enum FontWeight
    {
        Invalid = 0,

        Thin = 100,

        ExtraLight = 200,

        Light = 300,

        Normal = 400,

        Medium = 500,

        SemiBold = 600,

        Bold = 700,

        ExtraBold = 800,

        Heavy = 900,

        ExtraHeavy = 1000,

        Max = ExtraHeavy,
    }
}
