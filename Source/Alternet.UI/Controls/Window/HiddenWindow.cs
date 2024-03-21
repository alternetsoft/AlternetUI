using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// This <see cref="Window"/> descendant is always hidden.
    /// </summary>
    public class HiddenWindow : Window
    {
        /// <inheritdoc/>
        public override bool Visible
        {
            get => base.Visible;
            set => base.Visible = false;
        }
    }
}
