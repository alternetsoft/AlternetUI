using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains urls for color svg images that are included in Alternet.UI.dll resources.
    /// </summary>
    public static class KnownColorSvgUrls
    {
        private const string ResTemplate =
            "embres:Alternet.UI.Common.Resources.ColorSvg.{0}.svg?assembly=Alternet.UI.Common";

        /// <summary>
        /// Gets or sets url used to load "Error" svg image.
        /// </summary>
        public static string Error { get; set; } = GetImageUrl("circle-xmark-red");

        /// <summary>
        /// Gets or sets url used to load "Warning" svg image.
        /// </summary>
        public static string Warning { get; set; } = GetImageUrl("triangle-exclamation-yellow");

        /// <summary>
        /// Gets or sets url used to load "Information" svg image.
        /// </summary>
        public static string Information { get; set; } = GetImageUrl("circle-info-blue");

        private static string GetImageUrl(string name) => string.Format(ResTemplate, name);
    }
}
