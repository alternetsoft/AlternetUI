using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI.Tests
{
    /// <summary>
    /// Contains different test methods. This is for internal use only.
    /// </summary>
    public static class Tests
    {
        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
        [Conditional("DEBUG")]
        public static void TestParseTextWithIndexAccel()
        {
            var s1 = "Text1 with acell [2]";
            var s2 = "Text2 with acell [-5]";

            var splitted1 = StringUtils.ParseTextWithIndexAccel(s1, 2, FontStyle.Bold);
            var splitted2 = StringUtils.ParseTextWithIndexAccel(s2, -5, FontStyle.Bold);

            LogUtils.LogTextAndFontStyle(splitted1);
            LogUtils.LogTextAndFontStyle(splitted2);
        }

        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
        [Conditional("DEBUG")]
        public static void TestEnumArray()
        {
            EnumArray<VisualControlState, int> data = new();

            data[VisualControlState.Disabled] = 5;

            var result = data[VisualControlState.Disabled];

            App.Log($"EnumArray: 5 => {result}");
        }

        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
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
