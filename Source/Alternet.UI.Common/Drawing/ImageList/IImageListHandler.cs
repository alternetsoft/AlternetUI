using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to work with image list.
    /// </summary>
    public interface IImageListHandler : IImageContainer
    {
        /// <summary>
        /// Gets or sets image size.
        /// </summary>
        SizeI Size { get; set; }
    }
}
