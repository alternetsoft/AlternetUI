using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public static class SvgToPngUtils
    {
        public static void ConvertSvgToPng(string configFile, SvgToPngSettings settings, SvgToPngSetting item)
        {
            var basePath = Path.GetDirectoryName(configFile);

            if (item.Filename is null || basePath is null)
                return;

            var fileToConvert = Path.GetFullPath(item.Filename, basePath);
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileToConvert);
            var filePath = Path.GetDirectoryName(fileToConvert);

            if (!File.Exists(fileToConvert) || filePath is null)
            {
                Console.WriteLine($"File '{fileToConvert}' not found");
                return;
            }

            Console.WriteLine($"Converting '{fileToConvert}'");

            var svgImage = new MonoSvgImage(fileToConvert);

            var light16 = svgImage.AsNormalImage(16, isDark: false);
            var light32 = svgImage.AsNormalImage(32, isDark: false);
            var dark16 = svgImage.AsNormalImage(16, isDark: true);
            var dark32 = svgImage.AsNormalImage(32, isDark: true);
            var light16Name = fileNameWithoutExt + "16";
            var light32Name = fileNameWithoutExt + "32";
            var dark16Name = fileNameWithoutExt + "16.Dark";
            var dark32Name = fileNameWithoutExt + "32.Dark";

            light16Name += ".png";
            light32Name += ".png";
            dark16Name += ".png";
            dark32Name += ".png";

            light16Name = Path.Combine(filePath, light16Name);
            light32Name = Path.Combine(filePath, light32Name);
            dark16Name = Path.Combine(filePath, dark16Name);
            dark32Name = Path.Combine(filePath, dark32Name);
        }

        public static void ConvertSvgToPng(string configFile)
        {
            try
            {
                var settings = XmlUtils.DeserializeFromFile<SvgToPngSettings>(configFile);

                if (settings is null)
                {
                    Console.WriteLine($"Settings file reading error");
                    return;
                }

                foreach (var item in settings.Items)
                {
                    ConvertSvgToPng(configFile, settings, item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error replace in files");
                Console.WriteLine($"Error info logged to file");
                LogUtils.LogExceptionToFile(e);
            }
        }
    }
}
