using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the log events.
    /// </summary>
    public class LogMessageEventArgs : EventArgs
    {
        private readonly string? prefix;
        private readonly bool replaceLast;
        private string? message;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangeEventArgs"/> class.
        /// </summary>
        /// <param name="message">Message that needs to be logged.</param>
        public LogMessageEventArgs(string? message = null)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessageEventArgs"/> class.
        /// </summary>
        /// <param name="message">Message that needs to be logged.</param>
        /// <param name="prefix">Message text prefix.</param>
        /// <param name="replaceLast">If <c>true</c>, last logged message must be replaced
        /// with the <paramref name="message"/> (if old message starts from
        /// <paramref name="prefix"/>).</param>
        public LogMessageEventArgs(string message, string prefix, bool replaceLast)
        {
            this.message = message;
            this.prefix = prefix;
            this.replaceLast = replaceLast;
        }

        /// <summary>
        /// Message that needs to be logged.
        /// </summary>
        public string? Message { get => message; set => message = value; }

        /// <summary>
        /// Message text prefix.
        /// </summary>
        public string? MessagePrefix => prefix;

        /// <summary>
        /// If <c>true</c>, last logged message must be replaced
        /// with the <see cref="Message"/> (if old message starts from
        /// <see cref="MessagePrefix"/>).
        /// </summary>
        public bool ReplaceLastMessage => replaceLast;
    }
}
