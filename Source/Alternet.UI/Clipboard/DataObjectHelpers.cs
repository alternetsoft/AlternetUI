using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal static class DataObjectHelpers
    {
        public static object SetDataTransform(string format, object data)
        {
            if (format.Equals(DataFormats.Files, StringComparison.Ordinal))
            {
                return data switch
                {
                    FileInfo[] x => x.Select(x => x.FullName).ToArray(),
                    _ => data
                };
            }

            return data;
        }

        public static string DetectFormatFromData(object data) => data switch
        {
            string _ => DataFormats.Text,
            FileInfo[] _ => DataFormats.Files,
            Bitmap _ => DataFormats.Bitmap,
            _ => data.GetType().FullName ?? throw new InvalidOperationException(),
        };
    }
}