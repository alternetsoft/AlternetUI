using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the main menu structure of an application or a window.
    /// </summary>
    [ControlCategory("MenusAndToolbars")]
    public partial class MainMenu : Menu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenu"/> class.
        /// </summary>
        public MainMenu()
        {
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }
    }
}