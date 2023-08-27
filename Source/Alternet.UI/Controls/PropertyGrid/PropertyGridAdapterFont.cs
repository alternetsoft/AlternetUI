using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Helper class for using <see cref="Font"/> properties in the <see cref="PropertyGrid"/>.
    /// </summary>
    public class PropertyGridAdapterFont : PropertyGridAdapterGeneric
    {
        /// <summary>
        /// Returns <see cref="PropertyGridAdapterGeneric.Value"/> as <see cref="Font"/>.
        /// </summary>
        public Font? Font
        {
            get => Value as Font;
            set => Value = value;
        }

        /*
        Font readonly
            double SizeInPoints
            string Name,
            FontStyle Style         
         */

    }
}