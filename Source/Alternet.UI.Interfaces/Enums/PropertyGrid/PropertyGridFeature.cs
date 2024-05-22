using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to turn on/off different features in <see cref="PropertyGrid"/>.
    /// </summary>
    [Flags]
    public enum PropertyGridFeature
    {
        /// <summary>
        /// Adds '?' character to the property label if property type is nullable
        /// (for example: 'byte?', 'string?').
        /// </summary>
        QuestionCharInNullable,
    }
}
