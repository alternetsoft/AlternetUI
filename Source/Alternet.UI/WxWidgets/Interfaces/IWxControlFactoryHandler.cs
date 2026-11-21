using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a factory handler for creating platform-specific control handlers.
    /// </summary>
    /// <remarks>This interface extends <see cref="IControlFactoryHandler"/> to provide
    /// additional functionality for creating handlers.
    /// Implementations of this interface are responsible for returning appropriate
    /// platform-specific handlers.</remarks>
    public interface IWxControlFactoryHandler : IControlFactoryHandler
    {
        /// <summary>
        /// Creates a handler for a TreeView control.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> instance representing
        /// the TreeView to be handled. Must not be <see langword="null"/>.</param>
        /// <returns>An <see cref="IControlHandler"/> instance that provides
        /// platform-specific functionality for the TreeView control.</returns>
        IControlHandler CreateTreeViewHandler(Control control);

        /// <summary>
        /// Creates a handler for the specified <see cref="Slider"/> control.
        /// </summary>
        /// <param name="control">The <see cref="Slider"/> control for which the handler
        /// is to be created. Cannot be <see langword="null"/>.</param>
        /// <returns>An <see cref="IControlHandler"/> instance that manages
        /// the behavior and rendering of the specified <see cref="Slider"/> control.</returns>
        IControlHandler CreateSliderHandler(Slider control);
    }
}
