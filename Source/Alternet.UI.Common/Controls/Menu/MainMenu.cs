using System;
using System.ComponentModel;

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

        /// <summary>
        /// Gets a value indicating whether this <see cref="MainMenu"/> is currently attached to a window.
        /// </summary>
        [Browsable(false)]
        public bool IsAttached
        {
            get
            {
                return AttachedWindow != null;
            }
        }

        /// <inheritdoc/>
        public override MainMenu? MenuBar
        {
            get => this;
        }

        /// <summary>
        /// Gets the <see cref="Window"/> instance to which this menu is attached, if any.
        /// </summary>
        [Browsable(false)]
        public virtual Window? AttachedWindow
        {
            get
            {
                foreach (var window in App.Current.Windows)
                {
                    if (window.Menu == this)
                        return window;
                }

                return null;
            }
        }

        /// <inheritdoc/>
        protected override void OnItemChanged(Menu item, MenuChangeKind action)
        {
            base.OnItemChanged(item, action);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }
    }
}