using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alternet.Drawing
{
    public partial class Graphics
    {
        [Conditional("DEBUG")]
        private static void RunTests()
        {
        }

        private static Graphics GetMeasureCanvas()
        {
            Graphics? result = null;
            RequireMeasure(ref result, new());
            return result;
        }
    }
}
