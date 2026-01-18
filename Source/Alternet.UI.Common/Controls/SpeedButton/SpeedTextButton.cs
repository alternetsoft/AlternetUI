using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="SpeedButton"/> descendant which by default shows text and no image.
    /// </summary>
    [ControlCategory("Other")]
    public partial class SpeedTextButton : SpeedButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedTextButton"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public SpeedTextButton(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedTextButton"/> class.
        /// </summary>
        public SpeedTextButton()
        {
            ImageVisible = false;
            TextVisible = true;
        }
    }
}
