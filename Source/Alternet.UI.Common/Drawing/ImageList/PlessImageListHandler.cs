using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements dummy <see cref="IImageListHandler"/> provider.
    /// </summary>
    public class PlessImageListHandler : PlessImageContainer, IImageListHandler
    {
        /// <inheritdoc/>
        public SizeI Size { get; set; }
    }
}
