using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Used as a container for other controls.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class Panel : UserPaintControl
    {
        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Panel;
    }
}