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
    public partial class LogListBox : TreeView
    {
        /// <summary>
        /// Indicates whether message identifiers should be displayed in the log.
        /// </summary>
        public static bool ShowMessageIdentifier = true;

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
            ListBox.SelectionMode = ListBoxSelectionMode.Multiple;

            ListBox.KeyDown += (s, e) =>
            {
                if (e.KeyData == (Keys.Control | Keys.C))
                {
                    ListBox.SelectedItemsToClipboard();
                    e.Handled = true;
                }
            };
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
                if(RootItem.ItemCount > 0)
                    return lastLogMessage;
                return null;
            }
        }

        /// <summary>
        /// Gets or sets whether debug welcome message is logged at the application start.
        /// Default value is <c>false</c>.
        /// </summary>
        public virtual bool ShowDebugWelcomeMessage
        {
            get => showDebugWelcomeMessage;

            set
            {
                if (showDebugWelcomeMessage == value)
                    return;
                showDebugWelcomeMessage = value;
                LogUtils.ShowDebugWelcomeMessage = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to bind this control to the application log.
        /// </summary>
        public virtual bool BoundToApplicationLog
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
            LogUtils.DebugLogVersion(ShowDebugWelcomeMessage);
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
        }

        /// <summary>
        /// Same as <see cref="App.Log"/> but
        /// uses only this control for the logging.
        /// </summary>
        /// <param name="obj">Message text.</param>
        /// <param name="kind">Message kind.</param>
        public virtual TreeViewItem Log(object? obj, LogItemKind kind = LogItemKind.Information)
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
        protected virtual TreeViewItem CreateItem()
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

            if(ShowMessageIdentifier)
                return $" [{id}] {msg}";
            return msg?.ToString() ?? string.Empty;
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
        protected override void InitContextMenu()
        {
            ContextMenu.Add(new(CommonStrings.Default.ButtonClear, Clear));

            ContextMenu.Add(new(CommonStrings.Default.ButtonCopy, () => ListBox.SelectedItemsToClipboard()))
                .SetShortcutKeys(Keys.Control | Keys.C);

            ContextMenu.Add(new("Open log file", AppUtils.OpenLogFile));

            menuItemShowDevTools = ContextMenu.Add("Developer Tools...", ShowDevTools);

            ContextMenu.Add("Test Actions...", LogUtils.ShowTestActionsDialog);

            ContextMenu.Add("Search for Members...", () =>
            {
                WindowSearchForMembers.Default.ShowAndFocus();
            });

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
            if (DisposingOrDisposed)
                return;

            Invoke(Fn);

            void Fn()
            {
                if (DisposingOrDisposed)
                    return;

                if (!App.LogInUpdates() || !BoundToApplicationLog)
                {
                    SelectLastItemAndScroll();
                }
            }
        }

        private void Application_LogRefresh(object? sender, EventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            LogRefresh();
        }

        private TreeViewItem LogItemInternal(int id, TreeViewItem item, LogItemKind kind)
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
            SelectLastItemAndScroll();
            return item;
        }

        private TreeViewItem LogInternal(int id, object? obj, LogItemKind kind)
        {
            if (DisposingOrDisposed)
                return new();

            var message = obj.SafeToString();

            lastLogMessage = message;

            TreeViewItem item;

            if(message == LogUtils.SectionSeparator || message == "-")
            {
                item = new TreeControlSeparatorItem();
            }
            else
            {
                item = CreateItem();
                item.Text = ConstructLogMessage(id, message);
                item.SvgImage = GetImage(kind);
                item.SvgImageSize = ToolBarUtils.GetDefaultImageSize(this);
            }

            Add(item);
            SelectLastItemAndScroll();
            return item;
        }

        private TreeViewItem LogReplaceInternal(
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
                var item = RootItem.LastChild!;
                item.Text = ConstructLogMessage(id, message);
                item.SvgImage = GetImage(kind);
                item.SvgImageSize = ToolBarUtils.GetDefaultImageSize(this);
                ListBox.RefreshLastRow();

                SelectLastItemAndScroll();
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
                if (DisposingOrDisposed)
                    return;

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
