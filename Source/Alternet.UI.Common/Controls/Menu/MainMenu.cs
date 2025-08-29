using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the main menu structure of an application or a window.
    /// </summary>
    [ControlCategory("MenusAndToolbars")]
    public partial class MainMenu : Menu
    {
        private IMainMenuHandler? handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenu"/> class.
        /// </summary>
        public MainMenu()
        {
        }

        /// <summary>
        /// Gets the handler responsible for managing main menu interactions.
        /// </summary>
        /// <remarks>The handler is lazily initialized when accessed for the first time.
        /// Subsequent accesses return the same instance.</remarks>
        public IMainMenuHandler Handler
        {
            get
            {
                handler ??= CreateHandler();
                return handler;
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            SafeDispose(ref handler);
            base.DisposeManaged();
        }

        /// <summary>
        /// Creates and returns a new instance of an object that implements
        /// the <see cref="IMainMenuHandler"/> interface.
        /// </summary>
        /// <remarks>This method is intended to be overridden in derived classes
        /// to provide a custom implementation of the main menu handler.
        /// By default, it uses the <see cref="ControlFactory.Handler"/> to
        /// create the handler.</remarks>
        /// <returns>An instance of a class that implements
        /// the <see cref="IMainMenuHandler"/> interface.</returns>
        protected virtual IMainMenuHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateMainMenuHandler(this);
        }
    }
}