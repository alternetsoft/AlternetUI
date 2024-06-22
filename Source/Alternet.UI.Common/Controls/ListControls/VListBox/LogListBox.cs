using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ListBox"/> descendant with log and debug related functionality.
    /// </summary>
    [ControlCategory("Other")]
    public partial class LogListBox : VListBox
    {
        /// <summary>
        /// Gets or sets image used for error messages.
        /// </summary>
        public static Image? ErrorImage = null;

        /// <summary>
        /// Gets or sets image used for warning messages.
        /// </summary>
        public static Image? WarningImage = null;

        /// <summary>
        /// Gets or sets image used for information messages.
        /// </summary>
        public static Image? InformationImage = null;

        private ContextMenuStrip? contextMenu;
        private string? lastLogMessage;
        private MenuItem? menuItemShowDevTools;
        private bool boundToApplicationLog;
        private bool showDebugWelcomeMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogListBox"/> class.
        /// </summary>
        public LogListBox()
        {
            HScrollBarVisible = true;
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
        public virtual string? LastLogMessage
        {
            get
            {
                if(Items.Count > 0)
                    return lastLogMessage;
                return null;
            }
        }

        /// <summary>
        /// Gets or sets whether debug welcome message is logged at the application start.
        /// Default value is <c>false</c>.
        /// </summary>
        public bool ShowDebugWelcomeMessage
        {
            get => showDebugWelcomeMessage;

            set
            {
                if (showDebugWelcomeMessage == value || showDebugWelcomeMessage)
                    return;
                showDebugWelcomeMessage = value;
                LogUtils.ShowDebugWelcomeMessage = true;
                LogUtils.DebugLogVersion();
            }
        }

        /// <summary>
        /// Gets or sets whether to bind this control to the application log.
        /// </summary>
        public bool BoundToApplicationLog
        {
            get
            {
                return boundToApplicationLog;
            }

            set
            {
                if (boundToApplicationLog == value)
                    return;
                if (value)
                    BindApplicationLog();
                else
                    UnbindApplicationLog();
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
        /// Same as <see cref="App.LogReplace"/> but
        /// uses only this control for the logging.
        /// </summary>
        /// <param name="message">Message text.</param>
        /// <param name="prefix">Message text prefix.</param>
        /// <param name="kind">Message kind.</param>
        /// <remarks>
        /// If last logged message
        /// contains <paramref name="prefix"/>, last log item is replaced with
        /// <paramref name="message"/> instead of adding new log item.
        /// </remarks>
        public virtual LogListBoxItem LogReplace(
            string? message,
            string? prefix,
            LogItemKind kind = LogItemKind.Information)
        {
            var result = LogReplaceInternal(message, prefix, kind);
            LogRefresh();
            return result;
        }

        /// <summary>
        /// Binds this control to show messages which are logged with
        /// <see cref="App.Log"/>.
        /// </summary>
        public virtual void BindApplicationLog()
        {
            if (boundToApplicationLog)
                return;

            boundToApplicationLog = true;
            ContextMenu.Required();
            App.LogMessage += Application_LogMessage;
            App.LogRefresh += Application_LogRefresh;
            LogUtils.DebugLogVersion();
        }

        /// <summary>
        /// Unbinds this control from the application log.
        /// </summary>
        public virtual void UnbindApplicationLog()
        {
            if (!boundToApplicationLog)
                return;
            boundToApplicationLog = false;

            App.LogMessage -= Application_LogMessage;
            App.LogRefresh -= Application_LogRefresh;
        }

        /// <summary>
        /// Same as <see cref="App.Log"/> but
        /// uses only this control for the logging.
        /// </summary>
        /// <param name="message">Message text.</param>
        /// <param name="kind">Message kind.</param>
        public virtual LogListBoxItem Log(string? message, LogItemKind kind = LogItemKind.Information)
        {
            var result = LogInternal(message, kind);
            LogRefresh();
            return result;
        }

        /// <summary>
        /// Creates new empty item.
        /// </summary>
        /// <returns></returns>
        protected virtual LogListBoxItem CreateItem()
        {
            return new();
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
        protected override void DisposeManaged()
        {
            UnbindApplicationLog();
            base.DisposeManaged();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if(e.KeyData == (Keys.Control | Keys.C))
            {
                SelectedItemsToClipboard();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Initializes context menu with debug related actions.
        /// </summary>
        protected virtual void InitContextMenu()
        {
            ContextMenu.Add(new(CommonStrings.Default.ButtonClear, RemoveAll));

            ContextMenu.Add(new(CommonStrings.Default.ButtonCopy, () => SelectedItemsToClipboard()))
                .SetShortcutKeys(Keys.Control | Keys.C);

            ContextMenu.Add(new("Open log file", AppUtils.OpenLogFile));

            menuItemShowDevTools = ContextMenu.Add(new("Developer Tools"));
            menuItemShowDevTools.ClickAction = ShowDevTools;

            var logToFileItem = ContextMenu.Add(new("Enable Log to file"));
            logToFileItem.ClickAction = AlsoLogToFile;

            void AlsoLogToFile()
            {
                if (App.LogFileIsEnabled)
                    return;
                App.LogFileIsEnabled = true;
                logToFileItem.Checked = true;
                logToFileItem.Enabled = false;
            }

            void ShowDevTools()
            {
                DialogFactory.ShowDeveloperTools();
            }
        }

        private void LogRefresh()
        {
            Invoke(Fn);

            void Fn()
            {
                if (!App.LogInUpdates() || !BoundToApplicationLog)
                {
                    var index = Items.Count - 1;
                    SelectedIndex = index;
                    EnsureVisible(index);
                    Refresh();
                }
            }
        }

        private void Application_LogRefresh(object? sender, EventArgs e)
        {
            LogRefresh();
        }

        private LogListBoxItem LogInternal(string? message, LogItemKind kind)
        {
            if (IsDisposed)
                return new();

            lastLogMessage = message;

            LogListBoxItem item = CreateItem();
            item.Text = ConstructLogMessage(message);
            item.Kind = kind;
            Add(item);
            SelectedIndex = Count - 1;
            RefreshRow(Count - 1);
            EnsureVisible(Count - 1);
            return item;
        }

        private LogListBoxItem LogReplaceInternal(
            string? message,
            string? prefix,
            LogItemKind kind)
        {
            if (IsDisposed)
                return new();

            string? s;

            s = LastLogMessage;

            if (s is null)
                return Log(message, kind);

            var b = s?.StartsWith(prefix ?? string.Empty) ?? false;

            if (b)
            {
                lastLogMessage = message;
                var item = (LogListBoxItem)LastItem!;
                item.Text = ConstructLogMessage(message);
                item.Kind = kind;
                SelectedIndex = Count - 1;
                RefreshRow(Count - 1);
                EnsureVisible(Count - 1);
                return item;
            }
            else
                return LogInternal(message, kind);
        }

        private void Application_LogMessage(object? sender, LogMessageEventArgs e)
        {
            Invoke(Fn);

            void Fn()
            {
                if (e.ReplaceLastMessage)
                    LogReplaceInternal(e.Message, e.MessagePrefix, e.Kind);
                else
                    LogInternal(e.Message, e.Kind);
            }
        }

        /// <summary>
        /// Item of the <see cref="LogListBox"/> control.
        /// </summary>
        public class LogListBoxItem : ListControlItem
        {
            private LogItemKind kind = LogItemKind.Other;

            /// <summary>
            /// Gets or sets log item kind.
            /// </summary>
            public LogItemKind Kind
            {
                get => kind;

                set
                {
                    if (kind == value)
                        return;
                    kind = value;

                    var size = ToolBarUtils.GetDefaultImageSize();

                    switch (kind)
                    {
                        case LogItemKind.Error:
                            ErrorImage ??= KnownColorSvgImages.ImgError.AsImage(size.Width);
                            Image = ErrorImage;
                            break;
                        case LogItemKind.Warning:
                            WarningImage ??=
                                KnownColorSvgImages.ImgWarning.AsImage(size.Width);
                            Image = WarningImage;
                            break;
                        case LogItemKind.Information:
                            InformationImage ??=
                                KnownColorSvgImages.ImgInformation.AsImage(size.Width);
                            Image = InformationImage;
                            break;
                        default:
                            Image = null;
                            break;
                    }
                }
            }
        }
    }
}
