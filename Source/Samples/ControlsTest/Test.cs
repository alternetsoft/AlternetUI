using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsTest
{
    internal class Test
    {
        public static void DoTests()
        {
        }

        public static void LogManifestResourceNames()
        {
            var assembly = typeof(Test).Assembly;

            var resources = assembly?.GetManifestResourceNames();

            if (resources is null)
                return;

            foreach(var item in resources)
            {
                LogUtils.LogToFile(item);
            }
        }
    }
}
