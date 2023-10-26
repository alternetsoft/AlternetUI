﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ListBox"/> descendant with log and debug related functionality.
    /// </summary>
    public class LogListBox : ListBox
    {
        private ContextMenu? contextMenu;

        /// <summary>
        /// Gets context menu for the control.
        /// </summary>
        public ContextMenu ContextMenu
        {
            get
            {
                if (contextMenu == null)
                {
                    contextMenu = new();
                    InitContextMenu();
                    MouseRightButtonDown += Control_ShowMenu;
                }

                return contextMenu;
            }
        }

        /// <summary>
        /// Same as <see cref="Application.LogReplace(string, string)"/> but
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
            string? s;

            s = LastItem?.ToString();

            if (s is null)
            {
                Log(message);
                return;
            }

            var b = s?.StartsWith(prefix ?? string.Empty) ?? false;

            if (b)
            {
                LastItem = ConstructLogMessage(message);
                SelectedIndex = Items.Count - 1;
            }
            else
                Log(message);

            Application.DoEvents();
        }

        /// <summary>
        /// Binds this control to show messages which are logged with
        /// <see cref="Application.Log"/>.
        /// </summary>
        public virtual void BindApplicationLog()
        {
            ContextMenu.Required();
            Application.Current.LogMessage += Application_LogMessage;
        }

        /// <summary>
        /// Same as <see cref="Application.Log"/> but
        /// uses only this control for the logging.
        /// </summary>
        /// <param name="message">Message text.</param>
        public virtual void Log(string? message)
        {
            Add(ConstructLogMessage(message));
            SelectedIndex = Items.Count - 1;

            Application.DoEvents();
        }

        internal virtual void Application_LogMessage(object? sender, LogMessageEventArgs e)
        {
            if (e.ReplaceLastMessage)
                LogReplace(e.Message, e.MessagePrefix);
            else
                Log(e.Message);
        }

        /// <summary>
        /// Adds additional information to the log messages.
        /// </summary>
        /// <param name="msg">Log message.</param>
        /// <remarks>
        /// By default adds unique integer identifier to the end of the <paramref name="msg"/>.
        /// </remarks>
        protected virtual string ConstructLogMessage(string? msg)
        {
            return $"{msg} [{LogUtils.GenNewId()}]";
        }

        /// <summary>
        /// Initializes context menu with debug related actions.
        /// </summary>
        protected virtual void InitContextMenu()
        {
            ContextMenu.Add(new(CommonStrings.Default.ButtonClear, RemoveAll));

            ContextMenu.Add(new("Open log file", LogUtils.OpenLogFile));

#if DEBUG
            /*Add(
             new("C++ Throw", () => { WebBrowser.DoCommandGlobal("CppThrow"); }));*/
#endif
        }

        private void Control_ShowMenu(object? sender, MouseButtonEventArgs e)
        {
            ShowPopupMenu(ContextMenu);
        }
    }
}
