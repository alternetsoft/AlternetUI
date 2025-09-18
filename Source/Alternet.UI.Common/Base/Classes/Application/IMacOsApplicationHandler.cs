using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines methods and properties for handling macOS-specific application functionality.
    /// </summary>
    public interface IMacOsApplicationHandler : IApplicationHandler
    {
        /// <summary>
        /// Sets the common menu bar for the application on macOS.
        /// </summary>
        /// <param name="menuBar">
        /// The <see cref="MainMenu"/> to set as the common menu bar, or <c>null</c> to remove it.
        /// </param>
        void MacSetCommonMenuBar(MainMenu? menuBar);

        /// <summary>
        /// Gets the title name of the Help menu for macOS.
        /// </summary>
        string? HelpMenuTitleName { get; }

        /// <summary>
        /// Gets the title name of the Window menu for macOS.
        /// </summary>
        string? WindowMenuTitleName { get; }
    }
}
