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
    /// Contains URLs for svg images that are included in the library resources.
    /// </summary>
    public static class KnownAnimatedImageUrls
    {
        /// <summary>
        /// Gets template for the gif resource URLs used by <see cref="GetImageUrl(string)"/>.
        /// </summary>
        public static string ResTemplate { get; } =
            "embres:Alternet.UI.Common.Resources.Animation.{0}?assembly=Alternet.UI.Common";

        /// <summary>
        /// Gets or sets a delegate that provides a custom image URL for a given input string.
        /// </summary>
        /// <remarks>If set, this delegate is invoked to determine the image URL based on the input. If
        /// null, the default image URL resolution is used. This can be used to override or customize image sourcing
        /// logic at runtime.</remarks>
        public static Func<string, string>? ImageUrlOverride;

        /// <summary>
        /// Gets or sets url used to load "DualRing32" image.
        /// </summary>
        public static string DualRing32 { get; set; } = GetImageUrl("DualRing32.gif");

        /// <summary>
        /// Gets or sets url used to load "DualRing64" image.
        /// </summary>
        public static string DualRing64 { get; set; } = GetImageUrl("DualRing64.gif");

        /// <summary>
        /// Returns the URL of the image resource associated with the specified name.
        /// </summary>
        /// <remarks>If an image URL override is configured, this method uses the override to generate the
        /// URL; otherwise, it constructs the URL using a predefined template. This method does not validate whether the
        /// resulting URL points to an existing resource.</remarks>
        /// <param name="name">The name of the image resource for which to retrieve the URL. Cannot be null.</param>
        /// <returns>A string containing the URL of the image resource. If no override is set, the URL is generated using a
        /// default template.</returns>
        public static string GetImageUrl(string name)
        {
            if (ImageUrlOverride != null)
                return ImageUrlOverride(name);
            return string.Format(ResTemplate, name);
        }
    }
}