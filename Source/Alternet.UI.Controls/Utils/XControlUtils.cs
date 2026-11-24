using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides different utility methods.
    /// </summary>
    public static class XControlUtils
    {
        private static TestActionsWindow? testActionsWindow;

        /// <summary>
        /// Logs the specified file name (or a folder) with a prefix.
        /// When log item is double clicked, application
        /// opens a preview window for the file.
        /// </summary>
        /// <param name="prefix">The prefix to include in the log message.</param>
        /// <param name="fileName">The path to the file or folder to log and preview.</param>
        /// <param name="kind">The kind of log item. Default is <see cref="LogItemKind.Information"/>.</param>
        public static void LogFileName(
            string prefix,
            string fileName,
            LogItemKind kind = LogItemKind.Information)
        {
            App.LogAction($"{prefix}: {fileName}", () =>
            {
                WindowFilePreview.ShowPreviewWindow(fileName);
            });
        }

        /// <summary>
        /// Shows developer tools window.
        /// </summary>
        public static void ShowDeveloperTools()
        {
            LogListBox.RaiseShowDeveloperTools();
        }

        /// <summary>
        /// Shows a dialog with the list of test actions.
        /// </summary>
        public static void ShowTestActionsDialog()
        {
            if (testActionsWindow is null)
            {
                testActionsWindow = new();
                testActionsWindow.Disposed += (s, e) =>
                {
                    testActionsWindow = null;
                };
            }

            testActionsWindow.ShowAndFocus();
        }

        /// <summary>
        /// Adds new page with the specified title and child <see cref="PanelSettings"/> control.
        /// </summary>
        /// <param name="tabControl">The tab control to which the page will be added.</param>
        /// <param name="title">Page title.</param>
        /// <param name="configure">Action to configure the panel settings control.</param>
        /// <returns>Created panel settings.</returns>
        public static PanelSettings AddTabPanelSettings(
            this TabControl tabControl,
            string title,
            Action<PanelSettings>? configure = null)
        {
            ScrollablePanelSettings result = new();
            result.Title = title;
            configure?.Invoke(result.ScrolledControl);
            return result.ScrolledControl;
        }
    }
}
