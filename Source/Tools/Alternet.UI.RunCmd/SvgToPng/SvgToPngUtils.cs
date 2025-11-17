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

            Color lightColor = item.LightColor ?? Color.FromArgb(255, 0, 0, 0);
            Color darkColor = item.DarkColor ?? Color.FromArgb(255, 0xE6, 0xE6, 0xE6);

            if (lightColor != Color.FromArgb(255, 0, 0, 0))
            {
            }

            var light16 = svgImage.ImageWithColor(16, lightColor);
            var light32 = svgImage.ImageWithColor(32, lightColor);
            var dark16 = svgImage.ImageWithColor(16, darkColor);
            var dark32 = svgImage.ImageWithColor(32, darkColor);

            item.Light16 = light16;
            item.Light32 = light32;
            item.Dark16 = dark16;
            item.Dark32 = dark32;

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

            File.Delete(light16Name);
            File.Delete(light32Name);
            File.Delete(dark16Name);
            File.Delete(dark32Name);

            light16?.Save(light16Name);
            light32?.Save(light32Name);
            dark16?.Save(dark16Name);
            dark32?.Save(dark32Name);
        }

        public static void ConvertSvgToPng(string configFile)
        {
            try
            {
                var basePath = Path.GetDirectoryName(configFile);
                var settings = XmlUtils.DeserializeFromFile<SvgToPngSettings>(configFile);

                if (settings is null)
                {
                    Console.WriteLine($"Settings file reading error");
                    return;
                }

                var darkStripName16 = settings.DarkStripName16 ?? "Images.16.Dark.png";
                var darkStripName32 = settings.DarkStripName32 ?? "Images.32.Dark.png";
                var lightStripName16 = settings.LightStripName16 ?? "Images.16.png";
                var lightStripName32 = settings.LightStripName32 ?? "Images.32.png";

                var light16Images = new ImageList(16);
                var light32Images = new ImageList(32);
                var dark16Images = new ImageList(16);
                var dark32Images = new ImageList(32);

                foreach (var item in settings.Items)
                {
                    ConvertSvgToPng(configFile, settings, item);
                    light16Images.Add(item.Light16);
                    light32Images.Add(item.Light32);
                    dark16Images.Add(item.Dark16);
                    dark32Images.Add(item.Dark32);
                }

                var light16strip = light16Images.AsSkiaStrip();
                var light32strip = light32Images.AsSkiaStrip();
                var dark16strip = dark16Images.AsSkiaStrip();
                var dark32strip = dark32Images.AsSkiaStrip();

                lightStripName16 = Path.GetFullPath(lightStripName16!, basePath!);
                lightStripName32 = Path.GetFullPath(lightStripName32!, basePath!);
                darkStripName16 = Path.GetFullPath(darkStripName16!, basePath!);
                darkStripName32 = Path.GetFullPath(darkStripName32!, basePath!);

                SkiaUtils.SaveBitmapToPng(light16strip!, lightStripName16!);
                SkiaUtils.SaveBitmapToPng(light32strip!, lightStripName32!);
                SkiaUtils.SaveBitmapToPng(dark16strip!, darkStripName16!);
                SkiaUtils.SaveBitmapToPng(dark32strip!, darkStripName32!);
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
