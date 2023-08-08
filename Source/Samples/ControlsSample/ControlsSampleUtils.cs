using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal static partial class ControlsSampleUtils
    {
        private static bool cmdLineNoMfcDedug = false;

        public static bool CmdLineNoMfcDedug => cmdLineNoMfcDedug;

        public static void ParseCmdLine(string[] args)
        {
            for (int i = 1; i < args.Length; i++)
            {
                string text = args[i];

                Fn("-nomfcdebug", out cmdLineNoMfcDedug);

                void Fn(string strFlag, out bool boolFlag)
                {
                    boolFlag = text.ToLower() == strFlag;
                }
            }
        }
    }
}
