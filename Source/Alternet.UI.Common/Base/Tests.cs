using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public static void TestRichToolTipImage()
        {
            TemplateControls.RichToolTipTemplate template = new();
            RichToolTipParams data = new();
            data.Icon = MessageBoxIcon.Information;
            data.Text = "This is tooltip text";
            data.Title = "This is title";
            var image = RichToolTip.CreateToolTipImage(template, data);
            LogUtils.LogImage(image);
        }

        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
        [Conditional("DEBUG")]
        public static void TestLogHoveredControl()
        {
            AbstractControl.HoveredControlChanged -= LogHoveredControl;
            AbstractControl.HoveredControlChanged += LogHoveredControl;

            void LogHoveredControl(object? sender, EventArgs e)
            {
                var prefix = "Hovered control: ";
                App.DebugLogReplace(
                    $"{prefix}{AbstractControl.HoveredControl?.GetType().Name ?? "null"}", prefix);
            }
        }

        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
        [Conditional("DEBUG")]
        public static void TestTransformPoint()
        {
            TransformMatrix matrix = TransformMatrix.CreateTranslation(-50, 0);
            PointD point = new(50, 25);
            var transformedPoint = matrix.TransformPoint(point);
            App.Log($"Point transformed: {point} -> {transformedPoint}");
        }

        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
        [Conditional("DEBUG")]
        [Browsable(false)]
        public static void TestVariant2()
        {
            Random rnd = new();
            bool useRandom = false;

            int fn()
            {
                if(useRandom)
                    return rnd.Next();
                return 5;
            }

            int rowCount = 50000;
            int colCount = 1000;

            App.Log($"=== Test arrays [{rowCount}, {colCount}]");

            int[,] intArray = new int[rowCount, colCount];
            object[,] objArray = new object[rowCount, colCount];
            PlessVariant[,] variantArray = new PlessVariant[rowCount, colCount];

            var watch = new System.Diagnostics.Stopwatch();

            App.Log("=== Beg SetAsInt");
            watch.Start();
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    int num = fn();
                    intArray[i, j] = num;
                }
            }

            watch.Stop();
            App.Log($"Execution Time: {watch.ElapsedMilliseconds} ms");
            App.Log("=== End");

            App.Log("=== Beg PlessVariant");
            watch.Reset();
            watch.Start();
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    int num = fn();
                    variantArray[i, j] = num;
                }
            }

            watch.Stop();
            App.Log($"Execution Time: {watch.ElapsedMilliseconds} ms");
            App.Log("=== End");

            App.Log("=== Beg SetAsObject Std");
            watch.Reset();
            watch.Start();
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    int num = fn();
                    objArray[i, j] = num;
                }
            }

            watch.Stop();
            App.Log($"Execution Time: {watch.ElapsedMilliseconds} ms");
            App.Log("=== End");
        }

        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
        [Conditional("DEBUG")]
        [Browsable(false)]
        public static void TestVariant1()
        {
            byte a1 = 15;
            bool a2 = true;
            char a3 = 'a';
            sbyte a4 = 5;
            short a5 = -15;
            ushort a6 = 25;
            int a7 = -30;
            uint a8 = 868;
            long a9 = -3000;
            ulong a10 = 55552;
            float a11 = 5.2f;
            double a12 = 5555.15d;
            decimal a13 = decimal.MaxValue;
            DateTime a14 = DateTime.Now;
            string a15 = "BassBassBass";

            PlessVariant v1 = a1;
            PlessVariant v2 = a2;
            PlessVariant v3 = a3;
            PlessVariant v4 = a4;
            PlessVariant v5 = a5;
            PlessVariant v6 = a6;
            PlessVariant v7 = a7;
            PlessVariant v8 = a8;
            PlessVariant v9 = a9;
            PlessVariant v10 = a10;
            PlessVariant v11 = a11;
            PlessVariant v12 = a12;
            PlessVariant v13 = a13;
            PlessVariant v14 = a14;
            PlessVariant v15 = a15;

            void test(object o, PlessVariant v)
            {
                App.Log(o.GetType().ToString() + " " + o.ToString() + " " + v.ToString());
            }

            test(a1, v1);
            test(a2, v2);
            test(a3, v3);
            test(a4, v4);
            test(a5, v5);
            test(a6, v6);
            test(a7, v7);
            test(a8, v8);
            test(a9, v9);
            test(a10, v10);
            test(a11, v11);
            test(a12, v12);
            test(a13, v13);
            test(a14, v14);
            test(a15, v15);
        }

        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
        [Conditional("DEBUG")]
        [Browsable(false)]
        public static void TestParseTextWithIndexAccel()
        {
            var s1 = "Text1 with underline [2]";
            var s2 = "Text2 with underline [-5]";

            var split1 = StringUtils.ParseTextWithIndexAccel(s1, 2, FontStyle.Bold);
            var split2 = StringUtils.ParseTextWithIndexAccel(s2, -5, FontStyle.Bold);

            LogUtils.LogTextAndFontStyle(split1);
            LogUtils.LogTextAndFontStyle(split2);
        }

        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
        [Conditional("DEBUG")]
        [Browsable(false)]
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
        public static void TestExceptionInBackgroundTask()
        {
            App.AddBackgroundAction(() =>
            {
                throw new Exception("This is test exception");
            });
        }

        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
        [Conditional("DEBUG")]
        [Browsable(false)]
        public static void TestBitArray64()
        {
            BitArray64 value = new();
            value[1] = true;
            value[3] = true;
            value[63] = true;

            App.Log(value);
        }

        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
        [Conditional("DEBUG")]
        [Browsable(true)]
        public static void TestLogListBoxItemWithChilds()
        {
            TreeViewItem item = new("This is log item with children");
            item.Add(new("Child 1"));
            item.Add(new("Child 2"));
            item.Add(new("Child 3"));

            App.AddLogItem(item);
        }
    }
}
