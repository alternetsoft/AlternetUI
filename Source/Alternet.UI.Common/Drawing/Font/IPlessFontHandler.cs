using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    public interface IPlessFontHandler : IFontHandler
    {
        /// <summary>
        /// Gets or sets a description string that represents the font.
        /// </summary>
        /// <returns></returns>
        new string Description { get; set; }

        /// <summary>
        /// Gets of sets name of the font.
        /// </summary>
        new string Name { get; set; }

        /// <summary>
        /// Gets or sets style information for the font.
        /// </summary>
        /// <returns></returns>
        new FontStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the em-size, in points, of the font.
        /// </summary>
        new double SizeInPoints { get; set; }

        void SetEncoding(int value);

        void SetIsFixedWidth(bool value);
    }
}
