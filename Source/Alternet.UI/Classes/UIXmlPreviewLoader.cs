using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Native;

namespace Alternet.UI.Integration
{
    internal class UIXmlPreviewLoader
    {
        private static string? dllPath;

        public static string? DllPath
        {
            get => dllPath;

            set
            {
                dllPath = value;
                NativeApiProvider.Initialize();
            }
        }

        public static bool SetDllResolver { get; set; } = true;
    }
}
