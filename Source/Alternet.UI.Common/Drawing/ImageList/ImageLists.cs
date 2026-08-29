using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a pair of small and large image lists,
    /// typically used for controls that require both sizes of images, such as tree views or list views.
    /// </summary>
    public class ImageLists : BaseObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLists"/> class with the specified small and large image lists.
        /// </summary>
        /// <param name="small">The small image list.</param>
        /// <param name="large">The large image list.</param>
        public ImageLists(ImageList small, ImageList large)
        {
            Small = small;
            Large = large;
        }

        /// <summary>
        /// Gets the small image list.
        /// </summary>
        public virtual ImageList Small { get; }

        /// <summary>
        /// Gets the large image list.
        /// </summary>
        public virtual ImageList Large { get; }
    }
}
