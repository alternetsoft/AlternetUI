using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle an event with <see cref="UrlEventArgs"/> event data.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An object that contains an event data.</param>
    public delegate void UrlEventHandler(object? sender, UrlEventArgs e);

    /// <summary>
    /// Provides data for the events that have an url argument.
    /// </summary>
    public class UrlEventArgs : CancelEventArgs
    {
        private string? url;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlEventArgs"/> class.
        /// </summary>
        /// <param name="url">Url.</param>
        public UrlEventArgs(string? url)
        {
            this.url = url;
        }

        /// <summary>
        /// Gets or sets url event argument.
        /// </summary>
        public string? Url
        {
            get => url;
            set => url = value;
        }
    }
}
