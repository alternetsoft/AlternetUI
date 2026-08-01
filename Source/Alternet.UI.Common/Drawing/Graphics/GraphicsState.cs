using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents the state of a <see cref="Graphics"/> object.
    /// This object is returned by a call to the <see cref="Graphics.Save"/> methods.
    /// </summary>
    public readonly struct GraphicsState
    {
        internal GraphicsState(int state)
        {
            State = state;
        }

        internal int State { get; }
    }
}
