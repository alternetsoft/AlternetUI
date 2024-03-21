using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    public partial class Control
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
            {
                ThrowOnNullAdd = true;
            }

            /// <summary>
            /// Creates clone of this object.
            /// </summary>
            /// <returns></returns>
            public ControlCollection Clone()
            {
                var result = new ControlCollection();
                result.AddRange(this);
                return result;
            }
        }
    }
}
