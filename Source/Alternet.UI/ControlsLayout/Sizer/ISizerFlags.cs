using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to specify <see cref="SizerFlag"/> in the convenient way.
    /// </summary>
    public interface ISizerFlags : IDisposableObject
    {
        int GetDefaultBorder();

        float GetDefaultBorderFractional();

        int GetProportion();

        SizerFlag GetFlags();

        int GetBorderInPixels();

        ISizerFlags Proportion(int proportion);

        ISizerFlags Expand();

        ISizerFlags Align(GenericAlignment alignment);

        ISizerFlags Center();

        ISizerFlags CenterVertical();

        ISizerFlags CenterHorizontal();

        ISizerFlags Top();

        ISizerFlags Left();

        ISizerFlags Right();

        ISizerFlags Bottom();

        ISizerFlags Border(GenericDirection direction, int borderInPixels);

        ISizerFlags Border(GenericDirection direction = GenericDirection.All);

        ISizerFlags DoubleBorder(GenericDirection direction = GenericDirection.All);

        ISizerFlags TripleBorder(GenericDirection direction = GenericDirection.All);

        ISizerFlags HorzBorder();

        ISizerFlags DoubleHorzBorder();

        ISizerFlags Shaped();

        ISizerFlags FixedMinSize();

        /// <summary>
        /// Makes the item ignore window's visibility status.
        /// </summary>
        ISizerFlags ReserveSpaceEvenIfHidden();
    }
}
