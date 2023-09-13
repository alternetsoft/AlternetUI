using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to Svg image handling.
    /// </summary>
    public static class SvgUtils
    {
        private const string ResTemplate =
            "embres:Alternet.UI.Resources.Svg.{0}.svg?assembly=Alternet.UI";

        public static string UrlImagePlus { get; set; } =
            string.Format(ResTemplate, "plus");

        public static string UrlImageMinus { get; set; } =
            string.Format(ResTemplate, "minus");

        public static ImageSet GetToolbarImageSet(string url, Control control)
        {
            var image = GetToolbarImage(url, control);
            var result = new ImageSet(image);
            return result;
        }

        public static Image GetToolbarImage(string url, Control control)
        {
            Size deviceDpi = control.GetDPI();
            var width = Toolbar.GetDefaultImageSize(deviceDpi.Width);
            var height = Toolbar.GetDefaultImageSize(deviceDpi.Height);
            var result = Image.FromSvgUrl(url, width, height);
            return result;
        }
    }
}
