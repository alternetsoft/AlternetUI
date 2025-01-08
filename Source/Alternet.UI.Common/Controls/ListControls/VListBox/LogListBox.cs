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
    public partial class LogListBox : VirtualListBox
    {
        /// <summary>
        /// Gets or sets image used for error messages.
        /// </summary>
        public static SvgImage? ErrorImage = null;

        /// <summary>
        /// Gets or sets image used for warning messages.
        /// </summary>
        public static SvgImage? WarningImage = null;

        /// <summary>
        /// Gets or sets image used for information messages.
        /// </summary>
        public static SvgImage? InformationImage = null;

        private ContextMenuStrip? contextMenu;
        private string? lastLogMessage;
        private MenuItem? menuItemShowDevTools;
        private bool boundToApplicationLog;
        private bool showDebugWelcomeMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogListBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public LogListBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogListBox"/> class.
        /// </summary>
        public LogListBox()
        {
            HorizontalScrollbar = true;
            SelectionMode = ListBoxSelectionMode.Multiple;
        }

        /// <summary>
        /// Gets 'Developer Tools' menu item.
        /// </summary>
        [Browsable(false)]
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
        /// Gets default <see cref="SvgImage"/> for the specified <see cref="LogItemKind"/>.
        /// </summary>
        public static SvgImage? GetDefaultImage(LogItemKind kind)
        {
            switch (kind)
            {
                case LogItemKind.Error:
                    ErrorImage ??= KnownColorSvgImages.ImgError;
                    return ErrorImage;
                case LogItemKind.Warning:
                    WarningImage ??= KnownColorSvgImages.ImgWarning;
                    return WarningImage;
                case LogItemKind.Information:
                    InformationImage ??= KnownColorSvgImages.ImgInformation;
                    return InformationImage;
                default:
                    return null;
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
        public virtual ListControlItem LogReplace(
            string? message,
            string? prefix,
            LogItemKind kind = LogItemKind.Information)
        {
            var result = LogReplaceInternal(LogUtils.GenNewId(), message, prefix, kind);
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
        /// <param name="obj">Message text.</param>
        /// <param name="kind">Message kind.</param>
        public virtual ListControlItem Log(object? obj, LogItemKind kind = LogItemKind.Information)
        {
            var result = LogInternal(LogUtils.GenNewId(), obj, kind);
            LogRefresh();
            return result;
        }

        /// <summary>
        /// Calls <see cref="Log"/> if <paramref name="condition"/> is <c>true</c>.
        /// </summary>
        /// <param name="obj">Message text.</param>
        /// <param name="kind">Message kind.</param>
        /// <param name="condition">Whether to log message.</param>
        public void LogIf(object? obj, bool condition, LogItemKind kind = LogItemKind.Information)
        {
            if(condition)
                Log(obj, kind);
        }

        /// <summary>
        /// Creates new empty item.
        /// </summary>
        /// <returns></returns>
        protected virtual ListControlItem CreateItem()
        {
            return new();
        }

        /// <summary>
        /// Adds additional information to the log messages.
        /// </summary>
        /// <param name="msg">Message to log.</param>
        /// <param name="id">Message id.</param>
        /// <remarks>
        /// By default adds unique integer identifier to the beginning of the <paramref name="msg"/>.
        /// </remarks>
        protected virtual string ConstructLogMessage(int id, string? msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return string.Empty;
            return $" [{id}] {msg}";
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            UnbindApplicationLog();
            base.DisposeManaged();
        }

        /// <summary>
        /// Gets <see cref="SvgImage"/> for the specified <see cref="LogItemKind"/>.
        /// </summary>
        protected virtual SvgImage? GetImage(LogItemKind kind)
        {
            return GetDefaultImage(kind);
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

            menuItemShowDevTools = ContextMenu.Add("Developer Tools...", ShowDevTools);

            ContextMenu.Add("Test Actions...", LogUtils.ShowTestActionsDialog);

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
            if (DisposingOrDisposed)
                return;

            LogRefresh();
        }

        private ListControlItem LogItemInternal(int id, ListControlItem item, LogItemKind kind)
        {
            if (IsDisposed)
                return item;

            if (!item.HasImageOrSvg)
            {
                item.SvgImage = GetImage(kind);
                item.SvgImageSize = ToolBarUtils.GetDefaultImageSize(this);
            }

            lastLogMessage = item.Text;
            Add(item);
            SelectedIndex = Count - 1;
            RefreshRow(Count - 1);
            EnsureVisible(Count - 1);
            return item;
        }

        private ListControlItem LogInternal(int id, object? obj, LogItemKind kind)
        {
            if (DisposingOrDisposed)
                return new();

            var message = obj.SafeToString();

            lastLogMessage = message;

            var item = CreateItem();
            item.Text = ConstructLogMessage(id, message);
            item.SvgImage = GetImage(kind);
            item.SvgImageSize = ToolBarUtils.GetDefaultImageSize(this);
            Add(item);
            SelectedIndex = Count - 1;
            RefreshRow(Count - 1);
            EnsureVisible(Count - 1);
            return item;
        }

        private ListControlItem LogReplaceInternal(
            int id,
            string? message,
            string? prefix,
            LogItemKind kind)
        {
            if (DisposingOrDisposed)
                return new();

            string? s;

            s = LastLogMessage;

            if (s is null)
                return Log(message, kind);

            var b = s?.StartsWith(prefix ?? string.Empty) ?? false;

            if (b)
            {
                lastLogMessage = message;
                var item = LastItem!;
                item.Text = ConstructLogMessage(id, message);
                item.SvgImage = GetImage(kind);
                item.SvgImageSize = ToolBarUtils.GetDefaultImageSize(this);
                SelectedIndex = Count - 1;
                RefreshRow(Count - 1);
                EnsureVisible(Count - 1);
                return item;
            }
            else
                return LogInternal(id, message, kind);
        }

        private void Application_LogMessage(object? sender, LogMessageEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            Invoke(Fn);

            void Fn()
            {
                var item = e.Item?.Item;

                if(item is not null)
                {
                    LogItemInternal(e.Id, item, e.Kind);
                    return;
                }

                if (e.ReplaceLastMessage)
                    LogReplaceInternal(e.Id, e.Message, e.MessagePrefix, e.Kind);
                else
                    LogInternal(e.Id, e.Message, e.Kind);
            }
        }
    }
}
