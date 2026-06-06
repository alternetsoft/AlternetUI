using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to work with image sets.
    /// </summary>
    public interface IImageSetHandler : IImageContainer
    {
        /// <summary>
        /// Gets as image with the specified size.
        /// </summary>
        /// <param name="size">Image size.</param>
        /// <returns></returns>
        Image AsImage(SizeI size);
    }
}
