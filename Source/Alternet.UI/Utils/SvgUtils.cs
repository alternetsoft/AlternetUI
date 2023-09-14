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

        /// <summary>
        /// Gets or sets url used to load "plus" svg image used in "Add" toolbar buttons.
        /// </summary>
        public static string UrlImagePlus { get; set; } = GetImageUrl("plus");

        /// <summary>
        /// Gets or sets url used to load "minus" svg image used in "Remove" toolbar buttons.
        /// </summary>
        public static string UrlImageMinus { get; set; } = GetImageUrl("minus");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Ok" toolbar buttons.
        /// </summary>
        public static string UrlImageOk { get; set; } = GetImageUrl("check");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Cancel" toolbar buttons.
        /// </summary>
        public static string UrlImageCancel { get; set; } = GetImageUrl("xmark");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Add child" toolbar buttons.
        /// </summary>
        public static string UrlImageAddChild { get; set; } = GetImageUrl("alternet-add-child");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Remove All" toolbar buttons.
        /// </summary>
        public static string UrlImageRemoveAll { get; set; } = GetImageUrl("eraser");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Apply" toolbar buttons.
        /// </summary>
        public static string UrlImageApply { get; set; } = GetImageUrl("square-check");

        private static string GetImageUrl(string name) => string.Format(ResTemplate, name);
    }
}
