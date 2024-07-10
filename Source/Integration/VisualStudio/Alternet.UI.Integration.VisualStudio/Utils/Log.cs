using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Integration.VisualStudio
{
    public static class Log
    {
        public static void Information(string s)
        {
            System.Diagnostics.Debug.WriteLine($"Information: {s}");
        }

        public static void Error(string s)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {s}");
        }

        public static void Verbose(string s)
        {
            System.Diagnostics.Debug.WriteLine($"Verbose: {s}");
        }

        [Conditional("DEBUG")]
        public static void Debug(string s)
        {
            System.Diagnostics.Debug.WriteLine($"Debug: {s}");
        }
    }
}
