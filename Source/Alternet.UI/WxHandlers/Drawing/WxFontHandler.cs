using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.UI.Native
{
    internal partial class Font
    {
        public string GetNameAsString()
        {
            return GetName().ToString();
        }

        /// <summary>
        /// Gets whether font is using size in pixels.
        /// </summary>
        bool IsUsingSizeInPixels(Alternet.Drawing.Font font)
            => IsUsingSizeInPixels();
    }
}
