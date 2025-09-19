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

        [Conditional("DEBUG")]
        private static void ProfileCharSize()
        {
            var m = GetMeasureCanvas();

            Stopwatch sw1 = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                for (char ch = ' '; ch < 255; ch++)
                {
                    m.GetTextExtent(new(ch, 5), Font.Default);
                }
            }

            sw1.Stop();
            Debug.WriteLine($"Default: {sw1.Elapsed} ms");

            var sw2 = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                for (char ch = ' '; ch < 255; ch++)
                    m.CharSize(ch, 5, Font.Default);
            }

            sw2.Stop();
            Debug.WriteLine($"Optimized: {sw2.Elapsed} ms");

            if (sw1.Elapsed.TotalMilliseconds > 0)
            {
                double percent = (sw2.Elapsed.TotalMilliseconds / sw1.Elapsed.TotalMilliseconds) * 100.0;
                Debug.WriteLine($"Optimized is {percent:F2}% of Default time.");
            }
        }
    }
}
