using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alternet.UI.Tests
{
    internal static partial class Tests
    {
        [Conditional("DEBUG")]
        public static void TestBitArray64()
        {
            BitArray64 value = new();
            value[1] = true;
            value[3] = true;
            value[63] = true;

            App.Log(value);
        }
    }
}
