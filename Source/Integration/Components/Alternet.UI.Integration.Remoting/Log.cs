using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI.Integration
{
    public static class Log
    {
        public static bool InformationLogged = true;
        public static bool VerboseLogged = true;

        public static Action<string> Write;

        public static void Information(string s = null)
        {
            s ??= string.Empty;
            if(InformationLogged)
                Write($"Information: {s}");
        }

        public static void Error(string s)
        {
            Write($"Error: {s}");
        }

        public static void Verbose(string s)
        {
            if(VerboseLogged)
                Write($"Verbose: {s}");
        }

        [Conditional("DEBUG")]
        public static void Debug(string s)
        {
            Write($"Debug: {s}");
        }
    }
}