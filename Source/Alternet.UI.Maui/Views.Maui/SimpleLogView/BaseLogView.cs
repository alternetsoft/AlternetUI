using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents an abstract log view.
    /// </summary>
    public partial class BaseLogView : BaseContentView
    {
        /// <summary>
        /// Delegate to create a log view.
        /// </summary>
        public static Func<BaseLogView>? CreateLogView;

        /// <summary>
        /// Gets or sets a value indicating whether Debug.WriteLine is called
        /// when log item is added.
        /// </summary>
        public static bool IsDebugWriteLineCalled = false;

        /// <summary>
        /// Creates the default log view.
        /// </summary>
        /// <returns>A new instance of the default log view.</returns>
        public static BaseLogView CreateDefaultLogView()
        {
            if(CreateLogView is null)
                return new CollectionLogView();
            return CreateLogView();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the log view should scroll to the end when its size changes.
        /// </summary>
        public virtual bool ScrollToEndOnSizeChanged { get; set; } = true;

        /// <summary>
        /// Clears all log items.
        /// </summary>
        public virtual void Clear()
        {
        }

        /// <summary>
        /// Moves the view to the beginning of the log items.
        /// </summary>
        public virtual void GoToBegin()
        {
        }

        /// <summary>
        /// Binds the application log so items will be shown in this view.
        /// </summary>
        public virtual void BindApplicationLog()
        {
            Alternet.UI.App.LogMessage += HandleAppLogMessage;
        }

        /// <summary>
        /// Unbinds the application log previously bound with <see cref="BindApplicationLog"/>.
        /// </summary>
        public virtual void UnbindApplicationLog()
        {
            Alternet.UI.App.LogMessage -= HandleAppLogMessage;
        }

        /// <summary>
        /// Adds a log item to the view.
        /// </summary>
        /// <param name="s">The log message to add.</param>
        public virtual void Add(string? s)
        {
            if (s is null)
                return;
            try
            {
                if (IsDebugWriteLineCalled)
                    Debug.WriteLine(s);

                Alternet.UI.App.AddBackgroundInvokeAction(() =>
                {
                    AddItem(s);
                });
            }
            catch
            {
            }
        }

        /// <summary>
        /// Protected method that called from <see cref="Add"/> in order to
        /// add a log item to the view.
        /// </summary>
        /// <param name="s">The log message to add.</param>
        protected virtual void AddItem(string s)
        {
        }

        /// <summary>
        /// Handles the application log message event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments containing the log message.</param>
        protected void HandleAppLogMessage(object? sender, Alternet.UI.LogMessageEventArgs e)
        {
            Add(e.Message);
        }

        /// <summary>
        /// Represents a log item with a message.
        /// </summary>
        protected class LogItem
        {
            /// <summary>
            /// Gets or sets the message of the log item.
            /// </summary>
            public string Message = string.Empty;

            /// <summary>
            /// Initializes a new instance of the <see cref="LogItem"/> class with
            /// the specified message.
            /// </summary>
            /// <param name="message">The message to associate with the log item.</param>
            public LogItem(string message)
            {
                Message = message;
            }

            /// <summary>
            /// Returns the message of the log item as a string.
            /// </summary>
            /// <returns>The message of the log item.</returns>
            public override string ToString()
            {
                return Message;
            }
        }
    }
}
