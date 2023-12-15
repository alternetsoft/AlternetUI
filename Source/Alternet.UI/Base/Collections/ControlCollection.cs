using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a collection of <see cref="Control" /> objects.
    /// </summary>
    public class ControlCollection : Collection<Control>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlCollection"/> class.
        /// </summary>
        public ControlCollection()
            : base()
        {
            ThrowOnNullAdd = true;
        }
    }
}
