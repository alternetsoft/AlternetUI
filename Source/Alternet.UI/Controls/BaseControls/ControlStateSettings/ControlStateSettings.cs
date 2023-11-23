using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies sets of objects (images, colors, borders) for different control states.
    /// </summary>
    public class ControlStateSettings
    {
        /// <summary>
        /// Gets or sets <see cref="ControlStateImages"/>.
        /// </summary>
        public ControlStateImages? Images {get; set;}

        /// <summary>
        /// Gets or sets <see cref="ControlStateImageSets"/>.
        /// </summary>
        public ControlStateImageSets? ImageSets { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ControlStateColors"/>.
        /// </summary>
        public ControlStateColors? Colors { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ControlStateBorders"/>.
        /// </summary>
        public ControlStateBorders? Borders { get; set; }
    }
}
