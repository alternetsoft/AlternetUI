using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Gives access to the 'Text' property of the object.
    /// </summary>
    public interface ITextProperty
    {
        /// <summary>
        /// Gets or sets text property of the object.
        /// </summary>
        string Text { get; set; }
    }
}
