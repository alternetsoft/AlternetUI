using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Used as a container for other controls.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class Panel : ContainerControl
    {
        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Panel;

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }
    }
}