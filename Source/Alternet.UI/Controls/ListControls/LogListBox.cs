﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Localization;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ListBox"/> descendant with log and debug related functionality.
    /// </summary>
    [ControlCategory("Other")]
    public partial class LogListBox : ListBox
    {
        private ContextMenuStrip? contextMenu;
        private string? lastLogMessage;
        private MenuItem? menuItemShowDevTools;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogListBox"/> class.
        /// </summary>
        public LogListBox()
        {
            SelectionMode = ListBoxSelectionMode.Multiple;
        }

        /// <summary>
        /// Gets 'Developer Tools' menu item.
        /// </summary>
        public MenuItem? MenuItemShowDevTools => menuItemShowDevTools;

        /// <summary>
        /// Gets the last logged message.
        /// </summary>
        [Browsable(false)]
        public string? LastLogMessage
        {
            get
            {
                if(Items.Count > 0)
                    return lastLogMessage;
                return null;
            }
        }

        /// <summary>
        /// Gets context menu for the control.
        /// </summary>
        [Browsable(false)]
        public ContextMenuStrip ContextMenu
        {
            get
            {
                if (contextMenu == null)
                {
                    contextMenu = new();
                    InitContextMenu();
                    ContextMenuStrip = contextMenu;
                }

                return contextMenu;
            }
        }

        /// <summary>
        /// Same as <see cref="Application.LogReplace"/> but
        /// uses only this control for the logging.
        /// </summary>
        /// <param name="message">Message text.</param>
        /// <param name="prefix">Message text prefix.</param>
        /// <remarks>
        /// If last logged message
        /// contains <paramref name="prefix"/>, last log item is replaced with
        /// <paramref name="message"/> instead of adding new log item.
        /// </remarks>
        public virtual void LogReplace(string? message, string? prefix)
        {
            if (IsDisposed)
                return;

            string? s;

            s = LastLogMessage;

            if (s is null)
            {
                Log(message);
                return;
            }

            var b = s?.StartsWith(prefix ?? string.Empty) ?? false;

            if (b)
            {
                lastLogMessage = message;
                LastItem = ConstructLogMessage(message);
                SelectedIndex = Items.Count - 1;
                EnsureVisible(SelectedIndex.Value);
                Refresh();
            }
            else
                Log(message);
        }

        /// <summary>
        /// Binds this control to show messages which are logged with
        /// <see cref="Application.Log"/>.
        /// </summary>
        public virtual void BindApplicationLog()
        {
            ContextMenu.Required();
            Application.Current.LogMessage += Application_LogMessage;
            LogUtils.DebugLogVersion();
        }

        /// <summary>
        /// Same as <see cref="Application.Log"/> but
        /// uses only this control for the logging.
        /// </summary>
        /// <param name="message">Message text.</param>
        public virtual void Log(string? message)
        {
            if (IsDisposed)
                return;

            lastLogMessage = message;
            Add(ConstructLogMessage(message));
            SelectedIndex = Items.Count - 1;
            EnsureVisible(SelectedIndex.Value);
            Refresh();
        }

        /// <summary>
        /// Adds additional information to the log messages.
        /// </summary>
        /// <param name="msg">Log message.</param>
        /// <remarks>
        /// By default adds unique integer identifier to the beginning of the <paramref name="msg"/>.
        /// </remarks>
        protected virtual string ConstructLogMessage(string? msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return string.Empty;
            return $" [{LogUtils.GenNewId()}] {msg}";
        }

        /// <inheritdoc/>
        protected override void DisposeResources()
        {
            Application.Current.LogMessage -= Application_LogMessage;
            base.DisposeResources();
        }

        /// <summary>
        /// Initializes context menu with debug related actions.
        /// </summary>
        protected virtual void InitContextMenu()
        {
            void Copy()
            {
                var text = SelectedItemsAsText();
                if (string.IsNullOrEmpty(text))
                    return;
                Clipboard.SetText(text!);
            }

            ContextMenu.Add(new(CommonStrings.Default.ButtonClear, RemoveAll));

            ContextMenu.Add(new(CommonStrings.Default.ButtonCopy, Copy));

            ContextMenu.Add(new("Open log file", LogUtils.OpenLogFile));

            menuItemShowDevTools = ContextMenu.Add(new("Developer Tools"));
            menuItemShowDevTools.ClickAction = ShowDevTools;

            var logToFileItem = ContextMenu.Add(new("Enable Log to file"));
            logToFileItem.ClickAction = AlsoLogToFile;

            void AlsoLogToFile()
            {
                if (Application.LogFileIsEnabled)
                    return;
                Application.LogFileIsEnabled = true;
                logToFileItem.Checked = true;
                logToFileItem.Enabled = false;
            }

            void ShowDevTools()
            {
                DebugUtils.ShowDeveloperTools();
            }
        }

        private void Application_LogMessage(object? sender, LogMessageEventArgs e)
        {
            if (e.ReplaceLastMessage)
                LogReplace(e.Message, e.MessagePrefix);
            else
                Log(e.Message);
        }
    }
}
