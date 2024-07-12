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
    /// Dummy implementation of the <see cref="IIconSetHandler"/> interface.
    /// </summary>
    public class DummyIconSetHandler : DummyImageContainer, IIconSetHandler
    {
        /// <summary>
        /// Gets or sets default dummy implementation of the <see cref="IIconSetHandler"/> interface.
        /// </summary>
        public static IIconSetHandler Default = new DummyIconSetHandler();
    }
}
