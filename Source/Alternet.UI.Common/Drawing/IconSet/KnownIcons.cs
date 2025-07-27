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
    /// Provides access to commonly used icons.
    /// </summary>
    public static class KnownIcons
    {
        private static IconSet? defaultIcon;

        /// <summary>
        /// Gets template for the resource URLs.
        /// </summary>
        public static string ResTemplate { get; } =
            "embres:Alternet.UI.Common.Resources.Ico.{0}.ico?assembly=Alternet.UI.Common";

        /// <summary>
        /// Gets or sets url used to load default icon.
        /// </summary>
        public static string UrlDefault { get; set; }
            = GetImageUrl("Sample");

        /// <summary>
        /// Gets or sets the default <see cref="IconSet"/> instance.
        /// If not assigned, it will be loaded from <see cref="UrlDefault"/>.
        /// </summary>
        public static IconSet? Default
        {
            get
            {
                return defaultIcon ??= IconSet.FromUrlOrNull(UrlDefault);
            }

            set => defaultIcon = value;
        }

        private static string GetImageUrl(string name) => string.Format(ResTemplate, name);
    }
}