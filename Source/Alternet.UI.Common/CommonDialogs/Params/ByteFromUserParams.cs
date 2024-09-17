using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains properties which allow to customize dialog box which asks a <see cref="byte"/>
    /// value from the user.
    /// </summary>
    public class ByteFromUserParams : LongFromUserParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ByteFromUserParams"/> class.
        /// </summary>
        public ByteFromUserParams()
        {
            MinValue = byte.MinValue;
            MaxValue = byte.MaxValue;
        }
    }
}
